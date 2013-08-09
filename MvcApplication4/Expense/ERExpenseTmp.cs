using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ERExpenseTmp")]
    public class ERExpenseTmp
    {
        public Int32 ERExpenseTmpID { get; set; }
        public Int32 ExpenseCodeID { get; set; }
        public Int32 AccountingDisbursementsID { get; set; }
        public Int32 PayrollDisbursementsID { get; set; }
        public Decimal Amount { get; set; }
        public String VendorReference { get; set; }
        public DateTime? GrossUpDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public Int32 SourceCurrencyID { get; set; }
        public String SourceCurrencyCode { get; set; }
        public Decimal SourceCurrencyAmount { get; set; }
        public Decimal SourceConvRate { get; set; }
        public Int32 User_ID { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EntryDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public String InvoiceNumber { get; set; }
    }
}