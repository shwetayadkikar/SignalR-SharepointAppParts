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
        public List<string> Answers { get; set; }
        public string SelectedAnswer { get; set; }

        public SurveyModel()
        {
            Answers = new List<string>();
        }
    }

}