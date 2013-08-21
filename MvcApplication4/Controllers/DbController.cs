using ExpenseReportServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseReportServer.Helpers;
using ExpenseReportServer.Expense;
using System.Dynamic;
namespace ExpenseReportServer.Controllers
{
    public class DbController : ApiController
    {
        private LocalDatabase db = new LocalDatabase();
        //private ExpenseDB db = new ExpenseDB();
        
        

        [HttpGet]
        public List<FieldDefine> columns(String tablename)
        {
            if (tablename.ToLower().StartsWith("select "))
            {
                return new DbHelper(db).executeSqlToSchema(tablename);
            }
            else {
                return new DbHelper(db).columns(tablename);
            }

            
        }
        [HttpGet]
        public DataTable data(String tablename)
        {
            
            return new DbHelper(db).data(tablename);
        }
        [HttpGet]
        public Object test_dynamic()
        {
            
            dynamic t = new ExpandoObject();
            
            (t as IDictionary<String, Object>).Add("Username", "Michael");
            (t as IDictionary<String, Object>).Add("Password", "Michael");
            (t as IDictionary<String, Object>).Add("Id", 10);
             
            var list =db.Database.SqlQuery(t.GetType(), "select * from users");

            return list;
        }
        [HttpGet]
        public object test()
        {
            var list = db.Database.SqlQuery<SqlModule>("select top 10 * from sys.all_sql_modules").ToList();
            return list;
        }
        [HttpGet]
        public DataTable sqlSchema(String sql)
        {
            return new DbHelper(db).sqlSchema(sql);
        }
    }
    
}
