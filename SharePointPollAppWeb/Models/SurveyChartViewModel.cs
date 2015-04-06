using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointPollAppWeb.Models
{
    public class SurveyChartViewModel
    {
        public string SurveyId { get; set; }
        public string Question { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }

    public class AnswerViewModel 
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }                  
    }
}