using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseReportServer.Models;
namespace ExpenseReportServer.Controllers
{
   
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        public ViewResult Contact()
        {
            return View("Index");
        }
        public ViewResult About()
        {
            return View("Index");
        }
        
    }

}
