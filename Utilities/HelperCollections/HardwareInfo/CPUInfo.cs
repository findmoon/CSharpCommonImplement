using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.HardwareInfo
{
    public class CPUInfo
    {

        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int __cpuid(ref int s1, ref int s2);

        /// <summary>
        /// 利用 x86 汇编的 cpuid 指令 获取CPU序列号
        /// </summary>
        /// __asm 关键字用于调用内联汇编程序，并且可在 C 或 C++ 语句合法时出现。它不能单独显示，必须后跟一个程序集指令、一组用大括号括起来的指令，或者至少一对空大括号。
        /// 此处的术语“__asm 块”指任何指令或指令组
        /// Serial Number 或 S/N，Serial ID 或 SID
        /// <returns></returns>
        [Obsolete("必须在x86上运行，否则 cpuid执行报错 System.Runtime.InteropServices.SEHException:“外部组件发生异常。”")]
        public static string SerialNumber_Use_asm()
        {
            /*
                 pushad
                 mov eax, 01h
                 xor ecx, ecx
                 xor edx, edx
                 cpuid
                 mov ecx, dword ptr[ebp + 8]
                 mov dword ptr[ecx], edx
                 mov ecx, dword ptr[ebp + 0Ch]
                 mov dword ptr[ecx], eax
                 popad
            */
            byte[] shellcode = { 96, 184, 1, 0, 0, 0, 51, 201, 51, 210, 15, 162, 139, 77, 8, 137, 17, 139, 77, 12, 137, 1, 97, 195 };
            IntPtr address = GCHandle.Alloc(shellcode, GCHandleType.Pinned).AddrOfPinnedObject();
            VirtualProtect(address, (uint)shellcode.Length, 0x40, out uint lpflOldProtect);
            __cpuid cpuid = (__cpuid)Marshal.GetDelegateForFunctionPointer(address, typeof(__cpuid));


            int s1 = 0;
            int s2 = 0;
            for (int i = 0; i < 100000; i++)
            {
                cpuid(ref s1, ref s2);
            }
            // Console.Write("asm: {0}", );

            return $"{s1.ToString("X2")}{s2.ToString("X2")}";
        }


        /// <summary>
        /// 使用 WMI 获取CPU序列号
        /// </summary>
        /// Windows Management Instrumentation
        /// <returns></returns>
        public static string[] SerialNumber_Use_WMI()
        {
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    var cpus = new string[moc.Count];
                    var i = 0;
                    foreach (ManagementObject mo in moc)
                    {
                        //Console.WriteLine(", wmi: {0}", mo.Properties["ProcessorId"].Value.ToString());
                        cpus[i] = mo.Properties["ProcessorId"].Value.ToString();
                        i++;
                    }
                    return cpus;
                }
            }
        }
    }
}
