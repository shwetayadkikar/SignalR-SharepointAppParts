using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using SharePointPollAppWeb.Models;
using SharePointPollAppWeb.SPHelper;
using SharePointPollAppWeb.Utilities;

namespace SharePointPollAppWeb.Controllers
{
    public class SurveyChartController : Controller
    {
       [SharePointContextFilter]
        public ActionResult Index()
        {
            List<SurveyChartViewModel> model = new List<SurveyChartViewModel>();
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    //Get User
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Title);
                    clientContext.ExecuteQuery();
                    ViewBag.UserName = spUser.Title;
                    Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
               

                    //Get Survey List
                    var items = SharePointHelper.GetSharePointList(clientContext, Constants.SURVEY_LIST);
                    foreach (var item in items)
                    {
                        SurveyChartViewModel survey = new SurveyChartViewModel();
                        survey.SurveyId = item["QuestionNumber"].ToString();
                        survey.Question = item["Title"].ToString();
                        model.Add(survey);
                    }

                    //Get Survey answers List
                    var answers = SharePointHelper.GetSharePointList(clientContext, Constants.SURVEY_ANSWERS_LIST);
                    foreach (var survey in model)
                    {
                        var surveyAnswerEntries = answers.Where(x => Convert.ToString(x["QuestionNumber"]).Equals(survey.SurveyId)).ToList();
                        var surveyAnswers = surveyAnswerEntries.Select(x => new AnswerViewModel() { name = Convert.ToString(x["Title"]), y = Convert.ToInt32(x["VoteCount"]), selected= false, sliced= false }).ToList();
                        double totalVotes = surveyAnswers.Select(x => x.y).Sum();
                        survey.Answers = surveyAnswers.Select(x => { x.y = (x.y / totalVotes) * 100; return x; }).ToList();
                    }
                  
                }
            }

            return View(model);
        }
    }
}