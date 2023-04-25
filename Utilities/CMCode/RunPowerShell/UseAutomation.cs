using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CMCode.RunPowerShell
{
    /// <summary>
    /// 引用本地 System.Management.Automation.dll 执行PowerShell命令 【没有Async异步方法】
    /// C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Management.Automation\v4.0_3.0.0.0__31bf3856ad364e35
    /// C:\Windows\assembly\GAC_MSIL\System.Management.Automation\1.0.0.0__31bf3856ad364e35\
    /// </summary>
    public class UseAutomation
    {
        public static void Test()
        {
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Get-Process");
            ps.AddParameter("Name", "msedge");
            Collection<PSObject> results = ps.Invoke();
            foreach (PSObject result in results)
            {
                PSMemberInfoCollection<PSMemberInfo> memberInfos = result.Members;
                Debug.WriteLine(memberInfos["id"].Value);
            }

        }

        /// <summary>
        /// Runs a PowerShell script with parameters and prints the resulting pipeline objects to the console output. 
        /// </summary>
        /// <param name="scriptContents">The script file contents.</param>
        /// <param name="scriptParameters">A dictionary of parameter names and parameter values.</param>
        public void RunScript(string scriptContents, Dictionary<string, object> scriptParameters)
        {
            // create a new hosted PowerShell instance using the default runspace.
            // wrap in a using statement to ensure resources are cleaned up.

            using (PowerShell ps = PowerShell.Create())
            {
                // specify the script code to run.
                ps.AddScript(scriptContents);

                // specify the parameters to pass into the script.
                ps.AddParameters(scriptParameters);

                // execute the script and await the result.
                //var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);
                var pipelineObjects = ps.Invoke();

                // print the resulting pipeline objects to the console.
                foreach (var item in pipelineObjects)
                {
                    Debug.WriteLine(item.BaseObject.ToString());
                }
            }
        }
    }
}
