using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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
        public DbSet<PayeeBankRouting> PayeeBankRoutings { get; set; }
        public DbSet<WiVendor> WiVendors { get; set; }

        public DbSet<EmailType> EmailTypes { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }
        public DbSet<VW_Finance> Finances { get; set; }
        
    }



}