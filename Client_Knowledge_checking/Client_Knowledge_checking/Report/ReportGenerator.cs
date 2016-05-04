using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;

namespace Client_Knowledge_checking.Report
{
    public sealed class ReportGenerator
    {
        public string reportPath = "";
        StringWriter stringWriter;
        HtmlTextWriter writer;
        int questionNumerator;
        enum TypeOfQuestion { isImgIsTxt, isTxt, isImg };
        enum TypeOfTableRec { dobrze, źle, otwarte };
        List<TypeOfTableRec> listForTab;
        static ReportGenerator single_oInstance = null;

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
            reportPath = @Utilities.UsableMethods.unzippedTestPath + userName + ".html";
            stringWriter = new StringWriter();
            listForTab = new List<TypeOfTableRec>();
            questionNumerator = 0;
            AddStartTags(titleOfTest, userName);
        }

        public String GenerateReport()
        {
            GenerateTableWithAns();
            writer.RenderEndTag();
            byte[] byteArray = Encoding.UTF8.GetBytes(stringWriter.ToString());
            MemoryStream stream = new MemoryStream(byteArray);
            using (StreamWriter streamWriter = File.CreateText(reportPath))
            {
                streamWriter.Write(stringWriter);
            }
            return reportPath;
        }

        internal void GetAnswerForQuestion(string textQuestionContent = null, string imageQuestionContent = null, string answerForOpen = null, List<string> answersForClosed = null, string rightAnswersForClosed = null, string realAnswers = null, bool isCorrect = false, List<bool> isAnswerIsImage = null)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            AnswerForHTML answForHtml = new AnswerForHTML(isCorrect);
            questionNumerator++;

            if (answerForOpen != null)
                listForTab.Add(TypeOfTableRec.otwarte);
            else
            {
                if (answForHtml.describeForAnswer == "Odpowiedź jest prawidłowa")
                    listForTab.Add(TypeOfTableRec.dobrze);
                else
                    listForTab.Add(TypeOfTableRec.źle);
            }


            if (textQuestionContent != null && imageQuestionContent != null)
            {
                imageQuestionContent = imageQuestionContent.Substring(8);
                InsertProperTags(TypeOfQuestion.isImgIsTxt, textQuestionContent, imageQuestionContent);
            }
            if (textQuestionContent != null && imageQuestionContent == null)
            {
                InsertProperTags(TypeOfQuestion.isTxt, textQuestionContent, null);
            }
            if (textQuestionContent == null && imageQuestionContent != null)
            {
                imageQuestionContent = imageQuestionContent.Substring(8);
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
                            string answer = ans.Substring(8);
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, answer);
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
            writer.RenderEndTag();
        }

        private void AddStartTags(string titleOfTest, string userName)
        {
            writer = new HtmlTextWriter(stringWriter);
            writer.RenderBeginTag(HtmlTextWriterTag.Html);
            writer.RenderBeginTag(HtmlTextWriterTag.Head);
            writer.WriteLine(@"<meta charset =""utf-8"">");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(titleOfTest);
            writer.RenderEndTag();
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
                    writer.WriteLine("Pytanie nr " + questionNumerator.ToString() + ") ");
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
                    writer.WriteLine("Pytanie nr " + questionNumerator.ToString() + ") ");
                    writer.Write(textQuestionContent);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.WriteLine();
                    break;
                case TypeOfQuestion.isImg:
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.WriteLine("Pytanie nr " + questionNumerator.ToString() + ") ");
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, imageQuestionContent);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    break;
            }
        }

        private void GenerateTableWithAns()
        {
            int counterWithGoodAns = 0;
            int counterWithClosed = 0;
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.WriteLine(@"<font color=""red"" size=""12"">");
            writer.WriteLine("Podsumowanie:");
            writer.WriteLine("</font>");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            for(int i = 0; i < listForTab.Count; i++)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(i);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            for(int i = 0; i < listForTab.Count; i++)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(listForTab[i]);
                if (listForTab[i] != TypeOfTableRec.otwarte)
                {
                    counterWithClosed++;
                    if (listForTab[i] == TypeOfTableRec.dobrze)
                        counterWithGoodAns++;
                }
            }
            writer.WriteLine("</table>");
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.AddAttribute(HtmlTextWriterAttribute.Size, "12");
            writer.RenderBeginTag(HtmlTextWriterTag.Font);
            writer.WriteLine("Student odpowiedział dobrze na " + counterWithGoodAns + ", z " + counterWithClosed + "."   );
            writer.WriteLine("</font>");
            writer.WriteLine("</p>");
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
