using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public Int32 PersonID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String TaxID { get; set; }
        public Int32 PersonUserID { get; set; }
    }
}