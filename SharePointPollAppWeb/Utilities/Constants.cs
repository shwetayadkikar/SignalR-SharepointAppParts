using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointPollAppWeb.Utilities
{
    public class Constants
    {
        public const string SURVEY_LIST = "SurveyList";
        public const string SURVEY_ANSWERS_LIST = "SurveyAnswers";
        public const string SPHOSTURL_KEY = "SPHostUrl";
        public const string USERNAME_KEY = "pollApp_username";
        public static List<string> USER_GROUPS = new List<string>() { "group1", "group2" };
    }
}