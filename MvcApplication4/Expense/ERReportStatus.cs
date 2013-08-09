using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("ref_ERReportStatus")]
    public class ERReportStatus
    {
        [Key]
        public Int32 ref_ERReportStatusID { get; set; }
        public String Description { get; set; }

    }
}