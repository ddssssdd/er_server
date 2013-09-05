using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("PayeeBankRouting")]
    public class PayeeBankRouting
    {
        public Int32 PayeeBankRoutingID { get; set; }
        public Int32 EntityID { get; set; }
        public Int32 EntityTypeID { get; set; }
        public String PaymentType { get; set; }
        public Int32 BankID { get; set; }
        public String PayeeBankAccount { get; set; }
        public String ABA_Routing { get; set; }
        public String EntityName { get; set; }
        public String AccountType { get; set; }
        public Int32 User_ID { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime EntryDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public Int32 PrimaryAcct { get; set; }
        public List<PayeeBankRouting> RelocatePayeeBankRoutings(ExpenseDB db, int relocateeId)
        {
            return (from payeeBankRouting in db.PayeeBankRoutings
                    where payeeBankRouting.EntityID == relocateeId
                    select payeeBankRouting).ToList();
        }
    }
}