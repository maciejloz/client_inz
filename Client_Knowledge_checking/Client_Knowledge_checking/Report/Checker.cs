using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_Knowledge_checking.Test;

namespace Client_Knowledge_checking.Report
{
    static class Checker
    {
        static string[] tabWithLetters = new string[5] { "A", "B", "C", "D", "E" };

        public static void processAnswer(Test.Question question)
        {
            string textContentForOpen = question.Content;
            string imageContentForOpen = question.ContentForImageQuestion;

            if (question.IsOpen)
            {
                string answerForOpen = question.AnswersProperty[0].AnswerForOpen;

                if (question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(textContentForOpen, imageContentForOpen, answerForOpen);
                else if(!question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(textContentForOpen, null, answerForOpen);
                else if (question.IsImageQuestionVisible && !question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(null, imageContentForOpen, answerForOpen);
            }
            else
            {
                //internal void GetAnswerForQuestion(string textQuestionContent = null, string imageQuestionContent = null, string answerForOpen = null, List<string> answersForClosed = null, string rightAnswersForClosed = null, string realAnswers = null, bool isCorrect = false)
                string realAnswers = CreateStringWithAnswers(question);
                bool isCorrect = checkIfCorrect(question, realAnswers);
                List<string> answersForClosed = ChangeAnswerPropertyIntoList(question);
               
                if (question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(textContentForOpen, imageContentForOpen, null , answersForClosed, question.RightAnswer, realAnswers, isCorrect );
                else if (!question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(textContentForOpen, null, null, answersForClosed, question.RightAnswer, realAnswers, isCorrect);
                else if (question.IsImageQuestionVisible && !question.IsTextQuestionVisible)
                    Report.Instance.GetAnswerForQuestion(null, imageContentForOpen, null, answersForClosed, question.RightAnswer, realAnswers, isCorrect);
            }
        }

        private static bool checkIfCorrect(Test.Question question, string realAnswers)
        {
            bool isCorrect = false;
            string rightAnswer = question.RightAnswer;
            //string[] rightAnswerTab = rightAnswer.Split(',');
            // var enumerator = question.AnswersProperty.GetEnumerator();
            // question.AnswersProperty[0].IsChecked;
            if (rightAnswer == realAnswers)
                isCorrect = true;        
            return isCorrect;    
        }

        private static string CreateStringWithAnswers(Test.Question question)
        {
            int iter = 0;
            string realAnswer = "";
            foreach (Test.Question.Answer ans in question.AnswersProperty)
            {
                iter = question.AnswersProperty.IndexOf(ans);
                if (ans.IsChecked)
                    realAnswer = realAnswer + tabWithLetters[iter] + ',';
                if (question.AnswersProperty.Last() == ans)
                    realAnswer = realAnswer.Remove(realAnswer.Length - 1);
            }
            return realAnswer;
        }

        private static List<string> ChangeAnswerPropertyIntoList(Question question)
        {
            List<string> answersForClosed = new List<string>();
            foreach (Test.Question.Answer ans in question.AnswersProperty)
            {
                answersForClosed.Add(ans.AnswerContent);
            }
            return answersForClosed;
        }

    }
}
