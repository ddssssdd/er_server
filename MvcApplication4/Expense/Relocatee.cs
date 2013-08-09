using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    [Table("Relocatee")]
    public class Relocatee
    {
        [Key]
        public Int32 RelocateeID { get; set; }
        public Int32 UserID { get; set; }
        public Int32 PolicyID { get; set; }
        public Int32 PersonID { get; set; }
        public byte CancelFlag { get; set; }
        public Int32 ClientID { get; set; }
        public String RelocateeCode { get; set; }
        public Decimal? DestinationPhone { get; set; }
        public String AlternateNumber { get; set; }
        public Int32 OriginAddressID { get; set; }
        public Int32 OriginOfficeAddressID { get; set; }
        public Int32 InterimAddressID { get; set; }
        public Int32 DestinationAddressID { get; set; }
        public Int32 DestinationOfficeAddressID { get; set; }
        public Int32 OriginPropertyTypeID { get; set; }
        public Int32 DestinationPropertyTypeID { get; set; }
        public Int32 RelocateeInfoPhone1 { get; set; }
        public Int32 RelocateeInfoPhone2 { get; set; }
        public Int32 RelocateeInfoPhone3 { get; set; }
        public Int32 RelocateeInfoPhone4 { get; set; }
        public Int32 OriginHomePhone1 { get; set; }
        public Int32 OriginHomePhone2 { get; set; }
        public Int32 OriginHomePhone3 { get; set; }
        public Int32 OriginWorkPhone1 { get; set; }
        public Int32 OriginWorkPhone2 { get; set; }
        public Int32 OriginWorkPhone3 { get; set; }
        public Int32 InterimPhone1 { get; set; }
        public Int32 InterimPhone2 { get; set; }
        public Int32 InterimPhone3 { get; set; }
        public Int32 DestinationHomePhone1 { get; set; }
        public Int32 DestinationHomePhone2 { get; set; }
        public Int32 DestinationHomePhone3 { get; set; }
        public Int32 DestinationWorkPhone1 { get; set; }
        public Int32 DestinationWorkPhone2 { get; set; }
        public Int32 DestinationWorkPhone3 { get; set; }
        public String OriginDepartment { get; set; }
        public String DestinationDepartment { get; set; }
        public Int32 SalutationID { get; set; }
        public String FirstName { get; set; }
        public String MiddleInitial { get; set; }
        public String LastName { get; set; }
        public Int32 StatusID { get; set; }
        public DateTime? StatusDate { get; set; }
        public String SSN { get; set; }
        public String EmployeeNbr { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public Int32 EMailID { get; set; }
        public DateTime? InitiationDate { get; set; }
        public DateTime? ExpectedMoveDate { get; set; }
        public DateTime? VacateDate { get; set; }
        public DateTime? JobStartDate { get; set; }
        public DateTime? ProrateDate { get; set; }
        public DateTime? InterimStartDate { get; set; }
        public DateTime? InterimFinishDate { get; set; }
        public String SpouseFirstName { get; set; }
        public String SpouseMiddleInitial { get; set; }
        public String SpouseLastName { get; set; }
        public String SpouseSSN { get; set; }
        public Int32 SendMailTo { get; set; }
        public Int32 CoordinatorID { get; set; }
        public String CoordAsst { get; set; }
        public Int32 CoordinatorAsstID { get; set; }
        public String RequestDestination { get; set; }
        public String RequestType { get; set; }
        public Int32 MoveTypeID { get; set; }
        public String ProgramType { get; set; }
        public Int32 MoveReasonID { get; set; }
        public Int32 MoveMiles { get; set; }
        public Int32 AccommodationID { get; set; }
        public String SeniorityDate { get; set; }
        public byte PreviousMoveCount { get; set; }
        public String ColMultiple { get; set; }
        public String Title { get; set; }
        public Int32 CurrencyID { get; set; }
        public byte Exemptions1040 { get; set; }
        public byte ExemptionsW4 { get; set; }
        public Decimal? FICAEarnings { get; set; }
        public Int32 GrossupToState { get; set; }
        public DateTime? RelocationComplete { get; set; }
        public Int32 TaxFilingStatusID { get; set; }
        public Int32 ServiceRepID { get; set; }
        public String SpecialNeeds { get; set; }
        public String UrgentInstrctns { get; set; }
        public Int32 ReferralSourceID { get; set; }
        public String ReferralOther { get; set; }
        public String PolicyException { get; set; }
        public Int32 PreferredCommTypeID { get; set; }
        public Int32 PreferredCommOptionID { get; set; }
        public Int32 TypeOfSale { get; set; }
        public Int32 NewHire { get; set; }
        public Int32 LoginAttempts { get; set; }
        public Int32 DefaultVendorMiles { get; set; }
        public Decimal? OrigPurchPrice { get; set; }
        public Int32 TypeOfProgram { get; set; }
        public String ClosingCompanyName { get; set; }
        public String ClosingCompanyAddress1 { get; set; }
        public String ClosingCompanyAddress2 { get; set; }
        public String ClosingCompanyCity { get; set; }
        public String ClosingCompanyState { get; set; }
        public String ClosingCompanyPostalCode { get; set; }
        public String ClosingCompanyCountryCode { get; set; }
        public Int32 QualifyStatus { get; set; }
        public Int32 SentToBankOne { get; set; }
        public Int32 CobrandRemoteUserID { get; set; }
        public DateTime? LastLogin { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 SentInitLetter { get; set; }
        public Int32 GlobalContactID { get; set; }
        public DateTime? ReportingDate { get; set; }
        public DateTime? FinalRecDate { get; set; }
        public String SurveyRespondent { get; set; }
        public String SurveyComments { get; set; }
        public Int32 InterviewOverview { get; set; }
        public Int32 InvoicedInterviewOverview { get; set; }
        public DateTime? FullConversationDate { get; set; }
        public String ClientNotes { get; set; }
        public String ClosingCompanyContact { get; set; }
        public String ClosingCompanyPhone { get; set; }
        public String ClosingCompanyFax { get; set; }
        public String ClosingCompanyEmail { get; set; }
        public String Nickname { get; set; }
        public String SpouseNickname { get; set; }
        public Int32 SentToReloviews { get; set; }
        public Int32 TypeofFile { get; set; }
        public Int32 GACSAssignmentID { get; set; }
        public Int32 GACSHostCurrencyID { get; set; }
        public Int32 GACSHomeCurrencyID { get; set; }
        public Int32 GACSHQCurrencyID { get; set; }
        public Int32 CitizenshipPrimaryID { get; set; }
        public Int32 CitizenshipSecondaryID { get; set; }
        public String PassportNumber { get; set; }
        public DateTime? PassportIssue { get; set; }
        public Int32 BirthCountryID { get; set; }
        public String BirthProvince { get; set; }
        public String BirthCity { get; set; }
        public String GeneralLedger { get; set; }
        public String VendorNumber { get; set; }
        public String FileNumber { get; set; }
        public Int32 ExecutiveOfficer { get; set; }
        public Int32 OriginHomeToOrigOff { get; set; }
        public Int32 OriginHomeToDeatOff { get; set; }
        public Int32 GUPPolicyID { get; set; }
        public Int32 NETPolicyID { get; set; }
        public String TransferNumber { get; set; }

        public Int32 AllowDirectDeposit { get; set; }
        public Int32 BillingRateID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? Canceldate { get; set; }
        public DateTime? Closedate { get; set; }
        public Int32 UsersID { get; set; }
        public Int32 HomeCurrencyID { get; set; }
        public Int32 HostCurrencyID { get; set; }
        public bool OverrideMoveMiles { get; set; }
        /*
        public void init(ExpenseDB db)
        {
            this.client = db.Clients.Find(this.ClientID);
            this.addresses = (from address in db.Addresses
                              where address.Attr_Payment != 1 && address.RelocateeID==this.RelocateeID
                              select address).ToList();
        }
        [ForeignKey("ClientID")]
        public Client client { get; set; }
        [ForeignKey("RelocateeID")]
        public virtual ICollection<Address> addresses { get; set; }
         * */
    }
    
}