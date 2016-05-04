using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Knowledge_checking.Test
{
    public class Question
    {
        public bool isImage = false;
        public string content;

        public string Content
        {
            get;
            set;
        }
        public string ContentForImageQuestion
        {
            get;
            set;
        }
        public ObservableCollection<Answer> AnswersProperty
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
        public bool IsOpen
        {
            get;
            set;
        }
        public bool IsImageQuestionVisible
        {
            get;
            set;
        }
        public bool IsTextQuestionVisible
        {
            get;
            set;
        }

        public Question(string cont, string type, string time = "60")
        {
            Content = cont;
            IsOpen = true;
            checkIfIsImageQuestionWrapper();
            TimeToAnswer = Int32.Parse(time);
            getAnswers();
        }

        public Question(string cont, string type, List<string> answers, string right_answ, string time = "60")
        {
            Content = cont;
            IsOpen = false;
            RightAnswer = right_answ;
            TimeToAnswer = Int32.Parse(time);
            checkIfIsImageQuestionWrapper();
            getAnswers(answers);
        }

        private void checkIfIsImageQuestionWrapper()
        {
            content = Content;
            checkIfIsPicture(ref content, ref isImage);
            if (isImage)
            {
                Content = content;
                string[] partsOfContent = content.Split('+');
                if (partsOfContent.Length == 2)
                {
                    ContentForImageQuestion = partsOfContent[0];
                    Content = partsOfContent[1];
                    IsImageQuestionVisible = true;
                    IsTextQuestionVisible = true;
                }
                else
                {
                    ContentForImageQuestion = partsOfContent[0];
                    Content = null;
                    IsImageQuestionVisible = true;
                    IsTextQuestionVisible = false;
                }
            }
            else
            {
                IsImageQuestionVisible = false;
                IsTextQuestionVisible = true;
            }
        }

        private void getAnswers(List<string> answers = null)
        {
            AnswersProperty = new ObservableCollection<Answer>();
            if (answers == null)
            {
                Answer answer = new Answer();
                AnswersProperty.Add(answer);
            }
            else
            {
                foreach (string ans in answers)
                {
                    if (ans != "")
                    {
                        Answer answer = new Answer(ans);
                        AnswersProperty.Add(answer);
                    }
                }
            }
        }

        
        public class Answer
        {
            public bool isImage = false;
            public string answCont;

            public string AnswerContent
            {
                get;
                set;
            }
            public string AnswerForOpen
            {
                get;
                set;
            }
            public bool IsChecked
            {
                get;
                set;
            }
            public bool IsOpen
            {
                get;
                set;
            }
            public bool IsNotOpen
            {
                get;
                set;
            }
            public bool IsImageVisible
            {
                get;
                set;
            }
            public bool IsTextBlockVisible
            {
                get;
                set;
            }
            public bool IsCheckBoxVisible
            {
                get;
                set;
            }
            public Answer(string content)
            {
                AnswerContent = content;
                checkIfIsPictureAnswerWrapper();
                IsOpen = false;
                IsNotOpen = true;
            }

            public Answer()
            {
                IsOpen = true;
                IsNotOpen = false;
            }

            private void checkIfIsPictureAnswerWrapper()
            {
                answCont = AnswerContent;
                checkIfIsPicture(ref answCont, ref isImage);
                IsCheckBoxVisible = true;
                if (isImage)
                {
                    IsImageVisible = true;
                    IsTextBlockVisible = false;
                    AnswerContent = answCont;
                }
                else
                {
                    IsImageVisible = false;
                    IsTextBlockVisible = true;
                }
            }
        }

        public static void checkIfIsPicture(ref string content, ref bool isImage)
        {
            if (content.Length >= 9 && content.Substring(0, 9) == "Pictures\\")
            {
                isImage = true;
                content = @Utilities.UsableMethods.unzippedTestPath + content;
            }
            else
            {
                isImage = false;
            }
        }

    }
}
