using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ExpenseCode")]
    public class ExpenseCode
    {
        [Key]
        public Int32 ExpenseCodeID { get; set; }
        public Int32 ClientID { get; set; }
        public String UserCode { get; set; }
        public String Description { get; set; }
        public Int32 ReportingGroupID { get; set; }
        public Int32 TaxabilityTypeID { get; set; }
        public String SystemNumber { get; set; }
        public String FinancialCode1 { get; set; }
        public String FinancialCode2 { get; set; }
        public String FinancialCode3 { get; set; }
        public String FinancialCode4 { get; set; }
        public String FinancialCode5 { get; set; }
        public String FinancialCode6 { get; set; }
        public bool FICAOnly { get; set; }
        public Int32 CopyFromBase { get; set; }
        public bool Exclude { get; set; }
        public bool Hide { get; set; }
        public bool Displayable { get; set; }
        public Int32 Deletable { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public Int32 BaseExpenseCodeID { get; set; }
        public Int32 TaxCategoryID { get; set; }
        public String MoveCategory { get; set; }
        public bool NetCheckAdj { get; set; }
    }
}