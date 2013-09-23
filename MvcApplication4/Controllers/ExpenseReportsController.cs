using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using ExpenseReportServer.Expense;
using ExpenseReportServer.Models;
using ExpenseReportServer.Config;
namespace ExpenseReportServer.Controllers
{
    public class ExpenseReportsController : DbApiController
    {

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

        }
        //private const string upload_to_folder = @"D:\SC\Web\SC6-UploadedFile\SC6QA\om\ERReceipt\";
        //private const string reference_url = @"http://apps.synvata.com:8087/MC6Dev/UploadedFile/om/ERReceipt/";

        //private string upload_to_folder = System.Web.HttpContext.Current.Server.MapPath("~/Content/upload/");
        //private string reference_url = @"http://10.4.30.190:4205/Content/upload/";

        
        [HttpGet]
        public ReturnStatus reports(int relocateeId)
        {
            var list = db.ExpenseReports.Where((er) => er.RelocateeID == relocateeId).OrderByDescending(er => er.ReportDate).ToList();
            return new ReturnStatus { status=true, result = list };
        }
        [HttpGet]
        public ReturnStatus findReport(int reportId)
        {
            var report = db.ExpenseReports.Find(reportId);
            if (report != null)
            {
                report.details = db.ExpenseReportDetail.Where(detail => detail.ExpenseReportID == reportId).OrderByDescending(detail => detail.ExpenseDate).ToList();
                report.receipts = db.ExpenseReportReceipts.Where(receipt => receipt.ExpenseReportID == reportId).OrderByDescending(receipt => receipt.ExpenseReportReceiptsID).ToList();
                return new ReturnStatus { status = true, result = report };
            }
            else {
                return new ReturnStatus { status = false, message = "Cannot find report" };
            }
        }
        [HttpGet]
        public ReturnStatus existsReportName(String name,int relocateeId, int reportId=0)
        {
            Boolean exists;
            if (reportId == 0)
            {
                exists = db.ExpenseReports.Any(er =>er.Name.ToLower().Equals(name.Trim().ToLower()) && er.RelocateeID==relocateeId);
            }else{
                exists = db.ExpenseReports.Any(er => er.Name.ToLower().Equals(name.Trim().ToLower()) && er.RelocateeID == relocateeId && er.ExpenseReportID != reportId);
            }
            return new ReturnStatus{status = true,result=exists};
        }
        [HttpGet]
        public ReturnStatus removeReport(int reportId)
        {
            var report = db.ExpenseReports.Find(reportId);
            if (report == null)
            {
                return new ReturnStatus { status = false, message = "Cannot find report" };
            }
            db.Database.ExecuteSqlCommand("Delete from ExpenseReportDetail where ExpenseReportID={0}",  reportId);
            db.Database.ExecuteSqlCommand("Delete from ExpenseReportReceipts where ExpenseReportID={0}",  reportId);
            db.ExpenseReports.Remove(report);
            db.SaveChanges();
            return new ReturnStatus { status = true };
        }
        [HttpGet]
        public ReturnStatus details(int reportId)
        {
            return new ReturnStatus { status = true, result = db.ExpenseReportDetail.Where((detail) => detail.ExpenseReportID == reportId).OrderByDescending(detail=>detail.EnteredDate).ToList() };
        }
        [HttpGet]
        public ReturnStatus receipts(int reportId,int detailId=0)
        {
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath+Request.RequestUri.Query, string.Empty);
            var list = db.ExpenseReportReceipts.Where((detail) => detail.ExpenseReportID == reportId && (detailId == 0 ? 1 == 1 : detail.ExpenseReportDetailID == detailId)).ToList();
            list.ForEach(item => {
                //item.FileName = !String.IsNullOrEmpty(item.FileName) ? rootUrl +upload_folder+ item.FileName : item.FileName;
                //item.FileName = !String.IsNullOrEmpty(item.FileName) ? reference_url + item.FileName : "";
                item.FileName = !String.IsNullOrEmpty(item.FileName) ? AppSettings.Image_Reference_Url_Base + item.FileName : "";
            });
            return new ReturnStatus { status = true, result =list };
        }
        [HttpGet]
        public ReturnStatus expense(int relocateeId,int pageIndex=0,int pageSize=10)
        {
            var query = from ex in db.ExpenseView
                        where ex.EntityID == relocateeId 
                        select ex;
            int? count = query.Count();
            Decimal? amount = query.Sum(ex => ex.Amount);
            Decimal? netcheck = query.Sum(ex => ex.NetCheck);
            //var listgroup = query.GroupBy(ex => ex.ExpenseGroupFullName).ToList();
            query = query.OrderBy((ex)=>ex.ExpenseID).Skip(pageIndex*pageSize).Take(pageSize);
            //String sql = query.ToString();
            return new ReturnStatus { status = true, result = new {list= query.ToList(),amount=amount,netcheck=netcheck,count=count} };

        }
        [HttpGet]
        public ReturnStatus reportStatus()
        {
            return new ReturnStatus { status = true, result = db.ReportStatus.ToList() };
        }
        [HttpGet]
        public ReturnStatus purposes(int relocateeId)
        {
            var relocatee = db.Relocatees.Single(r=>r.RelocateeID==relocateeId);
            var client = db.Clients.Single(c => c.ClientID== relocatee.ClientID);
            String str_policyId = String.Format(",{0},", relocatee.PolicyID);
            String str_clientId = String.Format(",0_{0},", relocatee.ClientID);
            
            if (relocatee.PolicyID != 0 && relocatee.PolicyID != -1)
            {
                var query = from purpose in db.ExpensePurpose
                        where purpose.Deletable == 0 && (purpose.ClientID == 0 || purpose.ClientID == relocatee.ClientID) &&
                        (purpose.ClientTypeID == 0 || purpose.ClientTypeID == client.ClientTypeID) &&
                        (purpose.PolicyID == 0 || purpose.PolicyID == relocatee.PolicyID) &&
                        (!purpose.IsExcludePolicy.Contains(str_policyId))
                        orderby purpose.Description ascending
                        select purpose;
                return new ReturnStatus { status = true, result = query.ToList(), message = string.Format("clientid={0},policyId={1}", relocatee.ClientID, relocatee.PolicyID) };
            }
            else 
            {
                var query = from purpose in db.ExpensePurpose
                            where purpose.Deletable == 0 && (purpose.ClientID == 0 || purpose.ClientID == relocatee.ClientID) &&
                            (purpose.ClientTypeID == 0 || purpose.ClientTypeID == client.ClientTypeID) &&
                            (purpose.PolicyID == 0 || purpose.PolicyID == relocatee.PolicyID) &&
                            (!purpose.IsExcludePolicy.Contains(str_clientId))
                            orderby purpose.Description ascending
                            select purpose;
                return new ReturnStatus { status = true, result = query.ToList(),message= string.Format("clientid={0},policyId={1}",relocatee.ClientID,relocatee.PolicyID) };
            
            }
            
        }
        [HttpGet]
        public ReturnStatus services(int relocateeId)
        {
            var relocatee = db.Relocatees.Single(r => r.RelocateeID == relocateeId);
            var client = db.Clients.Single(c => c.ClientID == relocatee.ClientID);
            String str_policyId = String.Format(",{0},", relocatee.PolicyID);
            String str_clientId = String.Format(",0_{0},", relocatee.ClientID);

            if (relocatee.PolicyID != 0 && relocatee.PolicyID != -1)
            {
                var query = from service in db.ExpenseService
                            where service.Deletable == 0 && (service.ClientID == 0 || service.ClientID == relocatee.ClientID) &&
                            (service.ClientTypeID == 0 || service.ClientTypeID == client.ClientTypeID) &&
                            (service.PolicyID == 0 || service.PolicyID == relocatee.PolicyID) &&
                            (!service.IsExcludePolicy.Contains(str_policyId))
                            orderby service.Description ascending
                            select service;
                return new ReturnStatus { status = true, result = query.ToList(), message = string.Format("clientid={0},policyId={1}", relocatee.ClientID, relocatee.PolicyID) };
            }
            else
            {
                var query = from service in db.ExpenseService
                            where service.Deletable == 0 && (service.ClientID == 0 || service.ClientID == relocatee.ClientID) &&
                            (service.ClientTypeID == 0 || service.ClientTypeID == client.ClientTypeID) &&
                            (service.PolicyID == 0 || service.PolicyID == relocatee.PolicyID) &&
                            (!service.IsExcludePolicy.Contains(str_clientId))
                            orderby service.Description ascending
                            select service;
                return new ReturnStatus { status = true, result = query.ToList(), message = string.Format("clientid={0},policyId={1}", relocatee.ClientID, relocatee.PolicyID) };

            }
        }
        [HttpGet]
        public ReturnStatus addReport(int userid,int relocateeId, String name,String beginDate=null,String endDate=null,int peopleCovered=0,String description=null)
        {
            var relocatee = db.Relocatees.Single(r => r.RelocateeID == relocateeId);
            ExpenseReport er = new ExpenseReport();
            er.RelocateeID = relocateeId;
            er.ClientID = relocatee.ClientID;
            er.Name = name;
            if (beginDate!=null)
                er.PeriodBeginDate = DateTime.Parse(beginDate);
            if (endDate!=null)
                er.PeriodEndDate = DateTime.Parse(endDate);
            er.ReportDate = DateTime.Now;
            er.PeopleCovered = peopleCovered;
            er.ReportStatusID = 1;
            er.PaymentMethodID = 0;
            er.PaymentMethodVerifiedID = 0;
            er.HowSentID = 0;
            if (description !=null)
                er.Description = description;
            er.EnteredUser = userid;
            er.EnteredDate = DateTime.Now;
            er.UpdateUser = userid;
            er.UpdateDate = DateTime.Now;
            er.UpdateSeqNo = 1;
            db.ExpenseReports.Add(er);
            db.SaveChanges();

            return new ReturnStatus { status = true ,result=er,message=userid.ToString()};
        }
        [HttpGet]
        public ReturnStatus editReport(int reportId,int userid, String name, String beginDate = null, String endDate = null, int peopleCovered = 0, String description = null)
        {
            
            ExpenseReport er = db.ExpenseReports.Find(reportId);
            if (er == null)
            {
                return new ReturnStatus{status = false,message="Cannot find expense report"};
            }
            er.Name = name;
            if (beginDate != null)
                er.PeriodBeginDate = DateTime.Parse(beginDate);
            if (endDate != null)
                er.PeriodEndDate = DateTime.Parse(endDate);
            if (peopleCovered>0)
                er.PeopleCovered = peopleCovered;
            if (description != null)
                er.Description = description;
            er.UpdateUser = userid;
            er.UpdateDate = DateTime.Now;
            er.UpdateSeqNo = er.UpdateSeqNo + 1;
            
            db.SaveChanges();

            return new ReturnStatus { status = true, result = er, message = userid.ToString() };
        }
        [HttpGet]
        public ReturnStatus addDetail(int userid, int reportId, int purposeId, int serviceId, String date, decimal amount=0, decimal miles = 0)
        {
            var report = db.ExpenseReports.FirstOrDefault(er => er.ExpenseReportID == reportId);
            ExpenseReportDetail detail = new ExpenseReportDetail();
            detail.ExpenseReportID = reportId;
            detail.ExpenseDate = DateTime.Parse(date);
            detail.Amount = amount;
            detail.Mileage = miles;
            detail.ExpensePurposeID = purposeId;
            detail.ExpenseServiceID = serviceId;
            detail.EnteredUser = userid;
            detail.EnteredDate = DateTime.Now;
            detail.UpdateUser = userid;
            detail.UpdateDate = DateTime.Now;
            detail.UpdateSeqNo = 1;
            db.ExpenseReportDetail.Add(detail);
            db.SaveChanges();
           
            return new ReturnStatus { status = true, result = detail };
        }
        [HttpGet]
        public ReturnStatus editDetail(int userid, int detailId, int purposeId, int serviceId, String date=null, decimal amount = 0, decimal miles = 0)
        {

            ExpenseReportDetail detail = db.ExpenseReportDetail.Find(detailId);
            if (detail == null)
            {
                return new ReturnStatus { status = true, message = "Cannot find detail record" };
            }
            if (date!=null)
                detail.ExpenseDate = DateTime.Parse(date);
            if (amount>0)
                detail.Amount = amount;
            if (miles>0)
                detail.Mileage = miles;
            detail.ExpensePurposeID = purposeId;
            detail.ExpenseServiceID = serviceId;
            detail.UpdateUser = userid;
            detail.UpdateDate = DateTime.Now;
            detail.UpdateSeqNo = detail.UpdateSeqNo + 1;
            
            db.SaveChanges();

            return new ReturnStatus { status = true, result = detail };
        }
        [HttpGet]
        public ReturnStatus removeDetail(int detailId)
        {
            ExpenseReportDetail detail = db.ExpenseReportDetail.Find(detailId);
            if (detail == null)
            {
                return new ReturnStatus { status = false, message = "Cannot find detail record" };
            }
            db.Database.ExecuteSqlCommand("Delete from ExpenseReportReceipts where ExpenseReportID={0} and ExpenseReportDetailID={1}", detail.ExpenseReportID,detailId);
            db.ExpenseReportDetail.Remove(detail);
            db.SaveChanges();
            return new ReturnStatus { status = true };
        }


        [HttpGet]
        public ReturnStatus addReceiptNote(int userid, int reportId, int detailId, String note,int receiptId=0,int imageEdit=0)
        {
            if (receiptId==0)
            {

                ExpenseReportReceipts receipt = new ExpenseReportReceipts();
                receipt.ExpenseReportID = reportId;
                receipt.ExpenseReportDetailID = detailId;
                receipt.Notes = note;
                receipt.EnteredUser = userid;
                receipt.EnteredDate = DateTime.Now;
                receipt.UpdateDate = DateTime.Now;
                receipt.UpdateUser = userid;
                receipt.UpdateSeqNo = 1;
                db.ExpenseReportReceipts.Add(receipt);
                db.SaveChanges();
                return new ReturnStatus { status = true, result = receipt };
            }
            else
            {
                ExpenseReportReceipts receipt = db.ExpenseReportReceipts.Find(receiptId);
                if (receipt == null)
                {
                    return new ReturnStatus { status = false, message="Can not find this receipt" };
                }
                else
                {
                    receipt.Notes = note;
                    receipt.UpdateDate = DateTime.Now;
                    receipt.UpdateUser = userid;
                    receipt.UpdateSeqNo = receipt.UpdateSeqNo+ 1;
                    if (imageEdit == 1)
                    {
                        receipt.FileName = "";
                    }
                    db.SaveChanges();
                    return new ReturnStatus { status = true, result = receipt };
                }
            }
            
        }

        [HttpGet]
        public ReturnStatus changeReportStatus(int reportId)
        {
            ExpenseReport er = db.ExpenseReports.Find(reportId);
            if (er != null)
            {
                ERReportStatus status = db.ReportStatus.Find(er.ReportStatusID);
                String statusString = status != null ? status.Description : "Unknown";
                int count = 0;
                List<String> results = new List<String>();
                using (var localdb = new LocalDatabase())
                {
                    var list = localdb.UserDevices.Where(device =>
                               device.relocateeId == er.RelocateeID).ToList();
                    list.ForEach(device =>
                    {
                        if (!String.IsNullOrEmpty(device.token))
                        {
                            String message = String.Format("Your expense report [{0}] has been [{1}]!", er.Name,statusString);
                            Push.pushNotifcationToApple(device.token, message,1);
                            count++;
                            results.Add(message);
                        }
                    });

                    
                }
                return new ReturnStatus { status = true, message = String.Format("Send {0} notification(s)", count) ,result=results};
            }
            else
            {
                return new ReturnStatus { status = false, message = "Can not find this reportId" };
            }
            /*
            Push.pushNotifcationToApple("0c90770b7f56a3f8304784dc0520bcb8d212bccda9a0b579d9664d6b4d17cdbf", "Your expense report [name] has been [status]!", 1);
            return new ReturnStatus { status = true, result = null, message = "done" };
             */
        }
        public async Task<ReturnStatus> addReceiptNoteAndImage()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = System.Web.HttpContext.Current.Server.MapPath("~"+upload_folder);
            //string root = upload_to_folder;
            string root = AppSettings.Upload_To_Folder;
            var provider = new CustomMultipartFormDataStreamProvider(root);

            try
            {
              
                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);
                int receiptId = int.Parse(provider.FormData["receiptId"]);
                if (receiptId == 0)
                {

                    ExpenseReportReceipts receipt = new ExpenseReportReceipts();
                    receipt.ExpenseReportID = int.Parse(provider.FormData["reportId"]);
                    receipt.ExpenseReportDetailID = int.Parse(provider.FormData["detailId"]); ;
                    receipt.Notes = provider.FormData["note"];
                    receipt.EnteredUser = int.Parse(provider.FormData["userId"]);
                    receipt.EnteredDate = DateTime.Now;
                    receipt.UpdateDate = DateTime.Now;
                    receipt.UpdateUser = int.Parse(provider.FormData["userId"]);
                    receipt.UpdateSeqNo = 1;

                    foreach (var file in provider.FileData)
                    {
                        FileInfo fileInfo = new FileInfo(file.LocalFileName);
                        receipt.FileName = fileInfo.Name;
                    }

                    db.ExpenseReportReceipts.Add(receipt);
                    db.SaveChanges();

                    return new ReturnStatus { status = true, result = receipt };
                }
                else
                {
                    ExpenseReportReceipts receipt = db.ExpenseReportReceipts.Find(receiptId);
                    if (receipt == null)
                    {
                        return new ReturnStatus { status = false, message = "Can not find this receipt" };
                    }
                    else {
                        foreach (var file in provider.FileData)
                        {
                            FileInfo fileInfo = new FileInfo(file.LocalFileName);
                            receipt.FileName = fileInfo.Name;
                        }
                        receipt.Notes = provider.FormData["note"];
                        receipt.UpdateDate = DateTime.Now;
                        receipt.UpdateUser = int.Parse(provider.FormData["userId"]);
                        receipt.UpdateSeqNo = 1+ receipt.UpdateSeqNo;
                        db.SaveChanges();
                        return new ReturnStatus { status = true, result = receipt };
                    }
                }
              

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet]
        public ReturnStatus removeReceipt(int receiptId)
        {
            ExpenseReportReceipts receipt = db.ExpenseReportReceipts.Find(receiptId);
            if (receipt==null)
            {
                return new ReturnStatus { status = false, message = "Cannot find receipt" };
            }
            db.ExpenseReportReceipts.Remove(receipt);
            db.SaveChanges();
            return new ReturnStatus { status = true };
        }

        

        
    }
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return  DateTime.Now.ToString("yyyyMMddhhmmss")+name.Replace("\"", string.Empty);
        }
    }

    
}
