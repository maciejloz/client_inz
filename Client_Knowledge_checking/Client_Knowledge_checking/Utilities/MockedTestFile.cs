using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Knowledge_checking.Utilities
{
    public static class MockedTestFile
    {
        public static FileStream fileWithTest = File.Open(@"C:\Users\maciek\Desktop\test.zip", FileMode.Open);
        public static bool isFileMocked = false;
    }
}
