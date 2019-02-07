using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Plugin.SQLManagementStudio
{
    internal static class RunAsHelper
    {
        internal static void RunAs(
            string     command,
            string     domain,
            string     user,
            string     password)
        {
            STARTUPINFO startupInfo = default(STARTUPINFO);
            startupInfo.cb = Marshal.SizeOf(startupInfo);

            PROCESS_INFORMATION processInfo = default(PROCESS_INFORMATION);
            try
            {
                bool isProcessCreated = UnsafeNativeMethods.CreateProcessWithLogonW(
                    user,
                    domain,
                    password,
                    (uint)LogonFlags.LOGON_NETCREDENTIALS_ONLY,
                    null,
                    command,
                    0,
                    0,
                    null,
                    ref startupInfo,
                    out processInfo
                );
                if (!isProcessCreated)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                UnsafeNativeMethods.CloseHandle(processInfo.hProcess);
                UnsafeNativeMethods.CloseHandle(processInfo.hThread);
            }
        }
    }
}