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

        public TestFile(string clientName)
        {
            clientName = clientName.Replace(" ", "");
            zippedTestPath = @"C:\test_" + clientName + @"\file_test.zip";
            unzippedTestPath = @"C:\test_"+ clientName + @"\";
            fileWithTestName = "test.tst";
        }

        public void Dispose(object sender, EventArgs e)
        {
            var disposerProcess = new System.Diagnostics.Process();
            disposerProcess.StartInfo.FileName = @"..\..\..\Disposer\bin\Debug\Disposer.exe";
            disposerProcess.StartInfo.Arguments = unzippedTestPath;
            disposerProcess.Start();
        }
    }

}
