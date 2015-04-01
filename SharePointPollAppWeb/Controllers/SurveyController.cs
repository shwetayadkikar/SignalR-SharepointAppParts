using Microsoft.SharePoint.Client;
using SharePointPollAppWeb.Models;
using SharePointPollAppWeb.SPHelper;
using SharePointPollAppWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharePointPollAppWeb.Controllers
{
    public class SurveyController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            List<SurveyModel> model = new List<SurveyModel>();
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


                    //set cookies
                    HttpCookieCollection cookieCollection = new HttpCookieCollection();
                    Response.Cookies.Set(new HttpCookie(Constants.USERNAME_KEY, spUser.Title));
                    Uri spHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request);
                    Response.Cookies.Set(new HttpCookie(Constants.SPHOSTURL_KEY, spHostUrl.AbsoluteUri));
                    
                    //Get Survey List
                    var items = SharePointHelper.GetSharePointList(clientContext, Constants.SURVEY_LIST);
                    foreach (var item in items)
                    {
                        SurveyModel survey = new SurveyModel();
                        survey.SurveyId = item["QuestionNumber"].ToString();
                        survey.Question = item["Title"].ToString();
                        model.Add(survey);
                    }

                    //Get Survey answers List
                    var answers = SharePointHelper.GetSharePointList(clientContext, Constants.SURVEY_ANSWERS_LIST);
                    foreach (var survey in model)
                    {
                        var surveyAnswerEntries = answers.Where(x => Convert.ToString(x["QuestionNumber"]).Equals(survey.SurveyId)).ToList();
                        var surveyAnswers = surveyAnswerEntries.Select(x => Convert.ToString(x["Title"])).ToList();
                        survey.Answers = surveyAnswers;
                    }
                }
            }

            return View(model);
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult SubmitVote(SurveyModel surveyAnswer)
        {
            string username = Request.Cookies[Constants.USERNAME_KEY].Value;  
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    ListItemCollection items = SharePointHelper.GetSharePointList(clientContext, Constants.SURVEY_ANSWERS_LIST);
                    ListItem listItem = items != null  ? items.Where(x => x["QuestionNumber"].ToString() == surveyAnswer.SurveyId
                                                                       && x["Title"].Equals(surveyAnswer.SelectedAnswer)).FirstOrDefault() : null;

                    if (listItem != null)
                    {
                        listItem["VoteCount"] = Convert.ToInt32(listItem["VoteCount"]) + 1;
                        listItem.Update();
                        clientContext.ExecuteQuery();
                    }
                    else
                    {
                        //error handling
                    }
                }
            }
            return Json(new { data = surveyAnswer, user = username },
            JsonRequestBehavior.AllowGet);
        }



    }
}
