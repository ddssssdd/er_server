using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Models
{
    [Table("Users")]
    public class LoginUsers
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
}