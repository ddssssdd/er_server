using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ExpenseReportReceipts")]
    public class ExpenseReportReceipts
    {
        [Key]
        public Int32 ExpenseReportReceiptsID { get; set; }
        public Int32 ExpenseReportID { get; set; }
        public Int32 ExpenseReportDetailID { get; set; }
        public String FileName { get; set; }
        public String Notes { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
    }
}