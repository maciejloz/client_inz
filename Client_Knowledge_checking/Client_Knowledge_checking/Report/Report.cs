using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_Knowledge_checking.Test;

namespace Client_Knowledge_checking.Report
{
    public sealed class Report
    {
        private static Report single_oInstance = null;

        public static Report Instance
        {
            get
            {
                if (single_oInstance == null)
                    single_oInstance = new Report();
                return single_oInstance;
            }
        }

        public void initializeReport(Test.Question question)
        {

        }

        internal void GetAnswerForQuestion(string textQuestionContent = null, string imageQuestionContent = null, string answerForOpen =null , List<string> answersForClosed = null, string rightAnswersForClosed = null, string realAnswers = null, bool isCorrect = false)
        {
            
        }
    }
}
