using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SharePointPollAppWeb.Models;

namespace SharePointPollAppWeb.SignalR
{
    public class TheHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void notifyVotes(string user, SurveyModel data)
        {
            Clients.All.showNotifiedVotes(user,data);
        }
    }
}