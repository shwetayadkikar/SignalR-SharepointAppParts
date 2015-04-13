using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SharePointPollAppWeb.Models;
using System.Threading.Tasks;

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
            Clients.All.showNotifiedVotes(user, data);
        }

        public void notifyVotesToGroup(string user, SurveyModel data, string group)
        {
            Clients.Group(group).showNotifiedVotes(user, data);
        }

        public void addToGroup(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }
    }
}