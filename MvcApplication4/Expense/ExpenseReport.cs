using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ExpenseReport")]
    public class ExpenseReport
    {
        [Key]
        public Int32 ExpenseReportID { get; set; }
        public Int32? RelocateeID { get; set; }
        public Int32? ClientID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? PeriodBeginDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public Int32? PeopleCovered { get; set; }
        public Int32? ReportStatusID { get; set; }
        public Int32? PaymentMethodID { get; set; }
        public Int32? PaymentMethodVerifiedID { get; set; }

        public Int32? HowSentID { get; set; }
        public String RejectReason { get; set; }

        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public List<ExpenseReportDetail> details { get; set; }
        public List<ExpenseReportReceipts> receipts { get; set; }
    }
}