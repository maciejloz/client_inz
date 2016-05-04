using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_Knowledge_checking.Test;
using System.Windows;

namespace Client_Knowledge_checking.Report
{
    static class Checker
    {
        public static string[] tabWithLetters = new string[5] { "A", "B", "C", "D", "E" };
        public static bool isTestClosed = true;
        public static int goodAnswers = 0;
        public static int allQuestions = 0;

        public static void processAnswer(Test.Question question)
        {
            string textContentForOpen = question.Content;
            string imageContentForOpen = question.ContentForImageQuestion;
            if (question.IsOpen)
            {
                isTestClosed = false;
                string answerForOpen = question.AnswersProperty[0].AnswerForOpen;
                if (answerForOpen == null)
                    answerForOpen = "Brak odpowiedzi";

                if (question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(textContentForOpen, imageContentForOpen, answerForOpen);
                else if(!question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(textContentForOpen, null, answerForOpen);
                else if (question.IsImageQuestionVisible && !question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(null, imageContentForOpen, answerForOpen);
            }
            else
            {
                //internal void GetAnswerForQuestion(string textQuestionContent = null, string imageQuestionContent = null, string answerForOpen = null, List<string> answersForClosed = null, string rightAnswersForClosed = null, string realAnswers = null, bool isCorrect = false)
                string realAnswers = CreateStringWithAnswers(question);
                bool isCorrect = checkIfCorrect(question, realAnswers);

                if(isTestClosed)
                {
                    if (isCorrect)
                        goodAnswers++;
                    allQuestions++;
                }

                List<string> answersForClosed = GetInfoFromPropertyIntoList(question);
                List<bool> isAnswerAsImage = InsertInfoIfAnswerIsImage(question);

                if (question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(textContentForOpen, imageContentForOpen, null ,  answersForClosed, question.RightAnswer, realAnswers, isCorrect, isAnswerAsImage);
                else if (!question.IsImageQuestionVisible && question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(textContentForOpen, null, null, answersForClosed, question.RightAnswer, realAnswers, isCorrect, isAnswerAsImage);
                else if (question.IsImageQuestionVisible && !question.IsTextQuestionVisible)
                    ReportGenerator.Instance.GetAnswerForQuestion(null, imageContentForOpen, null, answersForClosed, question.RightAnswer, realAnswers, isCorrect, isAnswerAsImage);
            }
        }

        public static void presentScore()
        {
            if(isTestClosed)
                MessageBox.Show("Twój wynik to: " + goodAnswers + " pkt na " + allQuestions + ".");
            else
                MessageBox.Show("Twój test zostanie sprawdzony przez prowadzącego. Czekaj na wyniki!");
        }

        private static bool checkIfCorrect(Test.Question question, string realAnswers)
        {
            bool isCorrect = false;
            string rightAnswer = question.RightAnswer;

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
                //iter = question.AnswersProperty.IndexOf(ans);
                if (ans.IsChecked)
                    realAnswer = realAnswer + tabWithLetters[iter] + ',';
                if (question.AnswersProperty.Last() == ans && realAnswer != "")
                    realAnswer = realAnswer.Remove(realAnswer.Length - 1);
                iter++;
            }
            return realAnswer;
        }

        private static List<string> GetInfoFromPropertyIntoList(Question question)
        {
            List<string> answersForClosed = new List<string>();

            foreach (Test.Question.Answer ans in question.AnswersProperty)
            {
                answersForClosed.Add(ans.AnswerContent);
            }
            return answersForClosed;
        }

        private static List<bool> InsertInfoIfAnswerIsImage(Question question)
        {
            List<bool> ifAnswerIsImage = new List<bool>();
            foreach (Test.Question.Answer ans in question.AnswersProperty)
            {
                ifAnswerIsImage.Add(ans.isImage);
            }
            return ifAnswerIsImage;
        }



    }
}
