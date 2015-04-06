using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointPollAppWeb.Models
{
    public class SurveyModel 
    {
        public string SurveyId { get; set; }
        public string Question { get; set; }
        public List<SurveyAnswer> Answers { get; set; }

        /// <summary>
        /// this field is for user's selected answer, which is posted back to increase the vote count
        /// </summary>
        public string SelectedAnswer { get; set; }

        /// <summary>
        /// total votes submitted for the question
        /// </summary>
        public int TotalVotes { get { return Answers.Select(x => x.VoteCount).Sum(); } }

        public SurveyModel()
        {
            Answers = new List<SurveyAnswer>();
        }
    }

    public class SurveyAnswer
    {
        public string Answer { get; set; }
        public int VoteCount { get; set; }
    }

}