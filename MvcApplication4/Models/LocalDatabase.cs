using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Models
{
    public class LocalDatabase :DbContext
    {
        public DbSet<UserDevices> UserDevices { get; set; }
        public DbSet<LoginUsers> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
    [Table("Connection")]
    public class Connection
    {
        public String ConnectionString { get; set; }
        public String Database { get; set; }
        public String Host { get; set; }
        public int? id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public int? UserId { get; set; }
        public String Username { get; set; }
    }
}