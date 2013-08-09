using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ERExpenseService")]
    public class ERExpenseService
    {
        [Key]
        public Int32 ERExpenseserviceID { get; set; }
        public Int32 PolicyID { get; set; }
        public Int32 ClientID { get; set; }
        public Int32 ClientTypeID { get; set; }
        public String Description { get; set; }
        public Int32 Deletable { get; set; }
        public String IsExcludeClientType { get; set; }
        public String IsExcludeClient { get; set; }
        public String IsExcludePolicy { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 Owner { get; set; }
        public Int32 ParentID { get; set; }
        public Int32 RequireMilage { get; set; }
    }
}