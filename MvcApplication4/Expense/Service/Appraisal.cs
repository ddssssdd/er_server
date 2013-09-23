using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense.Service
{
    [Table("VW_ERCAppraisal")]
    public class VW_ERCAppraisal
    {
        [Key]
        public int? AppraiserID { get; set; }
        public int? VendorID { get; set; }
        public String VendorName { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String CountryCode { get; set; }
        public int? AddressID { get; set; }
        public String VendorContactNotes { get; set; }
        public int? RelocateeID { get; set; }
        public int? ERCAppraisalID { get; set; }
        public int? AppraisalAppraiserID { get; set; }
        public Decimal AppraisalEstimate { get; set; }
        public String ExtAppeal { get; set; }
        public String IntAppeal { get; set; }
        public String Quality { get; set; }
        public String Age { get; set; }
        public String Condition { get; set; }
        public String TotRoom { get; set; }
        public String TotBedroom { get; set; }
        public String TotBath { get; set; }
        public String GrossLivingArea { get; set; }
        public String BasementArea { get; set; }
        public String BasementFinish { get; set; }
        public String CarStorage { get; set; }
        public String Features { get; set; }
        public String FirePlace { get; set; }
        public String ReqInspec { get; set; }
        public String RecInspec { get; set; }
        public String Other { get; set; }
        public int? BeenParsed { get; set; }
        public DateTime? EnteredDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public String xAppraisalEstimate { get; set; }
        public int? UpdateUser { get; set; }
        public int? EnteredUser { get; set; }
        public String VendorNotes { get; set; }
    }
}