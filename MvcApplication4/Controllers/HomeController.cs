using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseReportServer.Models;
using System.Web.Security;
using System.Security.Principal;
using System.Threading;
using System.Data;
using ExpenseReportServer.Expense;
using ExpenseReportServer.Helpers;

namespace ExpenseReportServer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private LocalDatabase localDb = new LocalDatabase();
        private ExpenseDB db = new ExpenseDB();
        public ActionResult Index()
        {
            
            return View();
        }
        public ViewResult Apis()
        {
            DataTable dt = new DbHelper(localDb).tables();
            var list = new List<String>();
            foreach (DataRow row in dt.Rows) {
                list.Add(row["TABLE_NAME"].ToString());
            }
            ViewBag.list = list;
            return View();
        }
        public ViewResult About()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }
        public ViewResult Database()
        {
            ViewBag.list = localDb.Connections.ToList();
            return View();
        }
    }

}
