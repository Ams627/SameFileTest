using Microsoft.VisualStudio.TestTools.UnitTesting;
using SameFileTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.IO;

namespace SameFileTest.Tests
{
    [TestClass()]
    public class PathExtensionsTests
    {
        [TestMethod()]
        public void IsSameFileTestSymLinkDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var folder = Path.Combine("the", "fat", "cat");
            Directory.CreateDirectory(folder);
            var filename = Path.Combine(folder, "myfile.txt");
            File.WriteAllLines(filename, new[] { "first line" });

            var linkdir = "catdir";
            var lastError = NativeMethods.CreateSymbolicLinkAndGetError(linkdir, folder, NativeMethods.SymbolicLink.Directory);
            var errorMessage = ResultWin32.GetErrorName(lastError);
            PathExtensions.IsSameFile(folder, linkdir).Should().BeTrue();
        }

        public void IsSameFileTestHardLink()
        {

        }

        public void IsSameFileTestSubstDrive()
        {

        }
        public void IsSameFileTestMapNetworkDrive()
        {

        }
    }
}