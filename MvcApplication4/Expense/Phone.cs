using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Phone")]
    public class Phone
    {
        [Key]
        public Int32 PhoneID { get; set; }
        public Int32 TableID { get; set; }
        public String TableType { get; set; }
        public Int32 PhoneTypeID { get; set; }
        public String PhoneNbr { get; set; }
        public Int32 IconType { get; set; }

        public String PhoneExt { get; set; }
        [ForeignKey("PhoneTypeID")]
        public virtual PhoneType PhoneType { get; set; }

    }
    [Table("PhoneType")]
    public class PhoneType
    {
        [Key]
        public int PhoneTypeID { get; set; }
        public String Description { get; set; }
    }
}