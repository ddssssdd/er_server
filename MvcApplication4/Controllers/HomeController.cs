using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Models;
namespace MvcApplication4.Controllers
{
    public class PersonTest
    {
        public String name { get; set; }
        public int age { get; set; }
        public bool sex { get; set; }
        public DateTime birthday { get; set; }
    }
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.title = "I am index action";
            var person = new PersonTest{ name = "Michael", age = 30, sex = true, birthday = DateTime.Now };
            return View(person);
        }
        public ViewResult Contact()
        {
            return View("Index");
        }
        public ViewResult About()
        {
            return View("Index");
        }
        public ViewResult Guest()
        {
            ViewBag.Greeting = "Hello, ";
            return View();
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            GuestRespone gr = new GuestRespone();   
            return View(gr);
        }
        [HttpPost]
        public ViewResult RsvpForm(GuestRespone guestResponse)
        {
            if (ModelState.IsValid)
            {
                return View("Thanks", guestResponse);
            }
            else {
                return View();
            }
            
        }
    }

}
