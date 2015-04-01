using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointPollAppWeb.SPHelper
{
    public class SharePointHelper
    {
        public static ListItemCollection GetSharePointList(ClientContext clientContext, string title)
        {
            List list = clientContext.Web.Lists.GetByTitle(title);
            CamlQuery query = CamlQuery.CreateAllItemsQuery();
            ListItemCollection items = list.GetItems(query);
            clientContext.Load<ListItemCollection>(items);
            clientContext.ExecuteQuery();
            return items;
        }
    }
}