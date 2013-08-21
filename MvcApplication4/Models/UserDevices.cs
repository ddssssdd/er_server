using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Models
{
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