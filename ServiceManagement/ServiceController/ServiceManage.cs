using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace CMCode
{
    public class ServiceManage
    {
        #region Windows服务控制

        #region 安装服务
        /// <summary>
        /// 安装当前程序为服务
        /// </summary>
        /// <param name="NameService">服务名称，若不指定，默认使用当前程序名为服务名；若想指定不同于程序名称的服务名，可设置新的名称</param>
        /// <param name="descriptin">服务描述，默认无描述</param>
        /// <returns></returns>
        public static void InstallService(string NameService=null,string descriptin=null)
        {
            string serviceFileName = Assembly.GetEntryAssembly().Location;
            InstallService(serviceFileName, NameService, descriptin);
           
        }
        #endregion

        #region 卸载服务
        /// <summary>
        /// 卸载服务
        /// </summary>
        public static bool UninstallService(string NameService)
        {
            bool flag = true;
            if (IsServiceIsExisted(NameService))
            {
                try
                {
                    string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1) + NameService + ".exe";
                    UnInstallmyService(serviceFileName);
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion

        #region 检查服务存在的存在性
        /// <summary>
        /// 检查服务存在的存在性
        /// </summary>
        /// <param name=" NameService ">服务名</param>
        /// <returns>存在返回 true,否则返回 false;</returns>
        public static bool IsServiceIsExisted(string NameService)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == NameService.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 安装Windows服务
        /// <summary>
        /// 安装Windows服务
        /// </summary>
        /// <param name="execFileName">程序文件路径全名(含扩展名)</param>
        /// <param name="NameService">服务名称，若不指定，默认使用当前程序名为服务名；若想指定不同于程序名称的服务名，可设置新的名称</param>
        /// <param name="descriptin">服务描述，默认无描述</param>
        public static void InstallService(string execFileName, string NameService = null, string descriptin = null)
        {
            if (string.IsNullOrWhiteSpace(NameService))
            {
                NameService = Path.GetFileNameWithoutExtension(execFileName);
            }
            if (!IsServiceIsExisted(NameService))
            {
                //string location = executingAssembly.Location;
                //string serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1) + NameService + ".exe";

                var serviceCtl = new ServiceController();
                

                IDictionary savedState = new Hashtable();

                // 添加引用 System.Configuration.Install.dll
                using (AssemblyInstaller AssemblyInstaller1 = new AssemblyInstaller())
                {
                    try
                    {
                        AssemblyInstaller1.UseNewContext = true;
                        AssemblyInstaller1.Path = execFileName;
                        AssemblyInstaller1.Install(savedState);
                        AssemblyInstaller1.Commit(savedState);
                    }
                    catch (Exception ex)
                    {
                        AssemblyInstaller1.Rollback(savedState);
                        throw ex;
                    }
                }                
            }
        }
        #endregion

        #region 卸载Windows服务
        /// <summary>
        /// 卸载Windows服务
        /// </summary>
        /// <param name="filepath">程序文件路径</param>
        public static void UnInstallmyService(string filepath)
        {
            AssemblyInstaller AssemblyInstaller1 = new AssemblyInstaller();
            AssemblyInstaller1.UseNewContext = true;
            AssemblyInstaller1.Path = filepath;
            AssemblyInstaller1.Uninstall(null);
            AssemblyInstaller1.Dispose();
        }
        #endregion

        #region 判断window服务是否启动
        /// <summary>
        /// 判断某个Windows服务是否启动
        /// </summary>
        /// <returns></returns>
        public static bool IsServiceStart(string serviceName)
        {
            ServiceController psc = new ServiceController(serviceName);
            bool bStartStatus = false;
            try
            {
                if (!psc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    bStartStatus = true;
                }

                return bStartStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  修改服务的启动项
        /// <summary>  
        /// 修改服务的启动项 2为自动,3为手动  
        /// </summary>  
        /// <param name="startType"></param>  
        /// <param name="serviceName"></param>  
        /// <returns></returns>  
        public static bool ChangeServiceStartType(int startType, string serviceName)
        {
            try
            {
                RegistryKey regist = Registry.LocalMachine;
                RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                RegistryKey currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                RegistryKey services = currentControlSet.OpenSubKey("Services");
                RegistryKey servicesName = services.OpenSubKey(serviceName, true);
                servicesName.SetValue("Start", startType);
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;


        }
        #endregion

        #region 启动服务
        public static bool StartService(string serviceName)
        {
            bool flag = true;
            if (IsServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running && service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

        #region 停止服务
        private bool StopService(string serviceName)
        {
            bool flag = true;
            if (IsServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

        #endregion
    }
}
