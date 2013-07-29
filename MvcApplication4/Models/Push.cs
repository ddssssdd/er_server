using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PushSharp;
using PushSharp.Apple;
using System.IO;

namespace ExpenseReportServer.Models
{
    public static  class Push
    {
        private static PushBroker push;
       
        public static void pushNotifcationToApple(string token, string message, int badge)
        {
            if (push == null) {
                push = new PushBroker();
                String p12file = System.Web.HttpContext.Current.Server.MapPath("~/Content/ExpensReport.Push.Production.Certificates.p12");
                var appleCert = File.ReadAllBytes(p12file);
                push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "qingdao1!"));
             }
            push.QueueNotification(new AppleNotification()
           .ForDeviceToken(token)
           .WithAlert(message)
           .WithBadge(badge));
        }

    }
}