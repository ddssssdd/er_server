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
using System.Data.Entity;
namespace ExpenseReportServer.Controllers
{
    public class DbController : ApiController
    {
        private LocalDatabase db = new LocalDatabase();
        //private ExpenseDB db = new ExpenseDB();
        
        

        [HttpGet]
        public List<FieldDefine> columns(String tablename,int id=0)
        {
            DbContext tempDb;
            if (id == 0)
                tempDb = db;
            else
            {
                Connection cnn= db.Connections.Find(id);
                tempDb = new DbContext(cnn.ConnectionString);
            }
            if (tablename.ToLower().StartsWith("select "))
            {
                return new DbHelper(tempDb).executeSqlToSchema(tablename);
            }
            else {
                return new DbHelper(tempDb).columns(tablename);
            }

            
        }
        [HttpGet]
        public DataTable data(String tablename,int id=0)
        {
            DbContext tempDb;
            if (id == 0)
                tempDb = db;
            else
            {
                Connection cnn = db.Connections.Find(id);
                tempDb = new DbContext(cnn.ConnectionString);
            }
            
            return new DbHelper(tempDb).data(tablename);
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
        [HttpGet]
        public ReturnStatus removeConn(int id)
        {
            Connection cnn = db.Connections.Find(id);
            if (cnn != null)
            {
                db.Connections.Remove(cnn);
                db.SaveChanges();
                return new ReturnStatus { status = true };
            }
            else
            {
                return new ReturnStatus { status = false, message = "Could not find this connection id" };
            }
        }
        [HttpGet]
        public Object search(int id, String keyword)
        {
            Connection cnn = db.Connections.Find(id);
            if (cnn != null) {
                using (DbContext context = new DbContext(cnn.ConnectionString)) {
                    DbHelper helper = new DbHelper(context);
                    List<SqlObject> list = helper.search(keyword);
                    return  list.GroupBy(sqlobject=>{
                        return sqlobject.type_desc;
                    } );
                }
            }

            return "Nothing";
        }
        [HttpGet]
        public object defination(int id,int object_id)
        {
            Connection cnn = db.Connections.Find(id);
            if (cnn != null)
            {
                using (DbContext context = new DbContext(cnn.ConnectionString))
                {
                    DbHelper helper = new DbHelper(context);
                    List<SqlModule> list = helper.defination(object_id);
                    
                    return list;
                }
            }

            return "Nothing";
        }
        [HttpGet]
        public object summary(int id)
        {
            Connection cnn = db.Connections.Find(id);
            if (cnn != null)
            {
                using (DbContext context = new DbContext(cnn.ConnectionString))
                {
                    DbHelper helper = new DbHelper(context);
                    List<SqlSummary> list = helper.summary();
                    return list;
                }
            }

            return "Nothing";
        }
        [HttpGet]
        public object saveSection(String tableName, String sectionName,int groupIndex=0)
        {
            var result = db.Sections.Where(section => section.ClassName.Equals(tableName) && section.Title.Equals(sectionName)).FirstOrDefault();
            if (result == null)
            {
                result = new Section();
                result.ClassName = tableName;
                result.Title = sectionName;
                result.GroupIndex = groupIndex;
                db.Sections.Add(result);
                db.SaveChanges();
                
            }
            return result;
        }
        [HttpGet]
        public object getSections(String tableName)
        {
            var result = db.Sections.Where(section => section.ClassName.Equals(tableName)).ToList();
            return result;
        }
        [HttpGet]
        public object saveCell(int sectionId, String fieldName, String title,int cellIndex=0) {
            var result = db.CellDatas.Where(cell => cell.PropertyName.Equals(fieldName) && cell.section_id == sectionId).FirstOrDefault();
            if (result == null) {
                result = new CellData();
                result.section_id = sectionId;
                result.PropertyName = fieldName;
                db.CellDatas.Add(result);
            }
            result.Title = title;
            result.CellIndex = cellIndex;
            db.SaveChanges();
            return result;
        }
        [HttpGet]
        public object clearSections(String tableName)
        { 
            int count = db.Database.ExecuteSqlCommand("delete from CellData where section_id in (select id from section where ClassName='"+tableName+"')");
            count += db.Database.ExecuteSqlCommand("delete from section where ClassName='"+tableName+"'");
            return new ReturnStatus { status = true, result = count };

        }
    }
    
}
