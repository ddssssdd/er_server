using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Expense")]
    public class Expense
    {
        [Key]
        public Int32 ExpenseID { get; set; }
        public Int32 EntityID { get; set; }
        public Int32 EntityTypeID { get; set; }
        public Int32? InvoiceID { get; set; }
        public Int32 ExpenseCodeID { get; set; }
        public Int32? AdvanceAccountID { get; set; }
        public Int32? PayrollDisbursementsID { get; set; }
        public Int32? AccountingDisbursementsID { get; set; }
        public Int32? APBatchID { get; set; }
        public Int32 CompanyID { get; set; }
        public Decimal? Amount { get; set; }
        public String CheckNumber { get; set; }
        public String APGenLedgerNumber { get; set; }
        public String VendorReference { get; set; }
        public DateTime? GrossUpDate { get; set; }
        public DateTime? PRDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public Int32 SourceCurrencyID { get; set; }
        public String SourceCurrencyCode { get; set; }
        public Decimal? SourceCurrencyAmount { get; set; }
        public Decimal? SourceConvRate { get; set; }
        public bool IsManuallyInputRate { get; set; }
        public Int32? TargetCurrencyID { get; set; }
        public String TargetCurrencyCode { get; set; }
        public Decimal? TargetCurrencyAmount { get; set; }
        public Decimal? TargetConvRate { get; set; }
        public String PaidCurrencyCode { get; set; }
        public Decimal? PaidCurrencyAmount { get; set; }
        public Decimal? PaidConvRate { get; set; }
        public Int32? RecurringID { get; set; }
        public Int32? User_ID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EntryDate { get; set; }
        public Int32? UpdateSeqNo { get; set; }
        public String InvoiceNumber { get; set; }
        public Int32? RecalculateNetCheck { get; set; }
        public Int32? NetCheckAnn { get; set; }
        public String TransactionNumber { get; set; }
        public Int32? AdvanceID { get; set; }
        public String ERDetailID { get; set; }
        public Int32? ExpenseReportID { get; set; }
        public Int32? AmountID { get; set; }
    }
}