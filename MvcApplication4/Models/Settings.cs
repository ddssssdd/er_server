using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public String Key { get; set; }
        public String Value { get; set; }
    }
}