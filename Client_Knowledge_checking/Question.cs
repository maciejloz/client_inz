using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Knowledge_checking
{
    public class Question
    {
        public Question(string cont, string type, string time = "60")
        {
            Content = cont;
            TypeOfQuestion = type;
            TimeToAnswer = Int32.Parse(time);
        }
        public Question(string cont, string type, List<string> answers, string right_answ, string time = "60")
        {
            Content = cont;
            TypeOfQuestion = type;
            //Answ_A = answ_a;
            //Answ_B = answ_b;
            //Answ_C = answ_c;
            //Answ_D = answ_d;
            //Answ_E = answ_e;
            RightAnswer = right_answ;
            TimeToAnswer = Int32.Parse(time);
            getAnswers(answers);
        }

        private void getAnswers(List<string> answers)
        {
            AnswersProperty = new ObservableCollection<Answer>();

            foreach(string ans in answers)
            {
                Answer answer = new Answer(ans);
                AnswersProperty.Add(answer);
            }
        }

        public string Content
        {
            get;
            set;    
        }
        public string TypeOfQuestion
        {
            get;
            set;
        }
        public ObservableCollection<Answer> AnswersProperty
        {
            get;
            set;
        }
        public string Answ_A
        {
            get;
            set;
        }
        public string Answ_B
        {
            get;
            set;
        }
        public string Answ_C
        {
            get;
            set;
        }
        public string Answ_D
        {
            get;
            set;
        }
        public string Answ_E
        {
            get;
            set;
        }
        public string RightAnswer
        {
            get;
            set;
        }
        public int TimeToAnswer
        {
            get;
            set;
        }

        public class Answer
        {
            //public bool isPicture;
            //public string answerContent;

            public Answer(string content)
            {
                answerContent = content;
                checkIfIsPicture();
            }
            public string answerContent
            {
                get;
                set;
            }
            public bool isImageVisible
            {
                get;
                set;
            }
            public bool isTextBlockVisible
            {
                get;
                set;
            }


            private void checkIfIsPicture()
            {
                if (answerContent.Length >=9 && answerContent.Substring(0, 9) == "Pictures\\")
                {
                    isImageVisible = true;
                    isTextBlockVisible = false;
                    answerContent = @UsableMethods.unzippedTestPath + answerContent;
                }
                else
                {
                    isImageVisible = false;
                    isTextBlockVisible = true;
                }
            }
        }
    }
}
