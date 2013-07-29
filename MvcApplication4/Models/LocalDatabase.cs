using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Models
{
    public class LocalDatabase :DbContext
    {
        public DbSet<UserDevices> UserDevices { get; set; }
    }
    [Table("UserDevices")]
    public class UserDevices
    {
        public int id { get; set; }
        public int userId { get; set; }
        public String token { get; set; }
        public String key { get; set; }
        public int relocateeId { get; set; }
    }
}