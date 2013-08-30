using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseReportServer.Expense
{
    public class RelocateeService
    {
        public String Name { get; set; }
        public String GroupName { get; set; }
        public String Description { get; set; }
        public String abbr { get; set; }
        public String ServiceStatus { get; set; }
        public List<RelocateeService> items(ExpenseDB db, int relocateeId)
        {
            String sqlString = String.Format(@"select service.Name,service.GroupName,stat.Description as ServiceStatus,service.abbr,service.Description from relocateeService relo 
left join   ServiceStatus stat on relo.ServiceStatusID=stat.ServiceStatusID
left join Service on service.ServiceID = relo.ServiceID
where relo.RelocateeID={0} order by service.GroupName",relocateeId);
            return db.Database.SqlQuery<RelocateeService>(sqlString).ToList() ;
        }
    }
}