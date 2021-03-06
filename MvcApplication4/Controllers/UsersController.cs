﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseReportServer.Expense;
using ExpenseReportServer.Models;
using System.Data;
using System.Net.Mail;
using ExpenseReportServer.Config;
using System.Collections;

namespace ExpenseReportServer.Controllers
{
    public class UsersController : DbApiController
    {
        private LocalDatabase localDb = new LocalDatabase();
        
        [HttpGet]
        public ReturnStatus login(String username, String password)
        {
            
            var lists = db.Users.SqlQuery("select * from users where username=@p0 and dbo.clrOrionStauthDecryptString(password)=@p1 and userType='TR'", new object[] { username, password }).ToList();
            if (lists.Count == 1)
            {
                Users user = lists[0];
                var list_persons = db.Persons.Where((person) => person.PersonUserID == user.UserID).ToList();
                if (list_persons.Count == 0)
                {
                    return new ReturnStatus { status = false, message = "Could not find in person table" };
                }
                else
                {
                    return new ReturnStatus { status = true, result = list_persons[0] };
                }
                
            }
            else 
            {
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return new ReturnStatus { status = false, message = "Can not found user" };

            }
        }
        [HttpGet]
        public ReturnStatus loginMC(int personId)
        {
            var lists = db.Relocatees.Where((relocatee) => relocatee.PersonID == personId).ToList();
            if (lists.Count > 0)
            {
                return new ReturnStatus { status = true, result = lists };
            }
            else
            {
                return new ReturnStatus { status = false, message = "Could not found any relocatee records" };
            }
        }
        [HttpGet]
        public ReturnStatus registerToken(int userid, string token, string key,int relocateeId)
        {
            var list = localDb.UserDevices.Where(ud => ud.userId == userid && ud.key == key).ToList();
            if (list.Count > 0)
            {
                var ud = list[0];
                if (ud.token != token)
                {
                    ud.relocateeId = relocateeId;
                    ud.token = token;
                    localDb.SaveChanges();
                }
                
                return new ReturnStatus { status = true, result = ud,message="Update successfully"};
            }
            else
            {
                UserDevices ud = new UserDevices();
                ud.userId = userid;
                ud.token = token;
                ud.key = key;
                ud.relocateeId = relocateeId;
                localDb.UserDevices.Add(ud);
                localDb.SaveChanges();
                return new ReturnStatus { status = true, result = ud,message="Added token "};
            }
            

        }
        [HttpGet]
        public ReturnStatus clients()
        {
            var list = new ArrayList();
            AppSettings.clients().ForEach(cc => {
                list.Add(new { id = cc.ID, name = cc.ClientName });
            });
            return new ReturnStatus { status = true, result = list };
        }
        
        [HttpGet]
        public ReturnStatus send(String email, string subject, string body) {
            SendEMail(email, subject, body);
            return new ReturnStatus { message = "send" };
        }
        private void SendEMail(string emailid, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("synvata.developer@gmail.com", "qingdao1!");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("synvata.developer@gmail.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

    }
}
