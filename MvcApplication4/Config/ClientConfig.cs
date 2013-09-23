using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Config
{
    public class AdminContext : DbContext
    {
        public AdminContext(String connectionString):base(connectionString)
        {
        }
    }
    public class ClientConfig
    {
        public int ID { get; set; }
        public String ClientName { get; set; }
        public String DataBaseServerIP { get; set; }
        public String dataBaseName { get; set; }
        public String DBUsername { get; set; }
        public String password { get; set; }

        public String ConnectionString()
        {
            return String.Format(@"Data Source={0};Database={1};User ID={2};Password={3};",DataBaseServerIP,dataBaseName,DBUsername,password);
        }
    }
}