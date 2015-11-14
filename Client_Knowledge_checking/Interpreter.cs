using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Knowledge_checking
{
    static class Interpreter
    {
        
        private static List<string> lines;
        private static List<Question> listWithQuestions = new List<Question>();
        private enum typeOfQuestion { open, close};
        private static typeOfQuestion questionType;
        private static string question;
        private static List<string> answers = new List<string>();
        private static int time;
        private static MainWindow mainWindow;
       
        //regexp_1_Z:
        //.*Tresc:"(.*?)", Odpowiedz_A:"(.*?)", Odpowiedz_B:"(.*?)", Odpowiedz_C:"(.*?)", Odpowiedz_D:"(.*?)", Odpowiedz_E:"(.*?)", Prawidlowa:"(.)", Czas:"(\d{2,3})".
        //regexp_2_O:
        //.*Tresc:"(.*?)", Czas:"(\d{2,3})".
        //regexp_3_Rodzaj:
        //.*"(.)".*

        public static void StartInterpreting(TestFile testFile, MainWindow mw)
        {
            mainWindow = mw;
            UsableMethods.unzipTest(testFile);
            lines = UsableMethods.ReadFile(testFile.unzippedTestPath + testFile.fileWithTestName);
            PrepareTest();
            StartTest();
        }

        private static void PrepareTest()
        {
            const string patternForClassifyQuestion = @"Rodzaj:""(.)"".*";
            Regex rExtractToClassify = new Regex(patternForClassifyQuestion, RegexOptions.IgnoreCase);
            Match mExtract;
            Group g0;
            Group g1;

            foreach (string line in lines)
            {
                mExtract = rExtractToClassify.Match(line);
                g0 = mExtract.Groups[0];
                g1 = mExtract.Groups[1];

                if (g1.ToString() == "O")
                    questionType = typeOfQuestion.open;
                else if (g1.ToString() == "Z")
                    questionType = typeOfQuestion.close;

                if (g1.ToString() == "O" || g1.ToString() == "Z")
                {
                    Question question;
                    question = CreateQuestion(line, questionType);
                    listWithQuestions.Add(question);
                }
            }

        }

        private static Question CreateQuestion(string line, typeOfQuestion type)
        {
            Question question;

            if (type == typeOfQuestion.open)
            {
                const string patternForOpenQuestion = @".*Tresc:""(.*?)"", .*""(\d{2,3})"".";
                Regex rExtractForOpen = new Regex(patternForOpenQuestion, RegexOptions.IgnoreCase);
                Match mExtractForOpen = rExtractForOpen.Match(line);
                Group g0_open = mExtractForOpen.Groups[0];
                Group g1_open = mExtractForOpen.Groups[1];
                Group g2_open = mExtractForOpen.Groups[2];
                question = new Question(g1_open.ToString(), "open", g2_open.ToString());
                
            }
            else//(type == typeOfQuestion.close)
            {
                const string patternForCloseQuestion = @".*Tresc:""(.*?)""; Odpowiedz_A:""(.*?)""; Odpowiedz_B:""(.*?)""; Odpowiedz_C:""(.*?)""; Odpowiedz_D:""(.*?)""; Odpowiedz_E:""(.*?)""; Prawidlowa:""(.)""; Czas:""(\d{2,3})"".";
                Regex rExtractForClose = new Regex(patternForCloseQuestion, RegexOptions.IgnoreCase);
                Match mExtractForClose = rExtractForClose.Match(line);
                Group g0_close = mExtractForClose.Groups[0];
                Group g1_close = mExtractForClose.Groups[1];
                Group g2_close = mExtractForClose.Groups[2];
                Group g3_close = mExtractForClose.Groups[3];
                Group g4_close = mExtractForClose.Groups[4];
                Group g5_close = mExtractForClose.Groups[5];
                Group g6_close = mExtractForClose.Groups[6];
                Group g7_close = mExtractForClose.Groups[7];
                Group g8_close = mExtractForClose.Groups[8];
                answers.Add(g2_close.ToString());
                answers.Add(g3_close.ToString());
                answers.Add(g4_close.ToString());
                answers.Add(g5_close.ToString());
                answers.Add(g6_close.ToString());


                question = new Question(g1_close.ToString(), "close", answers, g7_close.ToString(), g8_close.ToString());

                answers.Clear();

            }
            return question;
        }

        private static void StartTest()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            foreach (var qst in listWithQuestions)
            {
                mainWindow.ChangeContext(qst);   
                dispatcherTimer.Interval = new TimeSpan(0, 0, qst.TimeToAnswer);
                dispatcherTimer.Start();
            }
        }


    }
}
