using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CMCode.ProcessExt
{
    public static class ProcessUtilities
    {
        #region 获取父进程
        #region 使用性能计数器，暂时代码不是很理解，后续再看。而且似乎本地化的版本中无法使用（即非英语系统）、性能很差、需要用户在性能用户组Performance Users group https://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processes = Process.GetProcessesByName(processName);
            string processIndexdName = null;
            for (var index = 0; index < processes.Length; index++)
            {
                processIndexdName = index == 0 ? processName : (processName + "#" + index); // 原  processIndexdName = index == 0 ? processName : processName + "#" + index; // 三元运算符根本没起作用
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }
            return null;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }
        /// <summary>
        /// 不推荐使用，似乎本地化的版本中无法使用（即非英语系统）、性能很差、需要用户在性能用户组Performance Users group
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [Obsolete("不推荐使用，似乎本地化的版本中无法使用（即非英语系统）、性能很差、需要用户在性能用户组Performance Users group")]
        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
        #endregion

        #region WMI获取父进程
        /// <summary>
        /// 获取父进程。如果出错可能返回null
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static Process GetParent(this Process process)
        {
            try
            {
                //using (var query = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId=" + process.Id))
                using (var query = new ManagementObjectSearcher("root\\CIMV2", "SELECT ParentProcessId FROM Win32_Process WHERE ProcessId=" + process.Id))
                {
                    return query
                      .Get()
                      .OfType<ManagementObject>()
                      .Select(p => Process.GetProcessById((int)(uint)p["ParentProcessId"]))
                      .FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取一个进程的父进程Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static int ParentProcessId(int processId)
        {
            try
            {
                using (var query = new ManagementObjectSearcher("root\\CIMV2", "SELECT ParentProcessId FROM Win32_Process WHERE ProcessId=" + processId))
                {
                    return query
                      .Get()
                      .OfType<ManagementObject>()
                      .Select(p => (int)(uint)p["ParentProcessId"])
                      .FirstOrDefault();
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取父进程第2种方法
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static Process GetParent2(this Process process)
        {
            try
            {
                var queryStr = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", process.Id);
                using (var search = new ManagementObjectSearcher("root\\CIMV2", queryStr))
                {
                    var results = search.Get().GetEnumerator();
                    results.MoveNext(); // 移动到下一个，并指示是否已经移动到了枚举的下一个对象。不执行下面的获取将会报错。【应该是从未指向对象，移动到第一个】
                    var queryObj = results.Current;
                    var parentId = (uint)queryObj["ParentProcessId"];
                    return Process.GetProcessById((int)parentId);
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 使用性能计数器。但应该有着和上面一样的限制，并且会很慢
        /// <summary>
        /// 获取所有的子进程父进程Id
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetAllChildToParentPids()
        {
            var childPidToParentPid = new Dictionary<int, int>();

            var processCounters = new SortedDictionary<string, PerformanceCounter[]>();
            var category = new PerformanceCounterCategory("Process");

            // As the base system always has more than one process running, 
            // don't special case a single instance return.
            var instanceNames = category.GetInstanceNames();
            foreach (string t in instanceNames)
            {
                try
                {
                    processCounters[t] = category.GetCounters(t);
                }
                catch (InvalidOperationException)
                {
                    // Transient processes may no longer exist between 
                    // GetInstanceNames and when the counters are queried.
                }
            }

            foreach (var kvp in processCounters)
            {
                int childPid = -1;
                int parentPid = -1;

                foreach (var counter in kvp.Value)
                {
                    if ("ID Process".CompareTo(counter.CounterName) == 0)
                    {
                        childPid = (int)(counter.NextValue());
                    }
                    else if ("Creating Process ID".CompareTo(counter.CounterName) == 0)
                    {
                        parentPid = (int)(counter.NextValue());
                    }
                }

                if (childPid != -1 && parentPid != -1)
                {
                    childPidToParentPid[childPid] = parentPid;
                }
            }

            return childPidToParentPid;
        }
        #endregion

        #region 使用另外的P/Invoke
        /// <summary>
        /// PInvoke获取父进程id 未测试
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static int ParentProcessId_PInvoke(this Process process)
        {
            return ParentProcessId_PInvoke(process.Id);
        }
        /// <summary>
        /// PInvoke获取父进程id 未测试
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int ParentProcessId_PInvoke(int Id)
        {
            PROCESSENTRY32 pe32 = new PROCESSENTRY32 { };
            pe32.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
            using (var hSnapshot = CreateToolhelp32Snapshot(SnapshotFlags.Process, (uint)Id))
            {
                if (hSnapshot.IsInvalid)
                    throw new Win32Exception();

                if (!Process32First(hSnapshot, ref pe32))
                {
                    int errno = Marshal.GetLastWin32Error();
                    if (errno == ERROR_NO_MORE_FILES)
                        return -1;
                    throw new Win32Exception(errno);
                }
                do
                {
                    if (pe32.th32ProcessID == (uint)Id)
                        return (int)pe32.th32ParentProcessID;
                } while (Process32Next(hSnapshot, ref pe32));
            }
            return -1;
        }
        private const int ERROR_NO_MORE_FILES = 0x12;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeSnapshotHandle CreateToolhelp32Snapshot(SnapshotFlags flags, uint id);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32First(SafeSnapshotHandle hSnapshot, ref PROCESSENTRY32 lppe);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32Next(SafeSnapshotHandle hSnapshot, ref PROCESSENTRY32 lppe);

        [Flags]
        private enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            All = (HeapList | Process | Thread | Module),
            Inherit = 0x80000000,
            NoHeaps = 0x40000000
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
        };
        [SuppressUnmanagedCodeSecurity, HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
        internal sealed class SafeSnapshotHandle : SafeHandleMinusOneIsInvalid
        {
            internal SafeSnapshotHandle() : base(true)
            {
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            internal SafeSnapshotHandle(IntPtr handle) : base(true)
            {
                base.SetHandle(handle);
            }

            protected override bool ReleaseHandle()
            {
                return CloseHandle(base.handle);
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            private static extern bool CloseHandle(IntPtr handle);
        }
        #endregion
        #endregion

        #region 获取子进程、杀死进程树
        /// <summary>
        /// 获取进程树
        /// </summary>
        /// <param name="pid">需要结束的进程ID</param>
        public static void GetProcessTree(int pid)
        {
            var topProcessId = ParentProcessId(pid);
            while (topProcessId>0)
            {
                topProcessId = ParentProcessId(pid);
            }
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select ProcessID From Win32_Process Where ParentProcessID=" + pid))
            {
                ManagementObjectCollection moc = searcher.Get();
                foreach (ManagementObject mo in moc)
                {
                    KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
                }
                try
                {
                    Process proc = Process.GetProcessById(pid);
                    Console.WriteLine(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    /* process already exited */
                }
            }
        }

        /// <summary>
        /// 获取所有的子进程\后代进程
        /// </summary>
        /// <param name="pid">需要结束的进程ID</param>
        public static ArrayList GetProcessAllChildrenIds (int pid)
        {
            ArrayList alst = new ArrayList();
            alst.Add(pid);
            var childPIds = GetChildrenProcessIds(pid);
            while (childPIds.Length>0)
            {
                alst.Add(childPIds);
                foreach (var childPId in childPIds)
                {
                    var cchildPIds = GetProcessAllChildrenIds(childPId);
                }
            }
        }
        /// <summary>
        /// 仅获取子进程ids
        /// </summary>
        /// <param name="pid">需要结束的进程ID</param>
        public static int[] GetChildrenProcessIds(int pid)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select ProcessID From Win32_Process Where ParentProcessID=" + pid))
            {
                ManagementObjectCollection moc = searcher.Get();
                var childPids = new int[moc.Count];
                var i = 0;
                foreach (ManagementObject mo in moc)
                {
                    childPids[i]= Convert.ToInt32(mo["ProcessID"]);
                    i++;

                    #region 使用Properties
                    //object data = mo.Properties["processid"].Value;
                    //if (data != null)
                    //{
                    //    var childId = Convert.ToInt32(data);
                    //    var childProcess = Process.GetProcessById(childId);
                    //    if (childProcess != null) { }
                    //} 
                    #endregion
                }
                return childPids;
            }
        }
        /// <summary>
        /// 杀死进程树
        /// </summary>
        /// <param name="pid">需要结束的进程ID</param>
        public static void KillProcessTree(int pid)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select ProcessID From Win32_Process Where ParentProcessID=" + pid))
            {
                ManagementObjectCollection moc = searcher.Get();
                foreach (ManagementObject mo in moc)
                {
                    KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
                }
                try
                {
                    Process proc = Process.GetProcessById(pid);
                    Console.WriteLine(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    /* process already exited */
                }
            }
        }

        /// <summary>
        /// 结束进程和相关的子进程
        /// </summary>
        /// <param name="pid">需要结束的进程ID</param>
        public static void KillProcessAndChildrens(int pid)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select ProcessID From Win32_Process Where ParentProcessID=" + pid))
            {
                ManagementObjectCollection moc = searcher.Get();
                foreach (ManagementObject mo in moc)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"]));
                }
                try
                {
                    Process proc = Process.GetProcessById(pid);
                    Console.WriteLine(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    /* process already exited */
                }
            }
        }
        #endregion
    }
    #region NtQueryInformationProcess/64 
    /// <summary>
    /// A utility class to determine a process parent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ParentProcessUtilities
    {
        // These members must match PROCESS_BASIC_INFORMATION
        internal IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        internal IntPtr Reserved2_0;
        internal IntPtr Reserved2_1;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        /// <summary>
        /// Gets the parent process of specified process.
        /// </summary>
        /// <param name="id">The process id.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(int id)
        {
            Process process = Process.GetProcessById(id);
            return GetParentProcess(process.Handle);
        }

        /// <summary>
        /// Gets the parent process of a specified process.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(IntPtr handle)
        {
            ParentProcessUtilities pbi = new ParentProcessUtilities();
            int returnLength;
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                // not found
                return null;
            }
        }
    }
    #endregion
}
