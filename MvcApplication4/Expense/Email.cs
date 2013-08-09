using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Email")]
    public class Email
    {
        [Key]
        public Int32 EMailID { get; set; }
        public Int32 TableID { get; set; }
        public String TableType { get; set; }
        public Int32 EMailTypeID { get; set; }
        public String EMail { get; set; }
        [ForeignKey("EMailTypeID")]
        public virtual EmailType EmailType {get;set;}
        /*
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 Preferred { get; set; }
         */
    }
    [Table("EmailType")]
    public class EmailType
    {
        public int EMailTypeID { get; set; }
        public String Description { get; set; }

    }
}