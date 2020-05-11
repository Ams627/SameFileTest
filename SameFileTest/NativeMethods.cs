using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SameFileTest
{


    public static class NativeMethods
    {
        public enum FileFlagsAndAttributes
        {
            //
            // Summary:
            //     The file is read-only.
            ReadOnly = 1,
            //
            // Summary:
            //     The file is hidden, and thus is not included in an ordinary directory listing.
            Hidden = 2,
            //
            // Summary:
            //     The file is a system file. That is, the file is part of the operating system
            //     or is used exclusively by the operating system.
            System = 4,
            //
            // Summary:
            //     The file is a directory.
            Directory = 16,
            //
            // Summary:
            //     The file is a candidate for backup or removal.
            Archive = 32,
            //
            // Summary:
            //     Reserved for future use.
            Device = 64,
            //
            // Summary:
            //     The file is a standard file that has no special attributes. This attribute is
            //     valid only if it is used alone.
            Normal = 128,
            //
            // Summary:
            //     The file is temporary. A temporary file contains data that is needed while an
            //     application is executing but is not needed after the application is finished.
            //     File systems try to keep all the data in memory for quicker access rather than
            //     flushing the data back to mass storage. A temporary file should be deleted by
            //     the application as soon as it is no longer needed.
            Temporary = 256,
            //
            // Summary:
            //     The file is a sparse file. Sparse files are typically large files whose data
            //     consists of mostly zeros.
            SparseFile = 512,
            //
            // Summary:
            //     The file contains a reparse point, which is a block of user-defined data associated
            //     with a file or a directory.
            ReparsePoint = 1024,
            //
            // Summary:
            //     The file is compressed.
            Compressed = 2048,
            //
            // Summary:
            //     The file is offline. The data of the file is not immediately available.
            Offline = 4096,
            //
            // Summary:
            //     The file will not be indexed by the operating system's content indexing service.
            NotContentIndexed = 8192,
            //
            // Summary:
            //     The file or directory is encrypted. For a file, this means that all data in the
            //     file is encrypted. For a directory, this means that encryption is the default
            //     for newly created files and directories.
            Encrypted = 16384,
            //
            // Summary:
            //     The file or directory includes data integrity support. When this value is applied
            //     to a file, all data streams in the file have integrity support. When this value
            //     is applied to a directory, all new files and subdirectories within that directory,
            //     by default, include integrity support.
            IntegrityStream = 32768,
            //
            // Summary:
            //     The file or directory is excluded from the data integrity scan. When this value
            //     is applied to a directory, by default, all new files and subdirectories within
            //     that directory are excluded from data integrity.
            NoScrubData = 131072,

            BackupSemantics = 0x02000000
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct BY_HANDLE_FILE_INFORMATION
        {
            [FieldOffset(0)]
            public uint FileAttributes;

            [FieldOffset(4)]
            public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;

            [FieldOffset(12)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

            [FieldOffset(20)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;

            [FieldOffset(28)]
            public uint VolumeSerialNumber;

            [FieldOffset(32)]
            public uint FileSizeHigh;

            [FieldOffset(36)]
            public uint FileSizeLow;

            [FieldOffset(40)]
            public uint NumberOfLinks;

            [FieldOffset(44)]
            public uint FileIndexHigh;

            [FieldOffset(48)]
            public uint FileIndexLow;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename,
          [MarshalAs(UnmanagedType.U4)] FileAccess access,
          [MarshalAs(UnmanagedType.U4)] FileShare share,
          IntPtr securityAttributes,
          [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
          [MarshalAs(UnmanagedType.U4)] FileFlagsAndAttributes flagsAndAttributes,
          IntPtr templateFile);

        [DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        public static int CreateSymbolicLinkAndGetError(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags)
        {
            var result = CreateSymbolicLink(lpSymlinkFileName, lpTargetFileName, dwFlags);
            if (result == 1)
            {
                return 0;
            }

            return Marshal.GetLastWin32Error();
        }

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

    }
}
