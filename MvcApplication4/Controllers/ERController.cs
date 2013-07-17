using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Expense;

namespace MvcApplication4.Controllers
{
    public class ERController : Controller
    {
        private ExpenseDB db = new ExpenseDB();

        //
        // GET: /ER/

        public ActionResult Index()
        {
            return View(db.ExpenseReports.ToList());
        }

        //
        // GET: /ER/Details/5

        public ActionResult Details(int id = 0)
        {
            ExpenseReport expensereport = db.ExpenseReports.Find(id);
            if (expensereport == null)
            {
                return HttpNotFound();
            }
            return View(expensereport);
        }

        //
        // GET: /ER/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ER/Create

        [HttpPost]
        public ActionResult Create(ExpenseReport expensereport)
        {
            if (ModelState.IsValid)
            {
                db.ExpenseReports.Add(expensereport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expensereport);
        }

        //
        // GET: /ER/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ExpenseReport expensereport = db.ExpenseReports.Find(id);
            if (expensereport == null)
            {
                return HttpNotFound();
            }
            return View(expensereport);
        }

        //
        // POST: /ER/Edit/5

        [HttpPost]
        public ActionResult Edit(ExpenseReport expensereport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expensereport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expensereport);
        }

        //
        // GET: /ER/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ExpenseReport expensereport = db.ExpenseReports.Find(id);
            if (expensereport == null)
            {
                return HttpNotFound();
            }
            return View(expensereport);
        }

        //
        // POST: /ER/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpenseReport expensereport = db.ExpenseReports.Find(id);
            db.ExpenseReports.Remove(expensereport);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}