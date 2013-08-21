using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Providers;

namespace ExpenseReportServer.Security
{
    public class ExpenseMembershipProvider : DefaultMembershipProvider
    {
        public ExpenseMembershipProvider()
        { 
             
        }
        public override bool ValidateUser(string username, string password)
        {
            return username.Equals("admin") && password.Equals("admin");
        }
    }
}