using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public Int32 AddressID { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public Int32? PANewHire { get; set; }
        public String PostalCode { get; set; }
        public Int32? ZipCode { get; set; }
        public String CountryCode { get; set; }
        public Int32? AddressTypeID { get; set; }
        public Int32? Region { get; set; }

        public Int32 RelocateeID { get; set; }
        public Int32? Attr_Billing { get; set; }
        public Int32? Attr_Shipping { get; set; }
        public Int32? Attr_Mailing { get; set; }
        public Int32? Attr_Primary { get; set; }
        public Int32? Attr_Payment { get; set; }
        public Int32? Attr_TaxDocs { get; set; }
        public Int32? Attr_Services { get; set; }
        public Int32? ClientLocationID { get; set; }


    }

}