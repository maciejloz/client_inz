using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Knowledge_checking.Test
{
    public class TestFile
    {
        public string zippedTestPath;
        public string unzippedTestPath;
        public string fileWithTestName;
        public FileStream fileWithTest;

        public TestFile()
        {
            zippedTestPath = @"C:\test\file_test.zip";
            unzippedTestPath = @"C:\test\";
            fileWithTestName = "test.txt";
        }

        public void Dispose(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\maciek\Documents\Visual Studio 2015\Projects\Client_Knowledge_checking\Disposer\bin\Debug\Disposer.exe");
        }


    }

}
