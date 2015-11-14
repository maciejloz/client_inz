using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Knowledge_checking
{
    class TestFile
    {
        public string zippedTestPath;
        public string unzippedTestPath;
        public string fileWithTestName;
        public FileStream fileWithTest;

        public TestFile()
        {
            zippedTestPath = @"C:\Users\maciek\Desktop\file_test.zip";
            unzippedTestPath  = @"C:\Users\maciek\Desktop\test\";
            fileWithTestName = "test.txt";
        }

        public void Dispose(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(unzippedTestPath))
                    Directory.Delete(unzippedTestPath, true);
                if (File.Exists(zippedTestPath))
                    File.Delete(zippedTestPath);
                if (this.fileWithTest != null)
                    this.fileWithTest.Dispose();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Prawdopodobnie inny proces aktualnie używa pliku z testem lub pliku .zip");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Problem z prawami dostępu do pliku z testem lub pliku .zip");
            }

        }

    }
}
