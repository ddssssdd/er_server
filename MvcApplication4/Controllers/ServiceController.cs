using ExpenseReportServer.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseReportServer.Models;
using ExpenseReportServer.Expense.Service;
namespace ExpenseReportServer.Controllers
{
    public class ServiceController : ApiController
    {
        private ExpenseDB db = new ExpenseDB();
        [HttpGet]
        public ReturnStatus appraisal(int id)
        {
            var list = db.Appraisal.Where(ap => ap.RelocateeID == id).ToList();
            return new ReturnStatus(
            new DisplayListFactory<VW_ERCAppraisal>(list).items);
        }
    }
}
