using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("client")]
    public class Client
    {
        [Key]
        public Int32 ClientID { get; set; }
        public Int32 ClientTypeID { get; set; }
        public String Company { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Title { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }
        public String BillingContactFirst { get; set; }
        public String BillingContactLast { get; set; }
        public String BillingAddress1 { get; set; }
        public String BillingAddress2 { get; set; }
        public String BillingCity { get; set; }
        public String BillingState { get; set; }
        public String BillingPostalCode { get; set; }
        public String BillingCountry { get; set; }
        public Int32 Phone1ID { get; set; }
        public Int32 Phone2ID { get; set; }
        public Int32 Phone3ID { get; set; }
        public Int32 Phone4ID { get; set; }
        public String PublicNotes { get; set; }
        public String PrivateNotes { get; set; }
        public Int32 Active { get; set; }
        public String WebPage { get; set; }
        public Int32 eMailTypeID { get; set; }
        public Int32 UseEC { get; set; }
        public Int32? parent { get; set; }
        public Int32? parentid { get; set; }
        public Int32? PreferredCommTypeID { get; set; }
        public Int32? PreferredCommOptionID { get; set; }
        public Int32? BPOFormTypeID { get; set; }
        public Int32? DefaultBillingParty { get; set; }
        public Int32? GrossupTableOpt { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? AccountManagerID { get; set; }
        public Int32? ref_ReportScheduleID { get; set; }
        public Int32? AutoAddExpense { get; set; }
        public Int32? HQCurrencyID { get; set; }
        public String CurrencyConvert { get; set; }
        public Int32? AddressTypeID { get; set; }
        public Int32? AddressAttributeID { get; set; }
        public string ReceiptFaxNbr { get; set; }
        public string ReceiptEmail { get; set; }
        public string ReceiptInstr { get; set; }
        public Int32? ERDisplayCheck { get; set; }
        public Int32? ERDisplayWire { get; set; }
        public Int32? ERDisplayACH { get; set; }
        public Int32? ERDisplayTaxId { get; set; }
        public Int32? ERDisplayEmpNbr { get; set; }
        public Int32? RequireEmpNbr { get; set; }
    }
}