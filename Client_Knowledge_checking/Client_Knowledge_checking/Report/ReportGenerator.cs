using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_Knowledge_checking.Test;
using System.IO;
using System.Web.UI;

namespace Client_Knowledge_checking.Report
{
    public sealed class ReportGenerator
    {
        private string reportPath;
        private StringWriter stringWriter;
        HtmlTextWriter writer;
        int questionNumerator;
        enum TypeOfQuestion { isImgIsTxt, isTxt, isImg };
        private static ReportGenerator single_oInstance = null;

        public static ReportGenerator Instance
        {
            get
            {
                if (single_oInstance == null)
                    single_oInstance = new ReportGenerator();
                return single_oInstance;
            }
        }

        public void InitializeReport(string titleOfTest, string userName)
        {
            reportPath = @"C:\users\maciek\Desktop\" + userName + ".html";
            stringWriter = new StringWriter();
            questionNumerator = 0;
            AddStartTags(titleOfTest, userName);
            
        }

        private void AddStartTags(string titleOfTest, string userName)
        {
            writer = new HtmlTextWriter(stringWriter);
            writer.RenderBeginTag(HtmlTextWriterTag.Html);
            writer.RenderBeginTag(HtmlTextWriterTag.Head);
            writer.WriteLine(@"<meta charset =""utf-8"">");
            //writer.WriteAttribute(@"charset", "utf-8");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(titleOfTest);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        internal void GetAnswerForQuestion(string textQuestionContent = null, string imageQuestionContent = null, string answerForOpen =null , List<string> answersForClosed = null, string rightAnswersForClosed = null, string realAnswers = null, bool isCorrect = false, List<bool> isAnswerIsImage = null)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            AnswerForHTML answForHtml = new AnswerForHTML(isCorrect);
            questionNumerator++;

            if (textQuestionContent != null && imageQuestionContent != null)
            {
                InsertProperTags(TypeOfQuestion.isImgIsTxt, textQuestionContent, imageQuestionContent);
            }
            if (textQuestionContent != null && imageQuestionContent == null)
            {
                InsertProperTags(TypeOfQuestion.isTxt, textQuestionContent, null);
            }
            if (textQuestionContent == null && imageQuestionContent != null)
            {
                InsertProperTags(TypeOfQuestion.isImg, null, imageQuestionContent);
            }

            switch (answerForOpen)
            {
                case null:
                    int index = 0;
                    foreach (string ans in answersForClosed)
                    {
                        if (isAnswerIsImage[index])
                        {
                            writer.WriteLine("<p>" + Checker.tabWithLetters[index] + " " + "</p>");
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, ans);
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);
                            writer.RenderEndTag();
                        }
                        else
                        {
                            if (ans != "")
                            {
                                writer.RenderBeginTag(HtmlTextWriterTag.P);
                                writer.Write(Checker.tabWithLetters[index] + " ");
                                writer.Write(ans);
                                writer.RenderEndTag();
                            }
                        }
                        index++;
                    }
                    InsertAnswerInfoTags(rightAnswersForClosed, realAnswers, answForHtml);
                    break;

                default:
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.Write(answerForOpen);
                    writer.RenderEndTag();
                    break;

            }
            //writer.RenderBeginTag(HtmlTextWriterTag.);
            writer.RenderEndTag();
        }

        private void InsertAnswerInfoTags(string rightAnswersForClosed, string realAnswers, AnswerForHTML answForHtml)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.WriteLine("Prawidłowa odpowiedź:");
            writer.Write(rightAnswersForClosed);
            writer.Write("  Odpowiedź studenta:");
            writer.WriteLine(realAnswers);
            writer.WriteLine(@"<font color =""" + answForHtml.fontColorForDescribe + @""">" + " " + answForHtml.describeForAnswer + " </font >");
            writer.RenderEndTag();
        }

        private void InsertProperTags(TypeOfQuestion typeOfQuestion, string textQuestionContent = null, string imageQuestionContent = null)
        {
            switch(typeOfQuestion)
            {
                case TypeOfQuestion.isImgIsTxt:
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.WriteLine("Pytanie nr " + questionNumerator.ToString());
                    writer.WriteLine(textQuestionContent);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, imageQuestionContent);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    break;
                case TypeOfQuestion.isTxt:
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.WriteLine("Pytanie nr " + questionNumerator.ToString());
                    writer.Write(textQuestionContent);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.WriteLine();
                    break;
            }
        }

        public String GenerateReport() 
        {
            writer.RenderEndTag();
            byte[] byteArray = Encoding.UTF8.GetBytes(stringWriter.ToString());
            MemoryStream stream = new MemoryStream(byteArray);
            using (StreamWriter streamWriter = File.CreateText(reportPath))
            {
                streamWriter.Write(stringWriter);
            }
            return reportPath;
        }

        internal class AnswerForHTML
        {
            internal string describeForAnswer;
            internal string fontColorForDescribe;
            internal AnswerForHTML(bool isCorrect)
            {
                if(isCorrect)
                {
                    describeForAnswer = "Odpowiedź jest prawidłowa";
                    fontColorForDescribe = "Green";
                }
                else
                {
                    describeForAnswer = "Odpowiedź jest nieprawidłowa !";
                    fontColorForDescribe = "Red";
                }
            }
        }

    }
}
