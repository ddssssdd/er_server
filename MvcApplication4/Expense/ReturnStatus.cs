using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    public class ReturnStatus
    {
        public Boolean status;
        public String message;
        public Object result;
        public ReturnStatus()
        { }
        public ReturnStatus(Object obj)
        {
            result = obj;
            status = result != null;
        }
    }
    
}