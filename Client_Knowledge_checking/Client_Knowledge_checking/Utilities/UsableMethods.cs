﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace Client_Knowledge_checking.Utilities
{
    public static class UsableMethods
    {
        public static string zippedTestPath;
        public static string unzippedTestPath;
        public static string clientName;

        public static void unzipTest(Test.TestFile testFile)
        {
            zippedTestPath = testFile.zippedTestPath;
            unzippedTestPath = testFile.unzippedTestPath;
            ZipFile.ExtractToDirectory(zippedTestPath, unzippedTestPath);
        }

        public static List<string> ReadFile(string pathToReadFile)
        {
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(pathToReadFile))
                {
                    while (!sr.EndOfStream)
                    {
                        lines.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return lines;
        }

        public static void TakeInput(string name)
        {
            clientName = name;
        }
    }
}
