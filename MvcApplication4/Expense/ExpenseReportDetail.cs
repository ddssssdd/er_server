using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ExpenseReportDetail")]
    public class ExpenseReportDetail
    {
        [Key]
        public Int32 ExpenseReportDetailID { get; set; }
        public Int32 ExpenseReportID { get; set; }
        public Int32 ExpensePurposeID { get; set; }
        public Int32 ExpenseServiceID { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public String Description { get; set; }
        public Decimal Amount { get; set; }
        public Int32 ExpenseStatusID { get; set; }
        public String ReturnReason { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public Int32 CurrencyID { get; set; }
        public Int32 PaidByCo { get; set; }
        public Int32 ExpenseID { get; set; }
        public Int32 ERExpenseTmpID { get; set; }
        public Decimal Mileage { get; set; }
        public Decimal CurrencyRate { get; set; }
        public Decimal TotalAmount { get; set; }
        public Int32 BaseCurrencyID { get; set; }
    }
}