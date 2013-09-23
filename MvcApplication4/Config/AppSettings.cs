using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpenseReportServer.Models;
using System.Reflection;

namespace ExpenseReportServer.Config
{
    public static class AppSettings
    {
        public static void init()
        {
            Smtp_Server_Host = "smtp.google.com";
            Orion_Admin_Database_Connnection_String = "Data Source=10.4.30.40;Database=OrionAdmin_Dev;User ID=sa;Password=s3@dm!n;";
            Decrypt_Database_Name = "ST_Dev";
            using (var db = new LocalDatabase())
            {
                var list = db.Settings.ToList();                
                Type type = typeof(AppSettings);

                type.GetProperties().ToList().ForEach(p => {
                    var row = list.Find(s => s.Key == p.Name);
                    if (row != null)
                    {
                        p.SetValue(null, row.Value);
                    }
                    else
                    {
                        Settings s = new Settings();
                        s.Key = p.Name;
                        s.Value = p.GetValue(null).ToString();
                        db.Settings.Add(s);
                    }
                });
                db.SaveChanges();
                
            }
            
            using (var db = new AdminContext(Orion_Admin_Database_Connnection_String))
            {
                String sql = String.Format("select ID,ClientName,DataBaseServerIP,dataBaseName,DBUsername,{0}.dbo.clrOrionStauthDecryptString(DBPassword) as password from clients", Decrypt_Database_Name);
                clientConfigs = db.Database.SqlQuery<ClientConfig>(sql, new object[]{}).ToList();
            }

        }
        public static String Smtp_Server_Host{get;set;}
        public static String Upload_To_Folder { get; set; }
        public static String Image_Reference_Url_Base { get; set; }
        public static String Orion_Admin_Database_Connnection_String { get; set; }
        public static String Decrypt_Database_Name { get; set; }
        private static List<ClientConfig> clientConfigs;
        public static String ConnectionString(int clientId)
        {
            if (clientId == 0)
            {
                return defaultExpenseDBConnectionString();
            }
            var clientConfig = clientConfigs.Find(cc => cc.ID==clientId);
            if (clientConfig!=null)
            {
                return clientConfig.ConnectionString();
            }
            else
            {
                return defaultExpenseDBConnectionString();
            }
        }
        public static String defaultExpenseDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["ExpenseDB"].ConnectionString;
        }
        public static List<ClientConfig> clients(){
            return clientConfigs;
        }
    }
}