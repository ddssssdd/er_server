using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseReportServer.Expense
{
    public class ExpenseDB : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Relocatee> Relocatees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCode> ExpenseCodes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
        public DbSet<ExpenseReportDetail> ExpenseReportDetail { get; set; }
        public DbSet<ERExpenseTmp> ERExpenseTmp { get; set; }
        public DbSet<ERReportStatus> ReportStatus { get; set; }
        public DbSet<ERExpensePurpose> ExpensePurpose { get; set; }
        public DbSet<ERExpenseService> ExpenseService { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ExppenseView> ExpenseView { get; set; }
        public DbSet<ExpenseReportReceipts> ExpenseReportReceipts { get; set; }
        public DbSet<CompanyMoveMileage> CompanyMoveMileages {get;set;}
    }


    public class Users
    {
        [Key]
        public Int32 UserID { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String eMail { get; set; }
        public String Password { get; set; }
    }
    [Table("Person")]
    public class Person
    {
        [Key]
        public Int32 PersonID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String TaxID { get; set; }
        public Int32 PersonUserID { get; set; }
    }
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
    }


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
    [Table("Email")]
    public class Email
    {
        [Key]
        public Int32 EMailID { get; set; }
        public Int32 TableID { get; set; }
        public String TableType { get; set; }
        public Int32 EMailTypeID { get; set; }
        public String EMail { get; set; }
        /*
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 Preferred { get; set; }
         */
    }
    [Table("Address")]
    public class Address
    {
        [Key]
        public Int32 AddressID { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public Int32 PANewHire { get; set; }
        public String PostalCode { get; set; }
        public Int32 ZipCode { get; set; }
        public String CountryCode { get; set; }
        public Int32 AddressTypeID { get; set; }
        public Int32 Region { get; set; }

        public Int32 RelocateeID { get; set; }


    }
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

    }
    [Table("Expense")]
    public class Expense
    {
        [Key]
        public Int32 ExpenseID { get; set; }
        public Int32 EntityID { get; set; }
        public Int32 EntityTypeID { get; set; }
        public Int32? InvoiceID { get; set; }
        public Int32 ExpenseCodeID { get; set; }
        public Int32? AdvanceAccountID { get; set; }
        public Int32? PayrollDisbursementsID { get; set; }
        public Int32? AccountingDisbursementsID { get; set; }
        public Int32? APBatchID { get; set; }
        public Int32 CompanyID { get; set; }
        public Decimal? Amount { get; set; }
        public String CheckNumber { get; set; }
        public String APGenLedgerNumber { get; set; }
        public String VendorReference { get; set; }
        public DateTime? GrossUpDate { get; set; }
        public DateTime? PRDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public Int32 SourceCurrencyID { get; set; }
        public String SourceCurrencyCode { get; set; }
        public Decimal? SourceCurrencyAmount { get; set; }
        public Decimal? SourceConvRate { get; set; }
        public bool IsManuallyInputRate { get; set; }
        public Int32? TargetCurrencyID { get; set; }
        public String TargetCurrencyCode { get; set; }
        public Decimal? TargetCurrencyAmount { get; set; }
        public Decimal? TargetConvRate { get; set; }
        public String PaidCurrencyCode { get; set; }
        public Decimal? PaidCurrencyAmount { get; set; }
        public Decimal? PaidConvRate { get; set; }
        public Int32? RecurringID { get; set; }
        public Int32? User_ID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EntryDate { get; set; }
        public Int32? UpdateSeqNo { get; set; }
        public String InvoiceNumber { get; set; }
        public Int32? RecalculateNetCheck { get; set; }
        public Int32? NetCheckAnn { get; set; }
        public String TransactionNumber { get; set; }
        public Int32? AdvanceID { get; set; }
        public String ERDetailID { get; set; }
        public Int32? ExpenseReportID { get; set; }
        public Int32? AmountID { get; set; }
    }
    [Table("VW_Expense")]
    public class ExppenseView
    {
        [Key]
        public Int32 ExpenseID { get; set; }
        public Int32 EntityID { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public String ExpenseCodeDescription { get; set; }
        public String ExpenseGroupFullName { get; set; }
        public Decimal? NetCheck { get; set; }
    }
    [Table("ExpenseCode")]
    public class ExpenseCode
    {
        [Key]
        public Int32 ExpenseCodeID { get; set; }
        public Int32 ClientID { get; set; }
        public String UserCode { get; set; }
        public String Description { get; set; }
        public Int32 ReportingGroupID { get; set; }
        public Int32 TaxabilityTypeID { get; set; }
        public String SystemNumber { get; set; }
        public String FinancialCode1 { get; set; }
        public String FinancialCode2 { get; set; }
        public String FinancialCode3 { get; set; }
        public String FinancialCode4 { get; set; }
        public String FinancialCode5 { get; set; }
        public String FinancialCode6 { get; set; }
        public bool FICAOnly { get; set; }
        public Int32 CopyFromBase { get; set; }
        public bool Exclude { get; set; }
        public bool Hide { get; set; }
        public bool Displayable { get; set; }
        public Int32 Deletable { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public Int32 BaseExpenseCodeID { get; set; }
        public Int32 TaxCategoryID { get; set; }
        public String MoveCategory { get; set; }
        public bool NetCheckAdj { get; set; }
    }
    [Table("Service")]
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public string GroupName { get; set; }
    }
    [Table("ExpenseReport")]
    public class ExpenseReport 
    {
        public Int32 ExpenseReportID { get; set; }
        public Int32? RelocateeID { get; set; }
        public Int32? ClientID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? PeriodBeginDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public Int32? PeopleCovered { get; set; }
        public Int32? ReportStatusID { get; set; }
        public Int32? PaymentMethodID { get; set; }
        public Int32? PaymentMethodVerifiedID { get; set; }
        
        public Int32? HowSentID { get; set; }
        public String RejectReason { get; set; }

        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public List<ExpenseReportDetail> details { get; set; }
        public List<ExpenseReportReceipts> receipts { get; set; }
    }
    [Table("ExpenseReportDetail")]
    public class ExpenseReportDetail 
    {
        public Int32 ExpenseReportDetailID { get; set; }
        public Int32 ExpenseReportID { get; set; }
        public Int32 ExpensePurposeID { get; set; }
        public Int32 ExpenseServiceID { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public String Description { get; set; }
        public Decimal Amount { get; set; }
        public Int32 ExpenseStatusID { get; set; }
        public String ReturnReason { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public Int32 CurrencyID { get; set; }
        public Int32 PaidByCo { get; set; }
        public Int32 ExpenseID { get; set; }
        public Int32 ERExpenseTmpID { get; set; }
        public Decimal Mileage { get; set; }
        public Decimal CurrencyRate { get; set; }
        public Decimal TotalAmount { get; set; }
        public Int32 BaseCurrencyID { get; set; }
    }
    [Table("ExpenseReportReceipts")]
    public class ExpenseReportReceipts 
    {
        public Int32 ExpenseReportReceiptsID { get; set; }
        public Int32 ExpenseReportID { get; set; }
        public Int32 ExpenseReportDetailID { get; set; }
        public String FileName { get; set; }
        public String Notes { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
    }
    [Table("ERExpenseTmp")]
    public class ERExpenseTmp
    {
        public Int32 ERExpenseTmpID { get; set; }
        public Int32 ExpenseCodeID { get; set; }
        public Int32 AccountingDisbursementsID { get; set; }
        public Int32 PayrollDisbursementsID { get; set; }
        public Decimal Amount { get; set; }
        public String VendorReference { get; set; }
        public DateTime? GrossUpDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public String PaidTo { get; set; }
        public String ReferenceInfo { get; set; }
        public Int32 SourceCurrencyID { get; set; }
        public String SourceCurrencyCode { get; set; }
        public Decimal SourceCurrencyAmount { get; set; }
        public Decimal SourceConvRate { get; set; }
        public Int32 User_ID { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime? EntryDate { get; set; }
        public Int32 UpdateSeqNo { get; set; }
        public String InvoiceNumber { get; set; }
    }
    [Table("ref_ERReportStatus")]
    public class ERReportStatus 
    {
        [Key]
        public Int32 ref_ERReportStatusID { get; set; }
        public String Description { get; set; }
        
    }
    [Table("ERExpensePurpose")]
    public class ERExpensePurpose 
    {
        [Key]
        public Int32 ERExpensePurposeID { get; set; }
        public Int32 PolicyID { get; set; }
        public Int32 ClientID { get; set; }
        public Int32 ClientTypeID { get; set; }
        public String Description { get; set; }
        public Int32 Deletable { get; set; }
        public String IsExcludeClientType { get; set; }
        public String IsExcludeClient { get; set; }
        public String IsExcludePolicy { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 Owner { get; set; }
        public Int32 ParentID { get; set; }
    }
    [Table("ERExpenseService")]
    public class ERExpenseService 
    {
        [Key]
        public Int32 ERExpenseserviceID { get; set; }
        public Int32 PolicyID { get; set; }
        public Int32 ClientID { get; set; }
        public Int32 ClientTypeID { get; set; }
        public String Description { get; set; }
        public Int32 Deletable { get; set; }
        public String IsExcludeClientType { get; set; }
        public String IsExcludeClient { get; set; }
        public String IsExcludePolicy { get; set; }
        public Int32 EnteredUser { get; set; }
        public DateTime EnteredDate { get; set; }
        public Int32 UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 Owner { get; set; }
        public Int32 ParentID { get; set; }
        public Int32 RequireMilage { get; set; }
    }
    [Table("client")]
    public class Client 
    {
        public Int32 ClientID
        {
            get;
            set;
        }
        public String Company
        {
            get;
            set;
        }
        public String FirstName
        {
            get;
            set;
        }
        public String LastName
        {
            get;
            set;
        }
        public String Title
        {
            get;
            set;
        }
        public String Address1
        {
            get;
            set;
        }
        public String Address2
        {
            get;
            set;
        }
        public String City
        {
            get;
            set;
        }
        public String State
        {
            get;
            set;
        }
        public String PostalCode
        {
            get;
            set;
        }
        public String Country
        {
            get;
            set;
        }
        public String BillingContactFirst
        {
            get;
            set;
        }
        public String BillingContactLast
        {
            get;
            set;
        }
        public String BillingAddress1
        {
            get;
            set;
        }
        public String BillingAddress2
        {
            get;
            set;
        }
        public String BillingCity
        {
            get;
            set;
        }
        public String BillingState
        {
            get;
            set;
        }
        public String BillingPostalCode
        {
            get;
            set;
        }
        public String BillingCountry
        {
            get;
            set;
        }
        public Int32 Phone1ID
        {
            get;
            set;
        }
        public Int32 Phone2ID
        {
            get;
            set;
        }
        public Int32 Phone3ID
        {
            get;
            set;
        }
        public Int32 Phone4ID
        {
            get;
            set;
        }
        public String PublicNotes
        {
            get;
            set;
        }
        public String PrivateNotes
        {
            get;
            set;
        }
        public Int32 Active
        {
            get;
            set;
        }
        public String WebPage
        {
            get;
            set;
        }
        public Int32 eMailTypeID
        {
            get;
            set;
        }
        public Int32 UseEC
        {
            get;
            set;
        }
        public Int32 parent
        {
            get;
            set;
        }
        public Int32 parentid
        {
            get;
            set;
        }
        public Int32 PreferredCommTypeID
        {
            get;
            set;
        }
        public Int32 PreferredCommOptionID
        {
            get;
            set;
        }
        public Int32 BPOFormTypeID
        {
            get;
            set;
        }
        public Int32 DefaultBillingParty
        {
            get;
            set;
        }
        public Int32 GrossupTableOpt
        {
            get;
            set;
        }
        public Int32 EnteredUser
        {
            get;
            set;
        }
        public DateTime? EnteredDate
        {
            get;
            set;
        }
        public Int32 UpdateUser
        {
            get;
            set;
        }
        public DateTime? UpdateDate
        {
            get;
            set;
        }
        public Int32 AccountManagerID
        {
            get;
            set;
        }
        public Int32 ref_ReportScheduleID
        {
            get;
            set;
        }
        public String DaySchedule
        {
            get;
            set;
        }
        public String DateSchedule
        {
            get;
            set;
        }
        public Int32 WaiveECFee
        {
            get;
            set;
        }
        public String BankAccountName
        {
            get;
            set;
        }
        public String BankAccntNbr
        {
            get;
            set;
        }
        public String BankName
        {
            get;
            set;
        }
        public String BankABANbr
        {
            get;
            set;
        }
        public Int32 BankAddressID
        {
            get;
            set;
        }
        public Int32 SelfFunded
        {
            get;
            set;
        }
        public Int32 SelfFundedWhere
        {
            get;
            set;
        }
        public Decimal ClientFundingThreshold
        {
            get;
            set;
        }
        public Decimal NonFundedCostPct
        {
            get;
            set;
        }
        public Decimal ClientAdditionalFunds
        {
            get;
            set;
        }
        public Int32 ClientFundingBillAtEnd
        {
            get;
            set;
        }
        public Int32 ECSelfFunded
        {
            get;
            set;
        }
        public Int32 ECSelfFundedWhere
        {
            get;
            set;
        }
        public Decimal ECClientFundingThreshold
        {
            get;
            set;
        }
        public Decimal ECNonFundedCostPct
        {
            get;
            set;
        }
        public Decimal ECClientAdditionalFunds
        {
            get;
            set;
        }
        public Int32 ECClientFundingBillAtEnd
        {
            get;
            set;
        }
        public Int32 HBSelfFunded
        {
            get;
            set;
        }
        public Int32 HBSelfFundedWhere
        {
            get;
            set;
        }
        public Decimal HBClientFundingThreshold
        {
            get;
            set;
        }
        public Decimal HBNonFundedCostPct
        {
            get;
            set;
        }
        public Decimal HBClientAdditionalFunds
        {
            get;
            set;
        }
        public Int32 HBClientFundingBillAtEnd
        {
            get;
            set;
        }
        public Int32 CVViewOption
        {
            get;
            set;
        }
        public Int32 CVUseContactCompany
        {
            get;
            set;
        }
        public Int32 CVDefContactLocation
        {
            get;
            set;
        }
        public Int32 ClientTypeID
        {
            get;
            set;
        }
        public Int32 BillingContactSalID
        {
            get;
            set;
        }
        public String OriginDestinationType
        {
            get;
            set;
        }
        public String ClientJobLevel
        {
            get;
            set;
        }
        public String CompanyDisplayLabel
        {
            get;
            set;
        }
        public String DepartmentDisplayLabel
        {
            get;
            set;
        }
        public String JobLevelDisplayLabel
        {
            get;
            set;
        }
        public Int32 InitiationDateisRequired
        {
            get;
            set;
        }
        public Int32 TrackType
        {
            get;
            set;
        }
        public double Minimum
        {
            get;
            set;
        }
        public Int32 EffectiveDateisRequired
        {
            get;
            set;
        }
        public String EFTKey
        {
            get;
            set;
        }
        public String OriginAccount
        {
            get;
            set;
        }
        public String BranchCode
        {
            get;
            set;
        }
        public String HSCashAccount
        {
            get;
            set;
        }
        public String HSDepositAccount
        {
            get;
            set;
        }
        public String EMCashAccount
        {
            get;
            set;
        }
        public String EMDepositAccount
        {
            get;
            set;
        }
        public String ClassID
        {
            get;
            set;
        }
        public Int32 EnableBilling
        {
            get;
            set;
        }
        public String BillingType
        {
            get;
            set;
        }
        public String OriginalDFI
        {
            get;
            set;
        }
        public String AccountReceive
        {
            get;
            set;
        }
        public Int32 RequireRate
        {
            get;
            set;
        }
        public Int32 RequireSSN
        {
            get;
            set;
        }
        public Int32 AutoAddExpense
        {
            get;
            set;
        }
        public Int32 HQCurrencyID
        {
            get;
            set;
        }
        public String CurrencyConvert
        {
            get;
            set;
        }
        public String BankAccntNbrBatch
        {
            get;
            set;
        }
        public string ClosingLOSAmax
        {
            get;
            set;
        }
        public string ClosingLOSAcalc
        {
            get;
            set;
        }
        public string ClosingLOSADsc1
        {
            get;
            set;
        }
        public string ClosingLOSADsc2
        {
            get;
            set;
        }

        public String AIRINCServer { get; set; }
        public String AIRINCUser { get; set; }
        public String AIRINCPassword { get; set; }
        public Int32 EnableCostProjection { get; set; }

        public int AllowIntlCurrency { get; set; }
        public int AddressTypeID { get; set; }
        public int AddressAttributeID { get; set; }
        public string MailingAddressee { get; set; }
        public int MailingAddressID { get; set; }
        public string ReceiptFaxNbr { get; set; }
        public string ReceiptEmail { get; set; }
        public string ReceiptInstr { get; set; }
        public int NotifyAdmin { get; set; }
        public int NotifyDays { get; set; }
        public string NotifyEmail { get; set; }
        public int ShowSignature { get; set; }
        public string SignatureHeader { get; set; }
        public string SignatureLine1 { get; set; }
        public string SignatureLine2 { get; set; }
        public string SignatureLine3 { get; set; }
        public string SignatureLine4 { get; set; }
        public int ShowDateSigLine { get; set; }

        public Int32 ERDisplayCheck { get; set; }
        public Int32 ERDisplayWire { get; set; }
        public Int32 ERDisplayACH { get; set; }
        public Int32 ERDisplayTaxId { get; set; }
        public Int32 ERDisplayEmpNbr { get; set; }
        public Int32 RequireEmpNbr { get; set; }
    }
    [Table("CompanyMoveMileage")]
    public class CompanyMoveMileage 
    {
        public Int32 CompanyMoveMileageID { get; set; }
        public Int32 CompanyID { get; set; }
        public Int32 TaxYear { get; set; }
        public Double MileageRate { get; set; }
        public Double Excludeable { get; set; }
    }
}