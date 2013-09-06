using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpenseReportServer.Expense;
using ExpenseReportServer.Models;
using System.Collections;

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
            return new ReturnStatus(new DisplayFactory<WiVendor>(db.WiVendors.Find(id)).sections);
        }
        [HttpGet]
        public ReturnStatus payeeBankRoutings(int id)
        {
            return new ReturnStatus(new DisplayFactory<PayeeBankRouting>(db.PayeeBankRoutings.Find(id)).sections);
            //return new ReturnStatus { status = true, result =new DisplayListFactory<PayeeBankRouting>(new PayeeBankRouting().RelocatePayeeBankRoutings(db, id)).items };
        }
        [HttpGet]
        public ReturnStatus services(int id)
        {
            return new ReturnStatus { status = true, result = new RelocateeService().items(db, id) };
        }
        [HttpGet]
        public object finance(int id)
        {
            return new ReturnStatus ( new DisplayFactory<VW_Finance>(db.Finances.Find(id)).sections );
            //return new DisplayListFactory<VW_Finance>(new VW_Finance().RelocateeFinances(db, id)).items;
        }
        [HttpGet]
        public object summary(int id)
        {
            Relocatee relocatee = db.Relocatees.Find(id);
            if (relocatee != null)
            {
                var list = new ArrayList();
                list.Add(new { title="Base Information",list=new DisplayFactory<Relocatee>(relocatee).items});
                list.Add(new
                {
                    title = "Address",
                    list = new DisplayListFactory<Address>(new Address().RelocateeAddress(db, id),
                        (Address item) => { return String.Format("{0},{1},{2}", item.Address1, item.City, item.State); },
                        (Address item) => { return item.ZipCode.ToString(); }).items
                });
                list.Add(new
                {
                    title = "Phone",
                    list = new DisplayListFactory<Phone>(new Phone().RelocatePhones(db, id),
                        (Phone item) => { return item.PhoneType.Description; },
                        (Phone item) => { return String.Format("{0} {1}", item.PhoneNbr, item.PhoneExt); }).items
                });
                list.Add(new
                {
                    title = "Email",
                    list = new DisplayListFactory<Email>(new Email().RelocateeEmails(db, id),
                        (Email item) => { return  item.EmailType.Description;},
                        (Email item) => { return item.EMail; }).items
                });
                list.Add(new
                {
                    title = "WiVendor",
                    list = new DisplayListFactory<WiVendor>(new WiVendor().RelocateeWivendors(db, relocatee),
                        (WiVendor wv) => { return "WiVendor"; },
                        (WiVendor wv) => { return wv.bankAccountName; },
                        (WiVendor wv) => { return String.Format("relocatee/wivendors/{0}", wv.WireID); } 
                        ).items
                });

                list.Add(new
                {
                    title = "PayeeBankRouting",
                    list = new DisplayListFactory<PayeeBankRouting>(new PayeeBankRouting().RelocatePayeeBankRoutings(db, id),
                        (PayeeBankRouting pb) => { return "Payee bank routing"; },
                        (PayeeBankRouting pb) => { return pb.ABA_Routing; },
                        (PayeeBankRouting pb) => {return String.Format("relocatee/payeeBankRoutings/{0}", pb.PayeeBankRoutingID);}
                        ).items
                });
                list.Add(new
                {
                    title = "Finance",
                    list = new DisplayListFactory<VW_Finance>(new VW_Finance().RelocateeFinances(db, id),
                        (VW_Finance f) => { return "Tax year"; },
                        (VW_Finance f) => { return f.Tax_Year; },
                        (VW_Finance f) => { return String.Format("relocatee/finance/{0}", f.FinanceID);}
                        ).items
                });
                
                return new ReturnStatus
                {
                    status = true,
                    result = list
                };
            }
            else
            {
                return new ReturnStatus { status = false, message = "Can not find relocateeID" };
            }
        }
    }
}
