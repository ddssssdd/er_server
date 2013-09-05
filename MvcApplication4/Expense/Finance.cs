using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("VW_Finance")]
    public class VW_Finance
    {
        [Column("Annual Wages")]
        public Decimal Annual_Wages { get; set; }
        [Key]
        public int? FinanceID { get; set; }
        [Column("IRS Tax Year")]
        public int? IRS_Tax_Year { get; set; }
        [Column("Update Seq No")]
        public int? Update_Seq_No { get; set; }
        [Column("USA State")]
        public String USA_State { get; set; }
        [Column("Canada State")]
        public String Canada_State { get; set; }
        [Column("Country Code")]
        public String Country_Code { get; set; }
        public Decimal Code800 { get; set; }
        public Decimal Code801 { get; set; }
        public Decimal Excludable { get; set; }
        [Column("FICA Only")]
        public Decimal FICA_Only { get; set; }
        [Column("Include Year")]
        public String Include_Year { get; set; }
        [Column("Local Tax Code")]
        public String Local_Tax_Code { get; set; }
        [Column("Local Wages 1")]
        public Decimal Local_Wages_1 { get; set; }
        public Decimal PaidToE { get; set; }
        [Column("Medicare Wages")]
        public Decimal Medicare_Wages { get; set; }
        public Decimal PaidToO { get; set; }
        public Decimal Retirement { get; set; }
        [Column("Std Deduct")]
        public String Std_Deduct { get; set; }
        public Decimal Taxable { get; set; }
        [Column("Update Date")]
        public DateTime? Update_Date { get; set; }
        public int? RelocateeID { get; set; }
        public int? Exemptions1040 { get; set; }
        public Decimal GR401K { get; set; }
        [Column("Actual Payroll Fica")]
        public Decimal Actual_Payroll_Fica { get; set; }
        [Column("NetCheck Policy")]
        public String NetCheck_Policy { get; set; }
        [Column("Grossup Policy")]
        public String Grossup_Policy { get; set; }
        [Column("Filing Status")]
        public String Filing_Status { get; set; }
        [Column("Filling Status Code")]
        public String Filling_Status_Code { get; set; }
        public Decimal Bonus { get; set; }
        public short Dependents { get; set; }
        [Column("Dep Under 17")]
        public short Dep_Under_17 { get; set; }
        [Column("Effective Date")]
        public DateTime? Effective_Date { get; set; }
        [Column("FICA Wages")]
        public Decimal FICA_Wages { get; set; }
        [Column("From State Code")]
        public String From_State_Code { get; set; }
        [Column("From State Wages")]
        public Decimal From_State_Wages { get; set; }
        [Column("Local 1 Begin")]
        public DateTime? Local_1_Begin { get; set; }
        [Column("Local 1 Pct")]
        public Decimal Local_1_Pct { get; set; }
        [Column("Local 1 End")]
        public DateTime? Local_1_End { get; set; }
        [Column("Local 2 Begin")]
        public DateTime? Local_2_Begin { get; set; }
        [Column("Local 2 Pct")]
        public Decimal Local_2_Pct { get; set; }
        [Column("Local 2 End")]
        public DateTime? Local_2_End { get; set; }
        [Column("Local 3 Begin")]
        public DateTime? Local_3_Begin { get; set; }
        [Column("Local 3 Pct")]
        public Decimal Local_3_Pct { get; set; }
        [Column("Local 3 End")]
        public DateTime? Local_3_End { get; set; }
        [Column("Local 4 Begin")]
        public DateTime? Local_4_Begin { get; set; }
        [Column("Local 4 Pct")]
        public Decimal Local_4_Pct { get; set; }
        [Column("Local 4 End")]
        public DateTime? Local_4_End { get; set; }
        [Column("Local 5 begin")]
        public DateTime? Local_5_begin { get; set; }
        [Column("Local 5 Pct")]
        public Decimal Local_5_Pct { get; set; }
        [Column("Local 5 End")]
        public DateTime? Local_5_End { get; set; }
        [Column("Other Income")]
        public Decimal Other_Income { get; set; }
        [Column("Tax State Code")]
        public String Tax_State_Code { get; set; }
        [Column("Tax Year")]
        public String Tax_Year { get; set; }
        [Column("To State Code")]
        public String To_State_Code { get; set; }
        [Column("To State Wages")]
        public Decimal To_State_Wages { get; set; }
        [Column("YTD Wages")]
        public Decimal YTD_Wages { get; set; }

        public List<VW_Finance> RelocateeFinances(ExpenseDB db, int relocateeID)
        {
            return db.Finances.Where(f => f.RelocateeID == relocateeID).ToList();
        }
    }
}