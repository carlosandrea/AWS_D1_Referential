
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using AWS_Referential;
using AWS_Referential.Query;
using System;
using System.IO;
using System.Text;
using CsvHelper;
using ReferentialChecks;
using AWS_Referential.Implementation;
using AWS_Referential.Enumeration;

namespace Reporting
{
    
    public class Report
    {
        private string _send_mail_adress = "sender adress";
        private string password = "password";
        private string _receiver_mail_adress = "receiver";
        private string _dividends_files_path = @"C:\Dev\AWS\AWS_Referential_Reporting\csv";
        private string _composition_files_path = @"C:\Dev\AWS\AWS_Referential_Reporting\csv";
        private string _Earnings_files_path = @"C:\Dev\AWS\AWS_Referential_Reporting\csv";

        private void SendEmail(string Subject, string Body,string AttachementPath=null)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(_send_mail_adress, password);
            client.EnableSsl = true;
            client.Credentials = credentials;

            MailMessage message = new MailMessage(_send_mail_adress, _receiver_mail_adress);
            message.Subject = Subject;
            message.IsBodyHtml = true;
            message.Body = Body;
            if (AttachementPath != null) { message.Attachments.Add (new Attachment(AttachementPath)); }
            client.Send(message);
        }

        private void SendEmail(string Subject, string Body,List<string> MailReceivers ,string AttachementPath = null)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(_send_mail_adress, password);
            client.EnableSsl = true;
            client.Credentials = credentials;

            foreach(string MailReceiver in MailReceivers) { 
                MailMessage message = new MailMessage(_send_mail_adress, MailReceiver);
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                if (AttachementPath != null) { message.Attachments.Add(new Attachment(AttachementPath)); }
                client.Send(message);
            }
        }

        #region referentialReport
        public void GenerateAndSendReferentialReport(DateTime Date) 
        {
            string body = "Modifications Report : " + "<br />";
            body=body+ "<br />";
            body = body+ GetModificationReport(Date);
            body = body + "<br />";
            body =body+ "Dividend Errors Report : " + "<br />";
            body = body + "<br />";
            body = body + GetDividendErrorsReport(Date);
            body = body + "<br />";
            body = body + GetIndexCheckerReport();
            body = body + "<br />";
            body = body + GetDuplicateDividendReport();
            body = body + "<br />";
            SendEmail("Referential Report as Of : " + Date.ToString(), body);
        }
        private string GetModificationReport(DateTime Date)
        {
            //excluding Earnings Estimate
            Query query = new Query();

            var Modifications = query.GetAllModifications(Date);
            var AllDividend = query.GetAlldDividends();
            string Body = "";
            foreach (var Modification in Modifications.Where(p => p.ObjectModification != ObjectModification.EARNINGESTIMATE))
            {
                if (Modification.ObjectModification == ObjectModification.DIVIDEND) {
                    var Instrument = AllDividend.Where(p => p.UniqueId == Modification.ObjectModifiedId).Select(p => p.Instrument).FirstOrDefault();
                    Body = Body + "Modification : " + Modification.ObjectModification + "/" + Instrument.Name + "/" + Modification.ModifiedField + "/" + Modification.OldValue + "/" + Modification.NewValue + "<br />";
                }
                else
                {
                    Body = Body + "Modification : " + Modification.ObjectModification + "/" + Modification.ObjectModifiedId + "/" + Modification.ModifiedField + "/" + Modification.OldValue + "/" + Modification.NewValue + "<br />";
                }

            }
            return Body;
        }
        private string GetDividendErrorsReport(DateTime Date)
        {
            Query query = new Query();

            var Errors = query.GetAllDividendErrors(Date);
            var AllDividend = query.GetAlldDividends();
            string Body = "";
            foreach (var Error in Errors)
            {
                    var Instrument = AllDividend.Where(p => p.MarkitId == Error.MarkitId).Select(p => p.Instrument).FirstOrDefault();
                    Body = Body + "Dividend Error : " + Error.Reason + "/" + Error.Ticker + "/"+Error.MarkitId + "<br />";
                
  
            }
            return Body;
        }
        
        private string GetIndexCheckerReport()
        {
            string OutPut = "";
            IndexChecker Checker = new IndexChecker();
            var CheckerOutput=Checker.CheckAllIndexesLast();
            decimal CountOkIndex = 0;
            foreach (var Result in CheckerOutput)
            {

                Convert.ToBoolean(Convert.ToInt16(Result.Value.ElementAt(0)));
                OutPut = OutPut + Result.Key +" "+ Convert.ToBoolean(Convert.ToInt16(Result.Value.ElementAt(0))) + " BBG Last =" + Result.Value.ElementAt(1) + " My Last Last =" + Result.Value.ElementAt(2) + "<br />";
                CountOkIndex =CountOkIndex+ Result.Value.ElementAt(0);
            }
            OutPut = "Number of Index Checked = " + CheckerOutput.Count() + " Number of Index Ok= " + CountOkIndex + "<br />" + OutPut;
            return OutPut;
        }
        
        private string GetDuplicateDividendReport()
        {
            DividendChecker CheckDiv = new DividendChecker();
            Query askdb = new Query();
            var AllInstrument = askdb.GetAllInstruments();
            var DuplicateDividends = CheckDiv.CheckPossibleDuplicateDividends();
            string Body="Duplicate Dividend Report" +"<br />";
            foreach(var dividend in DuplicateDividends)
            {
                Body = Body + AllInstrument.Where(p => p.UniqueId == dividend.InstrumentId).Select(p => p.Name).FirstOrDefault();
                Body = Body + "/" + dividend.UniqueId + "/" + dividend.ExDate + "/" + dividend.PayDate + "/" + dividend.GrossAmount + "/" + dividend.LastUpdate + "<br />";
            }
            return Body;
        }
        #endregion

        #region DividendReport
        public void GenerateIndexDividendReport(string BBGCODE, DateTime StartDate,DateTime EndDate, Boolean SendByMail=false)
        {
            string FileName = _dividends_files_path + BBGCODE+" Dividends Report" + " From=" + StartDate.ToString("dd-MM-yy") + "To=" + EndDate.ToString("dd-MM-yy") + "As Of=" + DateTime.Today.ToString("dd-MM-yy") + ".csv";
            using (var mem = File.OpenWrite(FileName))
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.WriteField("Ticker");
                csvWriter.WriteField("ExDate");
                csvWriter.WriteField("PayDate");
                csvWriter.WriteField("GrossAmount");
                csvWriter.WriteField("Currency");
                csvWriter.WriteField("Status");
                csvWriter.WriteField("Type");
                csvWriter.WriteField("Form");
                csvWriter.WriteField("IsScrip");
                csvWriter.WriteField("TaxJuridiction");
                csvWriter.WriteField("TaxCode");
                csvWriter.WriteField("OtherEstimate");
                csvWriter.WriteField("Notes");

                csvWriter.NextRecord();


                Query query = new Query();
                var Dividends = query.GetIndexDividends(BBGCODE, StartDate, EndDate);
                //Todo : ugly
                var Listings = query.GetAllStocks();

                foreach (var Dividend in Dividends)
                {
                    csvWriter.WriteField(Listings.Where(p=>p.UniqueId==Dividend.InstrumentId).FirstOrDefault().Listings.Where(p=>p.Mic==p.PrimaryMic).FirstOrDefault().BloombergCode);
                    csvWriter.WriteField(Dividend.ExDate);
                    csvWriter.WriteField(Dividend.PayDate);
                    csvWriter.WriteField(Dividend.GrossAmount);
                    csvWriter.WriteField(Dividend.DividendCurrency);
                    csvWriter.WriteField(Dividend.DividendStatus);
                    csvWriter.WriteField(Dividend.DividendType);
                    csvWriter.WriteField(Dividend.DividendForm);
                    csvWriter.WriteField(Dividend.IsScriptOptionnal);
                    csvWriter.WriteField(Dividend.TaxJuridiction);
                    csvWriter.WriteField(Dividend.TaxCode);
                    csvWriter.WriteField(Dividend.OtherEstimates);
                    csvWriter.WriteField(Dividend.Notes);

                    csvWriter.NextRecord();
                }

                writer.Flush();
               
            }
            if (SendByMail == true)
            {
                SendEmail(BBGCODE + " Dividends Report" + " From=" + StartDate.ToString("dd-MM-yy") + "To=" + EndDate.ToString("dd-MM-yy") + "As Of=" + DateTime.Today.ToString("dd-MM-yy"),"", FileName);
            }
        }
        public void GenerateIndexCompositionReport(string BBGCODE, DateTime CompositionDate, Boolean SendByMail = false)
        {
            string FileName = _composition_files_path + BBGCODE + " Composition Report" + " As Of=" + CompositionDate.ToString("dd-MM-yy") + ".csv";
            Query query = new Query();
            var BasketPriceComposition = query.GetIndexComposition(BBGCODE, CompositionDate);
            var IndexListing = query.GetIndexCompositionListing(BBGCODE, CompositionDate);

            using (var mem = File.OpenWrite(FileName))
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.WriteField("Index");
                csvWriter.WriteField(BBGCODE);
                csvWriter.NextRecord();
                csvWriter.WriteField("Date");
                csvWriter.WriteField(CompositionDate);
                csvWriter.NextRecord();
                csvWriter.WriteField("Divisor");
                csvWriter.WriteField(BasketPriceComposition.Divisor);
                csvWriter.NextRecord();

                csvWriter.WriteField("Ticker");
                csvWriter.WriteField("Weight");
                csvWriter.WriteField("PriceAdjustmentFactor");
                csvWriter.WriteField("NumberOfUnits");
                csvWriter.WriteField("FreeFloatAdjustementFactor");
                csvWriter.WriteField("OtherAdjustmentFactor1");
                csvWriter.WriteField("OtherAdjustmentFactor2");
                csvWriter.WriteField("Currency");
                csvWriter.NextRecord();

                foreach (var basketPriceComponent in BasketPriceComposition.Composition)
                {
                    Listing listing = IndexListing.Where(u => u.InstrumentId == basketPriceComponent.InstrumentId).FirstOrDefault();
                    string IndexComopnentListingBBGCODE = listing.BloombergCode;
                    string Currency = listing.CurrencyCode.ToString() ;

                    csvWriter.WriteField(IndexComopnentListingBBGCODE);
                    csvWriter.WriteField(basketPriceComponent.Weight);
                    csvWriter.WriteField(basketPriceComponent.PriceAdjustmentFactor);
                    csvWriter.WriteField(basketPriceComponent.Units);
                    csvWriter.WriteField(basketPriceComponent.FreeFloatAdjustmentFactor);
                    csvWriter.WriteField(basketPriceComponent.OtherAdjustmentFactor1);
                    csvWriter.WriteField(basketPriceComponent.OtherAdjustmentFactor2);
                    csvWriter.WriteField(Currency);

                    csvWriter.NextRecord();
                }

                writer.Flush();

            }
            if (SendByMail == true)
            {
                SendEmail(BBGCODE + " Composition Report" + " As Of=" + CompositionDate.ToString("dd-MM-yy"), "", FileName);
            }
        }

        public void GenerateIndexDividendModificationReport(string BBGCODE, DateTime StartDate, DateTime EndDate, DateTime ModificationSince, List<String> ReportReceivers, Boolean SendByMail = false)
        {     
            string FileName = _dividends_files_path + BBGCODE + " Dividends Modifications Report" + " From=" + StartDate.ToString("dd-MM-yy") + "To=" + EndDate.ToString("dd-MM-yy") + "As Of=" + DateTime.Today.ToString("dd-MM-yy") +" Modifications Since "+ ModificationSince.ToString("dd-MM-yy") + ".csv";
            List<Modification> IndexDivModification = new List<Modification>();
            using (var mem = File.OpenWrite(FileName))
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                Query query = new Query();
                var IndexDividends = query.GetIndexDividends(BBGCODE, StartDate, EndDate);
                var AllListings = query.GetAllStocks();
                var AllModifications = query.GetAllModifications(ModificationSince);

                foreach(var dividend in IndexDividends)
                {
                    IndexDivModification = AllModifications.Where(p => p.ObjectModification == ObjectModification.DIVIDEND && p.ObjectModifiedId == dividend.UniqueId).ToList();
                    if (IndexDivModification.Count != 0)
                    {
                        csvWriter.WriteField("Ticker");
                        csvWriter.WriteField(AllListings.Where(p => p.UniqueId == dividend.InstrumentId).FirstOrDefault().Listings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault().BloombergCode);
                        csvWriter.NextRecord();
                        csvWriter.WriteField("Dividend Id");
                        csvWriter.WriteField(dividend.UniqueId);
                        csvWriter.WriteField("Dividend ExDate");
                        csvWriter.WriteField(dividend.ExDate);
                        csvWriter.NextRecord();
                        csvWriter.WriteField("Modified Field");
                        csvWriter.WriteField("Old Value");
                        csvWriter.WriteField("New Value");
                        csvWriter.NextRecord();
                        foreach (var modification in IndexDivModification)
                        {
                            csvWriter.WriteField(modification.ModifiedField);
                            csvWriter.WriteField(modification.OldValue) ;
                            csvWriter.WriteField(modification.NewValue);
                            csvWriter.NextRecord();
                        }

                    }
                }
                writer.Flush();
            }
            if (SendByMail == true)
            {
                SendEmail(BBGCODE + " Dividends Report" + " From=" + StartDate.ToString("dd-MM-yy") + "To=" + EndDate.ToString("dd-MM-yy") + "As Of=" + DateTime.Today.ToString("dd-MM-yy"), "", ReportReceivers, FileName);
            }
        }
        #endregion

        #region EarningsReport
        public void GenerateIndexEarningModificationReport(string BBGCODE, DateTime ModificationSince, List<String> ReportReceivers, Boolean SendByMail=false)
        {
            string FileName = _Earnings_files_path + BBGCODE + " Earning Modifications Report" +  " Modifications Since " + ModificationSince.ToString("dd-MM-yy") + ".csv";
            List<Modification> IndexDivModification = new List<Modification>();
            using (var mem = File.OpenWrite(FileName))
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                Query query = new Query();
                var AllModifications = query.GetAllModifications(ModificationSince);
                var IndexComposition = query.GetIndexComposition(BBGCODE, DateTime.Today);
                var IndexListing = query.GetIndexCompositionListing(BBGCODE, DateTime.Today);

                Listing StockListing= new Listing();
                string ticker = "";
                csvWriter.WriteField("Ticker");
                csvWriter.WriteField("FieldModified");
                csvWriter.WriteField("OldValue");
                csvWriter.WriteField("NewValue");
                csvWriter.NextRecord();

                foreach (var Stock in IndexComposition.Composition)
                {
                    StockListing = IndexListing.Where(u => u.InstrumentId == Stock.InstrumentId).FirstOrDefault();
                    ticker = StockListing.BloombergCode;


                    if (AllModifications.Where(p=>p.ObjectModifiedId== StockListing.InstrumentId && p.ObjectModification==ObjectModification.EARNINGESTIMATE).Count() > 0)
                    {
                        foreach(var EarningModification in AllModifications.Where(p => p.ObjectModifiedId == StockListing.InstrumentId && p.ObjectModification == ObjectModification.EARNINGESTIMATE))
                        {
                            csvWriter.WriteField(ticker);
                            csvWriter.WriteField(EarningModification.ModifiedField);
                            csvWriter.WriteField(EarningModification.OldValue);
                            csvWriter.WriteField(EarningModification.NewValue);
                            csvWriter.NextRecord();
                        }
                    }

                }
                writer.Flush();
            }
            if (SendByMail == true)
            {
                SendEmail(BBGCODE+ " Earning Modifications Report" + " Modifications Since "+  ModificationSince.ToString("dd-MM-yy"), "", ReportReceivers, FileName);
            }

            }
        #endregion
    }

}

