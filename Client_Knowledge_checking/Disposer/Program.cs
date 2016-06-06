using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Disposer
{
    class Program
    {
        public static string unzippedTestPath;

        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            unzippedTestPath = args[0];
            try
            {
                if (Directory.Exists(unzippedTestPath))
                    Directory.Delete(unzippedTestPath, true);
            }
            catch (IOException)
            {
               Console.WriteLine(@"Prawdopodobnie inny proces aktualnie używa pliku z testem lub pliku .zip. Usuń ręcznie katalog: " + unzippedTestPath);
                Console.ReadKey();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(@"Problem z prawami dostępu do pliku z testem lub pliku .zip. Usuń ręcznie katalog: " + unzippedTestPath);
                Console.ReadKey();
            }
        }
    }
}
