using System;
using System.Runtime.InteropServices;

namespace Plugin.SQLManagementStudio
{
    [Flags]
    public enum LogonFlags : uint
    {
        LOGON                     = 0,
        LOGON_WITH_PROFILE        = 1,
        LOGON_NETCREDENTIALS_ONLY = 2
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct STARTUPINFO
    {
        internal int cb;
        internal string lpReserved;
        internal string lpDesktop;
        internal string lpTitle;
        internal int dwX;
        internal int dwY;
        internal int dwXSize;
        internal int dwYSize;
        internal int dwXCountChars;
        internal int dwYCountChars;
        internal int dwFillAttribute;
        internal int dwFlags;
        internal ushort wShowWindow;
        internal ushort cbReserved2;
        internal byte lpReserved2;
        internal IntPtr hStdInput;
        internal IntPtr hStdOutput;
        internal IntPtr hStdError;
    }

    internal struct PROCESS_INFORMATION
    {
        internal IntPtr hProcess;
        internal IntPtr hThread;
        internal int dwProcessId;
        internal int dwThreadId;
    }


    internal class UnsafeNativeMethods
    {
        internal const uint Infinite = 0xffffffff;
        internal const int Startf_UseStdHandles = 0x00000100;
        internal const int StdOutputHandle = -11;
        internal const int StdErrorHandle = -12;


        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool CreateProcessWithLogonW(
            string                  lpUsername,
            string                  lpDomain,
            string                  lpPassword,
            uint                    dwLogonFlags,
            string                  lpApplicationName,
            string                  lpCommandLine,
            uint                    dwCreationFlags,
            uint                    lpEnvironment,
            string                  lpCurrentDirectory,
            ref STARTUPINFO         lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetExitCodeProcess(IntPtr process, ref uint exitCode);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(IntPtr handle);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);
    }
}