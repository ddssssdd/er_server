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

        }
        public static String Smtp_Server_Host{get;set;}
        public static String Upload_To_Folder { get; set; }
        public static String Image_Reference_Url_Base { get; set; }
    }
}