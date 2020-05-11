using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SameFileTest
{
    public static class PathExtensions
    {
        public static bool IsSameFile(string path1, string path2)
        {
            using (SafeFileHandle sfh1 = NativeMethods.CreateFile(path1, FileAccess.Read, FileShare.ReadWrite,
                IntPtr.Zero, FileMode.Open, NativeMethods.FileFlagsAndAttributes.BackupSemantics, IntPtr.Zero))
            {
                if (sfh1.IsInvalid)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                using (SafeFileHandle sfh2 = NativeMethods.CreateFile(path2, FileAccess.Read, FileShare.ReadWrite,
                  IntPtr.Zero, FileMode.Open, NativeMethods.FileFlagsAndAttributes.BackupSemantics, IntPtr.Zero))
                {
                    if (sfh2.IsInvalid)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                    NativeMethods.BY_HANDLE_FILE_INFORMATION fileInfo1;
                    bool result1 = NativeMethods.GetFileInformationByHandle(sfh1, out fileInfo1);
                    if (!result1)
                        throw new IOException(string.Format("GetFileInformationByHandle has failed on {0}", path1));

                    NativeMethods.BY_HANDLE_FILE_INFORMATION fileInfo2;
                    bool result2 = NativeMethods.GetFileInformationByHandle(sfh2, out fileInfo2);
                    if (!result2)
                        throw new IOException(string.Format("GetFileInformationByHandle has failed on {0}", path2));

                    return fileInfo1.VolumeSerialNumber == fileInfo2.VolumeSerialNumber
                      && fileInfo1.FileIndexHigh == fileInfo2.FileIndexHigh
                      && fileInfo1.FileIndexLow == fileInfo2.FileIndexLow;
                }
            }
        }
    }
}
