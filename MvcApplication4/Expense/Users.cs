using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    public class Users
    {
        [Key]
        public Int32 UserID { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String eMail { get; set; }
        public String Password { get; set; }
    }
}