using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("CompanyMoveMileage")]
    public class CompanyMoveMileage
    {
        [Key]
        public Int32 CompanyMoveMileageID { get; set; }
        public Int32 CompanyID { get; set; }
        public Int32 TaxYear { get; set; }
        public Double MileageRate { get; set; }
        public Double Excludeable { get; set; }
    }
}