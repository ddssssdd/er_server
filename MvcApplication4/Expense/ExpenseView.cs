using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("VW_Expense")]
    public class ExppenseView
    {
        [Key]
        public Int32 ExpenseID { get; set; }
        public Int32 EntityID { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public String ExpenseCodeDescription { get; set; }
        public String ExpenseGroupFullName { get; set; }
        public Decimal? NetCheck { get; set; }
    }
}