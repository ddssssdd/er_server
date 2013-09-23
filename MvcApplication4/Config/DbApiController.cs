using ExpenseReportServer.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ExpenseReportServer.Config
{
    public class DbApiController:ApiController
    {
        protected ExpenseDB db
        {
            get 
            {
                return new ExpenseDB(AppSettings.ConnectionString(_clientId));       
            }
        }
        protected int _clientId;
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            if (controllerContext.RouteData.Values.Keys.Contains("clientId"))
            {
                _clientId = int.Parse(controllerContext.RouteData.Values["clientId"].ToString());
            }
            else
            {
                _clientId = 0;
            }
        }
    }
}