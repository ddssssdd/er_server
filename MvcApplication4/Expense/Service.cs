using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public string GroupName { get; set; }
    }
}