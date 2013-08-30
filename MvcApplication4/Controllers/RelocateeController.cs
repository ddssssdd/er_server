using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseReportServer.Expense;
using ExpenseReportServer.Models;

namespace ExpenseReportServer.Controllers
{
    public class RelocateeController : ApiController
    {
        private ExpenseDB db = new ExpenseDB();
        [HttpGet]
        public ReturnStatus index(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            
            DisplayFactory<Relocatee> factory = new DisplayFactory<Relocatee>(relocatee);

            return new ReturnStatus
            {
                status = relocatee != null ? true : false,
                result = factory.sections
            };
        }
        [HttpGet]
        public ReturnStatus addresses(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = (from address in db.Addresses
                            where address.RelocateeID == id
                            select address).ToList();
                return new ReturnStatus { status = true, result = list };
            }
            else
            {
                return new ReturnStatus { status = false };
            }
        }

        [HttpGet]
        public ReturnStatus emails(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = (from email in db.Emails
                            where email.TableType == "RL" && email.TableID == id
                            select new {
                                id = email.EMailID,
                                emailType=email.EmailType.Description,
                                email=email.EMail
                            }).ToList();
                return new ReturnStatus { status = true, result = list };
            }
            else
            {
                return new ReturnStatus { status = false };
            }
        }

        [HttpGet]
        public ReturnStatus phones(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = (from phone in db.Phones
                            where phone.TableType == "RL" && phone.TableID == id
                            select new
                            {
                                id = phone.PhoneID,
                                phoneType = phone.PhoneType.Description,
                                phone = phone.PhoneNbr,
                                ext = phone.PhoneExt,
                                iconType = phone.IconType
                            }).ToList();
                return new ReturnStatus { status = true, result = list };
            }else
            {
                return new ReturnStatus { status = false };
            }
        }
        [HttpGet]
        public ReturnStatus wivendors(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = (from wivender in db.WiVendors
                            where (wivender.CompanyID == 0 || wivender.CompanyID == relocatee.ClientID) && wivender.EntityID == id
                            select wivender).ToList();
                return new ReturnStatus { status = true, result = list };
            }
            else
            {
                return new ReturnStatus { status = false };
            }
        }
        [HttpGet]
        public ReturnStatus payeeBankRoutings(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = (from payeeBankRouting in db.PayeeBankRoutings
                            where payeeBankRouting.EntityID == id
                            select payeeBankRouting).ToList();
                return new ReturnStatus { status = true, result = list };
            }
            else
            {
                return new ReturnStatus { status = false };
            }
        }
        [HttpGet]
        public ReturnStatus services(int id)
        {
            return new ReturnStatus { status = true, result = new RelocateeService().items(db, id) };
        }
    }
}
