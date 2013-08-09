using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Message")]
    public class Message
    {
        public Int32 MessageID { get; set; }
        public String ServiceName { get; set; }
        public String Subject { get; set; }
        [Column("Message")]
        public String MessageBody { get; set; }
        public Int32 EnteredUser { get; set; }
        public Int32 RelocateeID { get; set; }
        public Int32 DeleteFlag { get; set; }
        public Int32 OwnerTypeID { get; set; }
    }
}