using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("WiVendor")]
    public class WiVendor
    {
        [Key]
        public Int32 WireID { get; set; }
        public Int32 CompanyID { get; set; }
        public Int32 EntityID { get; set; }
        public Int32 EntityTypeID { get; set; }
        public Int32 PrimaryAcct { get; set; }
        public String TrxTypeCode { get; set; }
        public String AltTrxTypeCode { get; set; }
        public String RecvgPartyName { get; set; }
        public String RecvgPartyCountryCode { get; set; }
        public String TrxCurrencyCode { get; set; }
        public String RecvgBankName { get; set; }
        public String RecvgBankQualifier { get; set; }
        public String RecvgBankIDSortCode { get; set; }
        public String RecvgBankAcct { get; set; }
        public String RecvgBankCity { get; set; }
        public String RecvgBankCountryCode { get; set; }
        public Int32 UseIntermedBank { get; set; }
        public String IntermedBankName { get; set; }
        public String IntermedBankQualifier { get; set; }
        public String IntermedBankIDSortCode { get; set; }
        public String IntermedBankAcct { get; set; }
        public String IntermedBankCity { get; set; }
        public String IntermedBankCountryCode { get; set; }
        public String SpecialInsturctions { get; set; }
        public Int32 User_ID { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public String bankAccountName { get; set; }
        public String RecvgHolderName { get; set; }
        public String RecvgAccountType { get; set; }
        public String RecvgSWIFTCode { get; set; }
        public String RecvgABANum { get; set; }
        public String RecvgIBAN { get; set; }
        public String IntermedHolderName { get; set; }
        public String IntermedAccountType { get; set; }
        public String IntermedSWIFTCode { get; set; }
        public String IntermedABANum { get; set; }
        public String IntermedIBAN { get; set; }
    }
}