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
using ExpenseReportServer.Config;

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
        public ActionResult Database(int id)
        {
            Connection cnn = localDb.Connections.Find(id);
            if (cnn == null)
            {
                return HttpNotFound("Not found this Connection");
            }
            
            return View(cnn);
        }
        [HttpPost]
        public ActionResult Save(Connection conn)
        {
            localDb.Connections.Add(conn);
            localDb.SaveChanges();

            return RedirectToAction("Database");
        }
        public ViewResult ConnList()
        {
            ViewBag.cnnlist = localDb.Connections.ToList();
            return View();
        }
        public ActionResult Setup(int id, String name)
        {
            Connection cnn = localDb.Connections.Find(id);
            List<FieldDefine> fields = (new DbHelper(new System.Data.Entity.DbContext(cnn.ConnectionString))).columns(name);
            ViewBag.cnnId = id;
            ViewBag.tablename = name;
            ViewBag.fields = fields;
            return View();
        }
        public ActionResult Settings()
        {
            var settings = localDb.Settings.ToList();
            ViewBag.items = settings;            
            return View();
        }
        [HttpPost]
        public ActionResult Settings(FormCollection list) {
            var settings = localDb.Settings.ToList();
            for(int i=0;i<list.Count;i++)
            {
                String key = list.Keys[i];
                String value = list[i];
                var item = settings.Find(s => s.Key == key);
                if (item != null)
                {
                    item.Value = value;
                }
                else
                {
                    item = new Settings();
                    item.Key = key;
                    item.Value = value;
                    localDb.Settings.Add(item);
                }
                
            }
            localDb.SaveChanges();
            AppSettings.init();
            ViewBag.items = localDb.Settings.ToList();
            
            return View();
        }
    }

}
