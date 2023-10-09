using System;
using System.Collections.Generic;
using System.Text;
using AWS_Referential.Query;
using Bloomberg;
using System.Linq;
using AWS_Referential.Management;
using AWS_Referential.Implementation;

namespace ReferentialFeeding
{
    public class FeedingFromBloomberg
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        #region dividendes
        public void GetAllDividendsEstimateFromBloomberg()
        {
            
            Query query = new Query();
            Management DBM = new Management();
            List<string> bloombergSecurityNames=  new List<string>(); 
            dynamic[,,] result;
            int Tol = 20;

            //GetAllStockInReferential
            var AllStocks = query.GetAllStocks();
            int NotMatchCount = 0;

            foreach (var Stock in AllStocks)
            {
                string ticker = Stock.Listings.Where(p => p.Mic == p.PrimaryMic).Select(p => p.BloombergCode).FirstOrDefault();
                if (ticker == null)
                {
                    ticker= Stock.Listings.Select(p => p.BloombergCode).FirstOrDefault();
                }
                //Get BBG Estimate
                bloombergSecurityNames.Add(ticker);
                BBCOMDataRequest wrapper = new BulkDataRequest(bloombergSecurityNames, "DV141");
                result = wrapper.Process_Full();
                int Size = result.Length/2-1;
                for(int i=0; i<= Size; i++)
                {
                   
                    DateTime BBGExDate  =ConvertBBGDate( result[i, 0, 0]);
                    Decimal BBGAmount = (decimal)result[i, 1, 0];

                    var DividendMatch = Stock.Dividends.Where(p => p.ExDate <= BBGExDate.AddDays(Tol) && p.ExDate >= BBGExDate.AddDays(-Tol)).ToList();
                    if (DividendMatch != null)
                    {
                        //Only One Dividend Wich Match
                        if (DividendMatch.Count == 1)
                        {
                            var DividendMatched = DividendMatch.FirstOrDefault();
                            string OtherEstimate = DividendMatched.OtherEstimates;
                            int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG")+5;
                            OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                            OtherEstimate = OtherEstimate+" ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                            if (OtherEstimate != DividendMatched.OtherEstimates)
                            {
                                DividendMatched.OtherEstimates = OtherEstimate;
                                DBM.UpdateDividend(DividendMatched);
                            }
                        }
                        else
                        {
                            #region Gestion des multiple Match
                            if (DividendMatch.Count > 1) {
                                Boolean IsMNatched = false;
                                //Multiple dividend Match, need to find a criteria the Amount ?
                                
                                if (i < Size )
                                {
                                    if (ConvertBBGDate(result[i + 1, 0, 0]) == BBGExDate)
                                    {
                                        decimal OtherAount = (decimal)result[i + 1, 1, 0];
                                        var FirstMatch = DividendMatch[0];
                                        var SecondMatch = DividendMatch[1];
                                        if (BBGAmount > OtherAount)
                                        {
                                            if (FirstMatch.GrossAmount > SecondMatch.GrossAmount)
                                            {
                                                string OtherEstimate = FirstMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != FirstMatch.OtherEstimates)
                                                {
                                                    FirstMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(FirstMatch);
                                                }
                                            }
                                            else
                                            {
                                                string OtherEstimate = SecondMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != SecondMatch.OtherEstimates)
                                                {
                                                    SecondMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(SecondMatch);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (FirstMatch.GrossAmount < SecondMatch.GrossAmount)
                                            {
                                                string OtherEstimate = FirstMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != FirstMatch.OtherEstimates)
                                                {
                                                    FirstMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(FirstMatch);
                                                }
                                            }
                                            else
                                            {
                                                string OtherEstimate = SecondMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != SecondMatch.OtherEstimates)
                                                {
                                                    SecondMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(SecondMatch);
                                                }
                                            }
                                        }

                                    }
                                }
                                if (i > 0) 
                                {
                                    if (ConvertBBGDate(result[i - 1, 0, 0]) == BBGExDate)
                                    {
                                        decimal OtherAount = (decimal)result[i - 1, 1, 0];
                                        var FirstMatch = DividendMatch[0];
                                        var SecondMatch = DividendMatch[1];
                                        if (BBGAmount > OtherAount)
                                        {
                                            if (FirstMatch.GrossAmount > SecondMatch.GrossAmount)
                                            {
                                                string OtherEstimate = FirstMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != FirstMatch.OtherEstimates)
                                                {
                                                    FirstMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(FirstMatch);
                                                   
                                                }
                                            }
                                            else
                                            {
                                                string OtherEstimate = SecondMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != SecondMatch.OtherEstimates)
                                                {
                                                    SecondMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(SecondMatch);
                                                    
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (FirstMatch.GrossAmount < SecondMatch.GrossAmount)
                                            {
                                                string OtherEstimate = FirstMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != FirstMatch.OtherEstimates)
                                                {
                                                    FirstMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(FirstMatch);

                                                }
                                            }
                                            else
                                            {
                                                string OtherEstimate = SecondMatch.OtherEstimates;
                                                int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                                OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                                OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                                IsMNatched = true;
                                                if (OtherEstimate != SecondMatch.OtherEstimates)
                                                {
                                                    SecondMatch.OtherEstimates = OtherEstimate;
                                                    DBM.UpdateDividend(SecondMatch);

                                                }
                                            }
                                        }

                                    }
                                }
                                if (IsMNatched == false)
                                {
                                    var FirstMatch = DividendMatch[0];
                                    var SecondMatch = DividendMatch[1];
                                    decimal FirstMatchV = MatchGennerator(FirstMatch, BBGAmount);
                                    decimal SecondMatchV = MatchGennerator(SecondMatch, BBGAmount);
                                    if (FirstMatchV> SecondMatchV)
                                    {
                                        string OtherEstimate = SecondMatch.OtherEstimates;
                                        int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                        OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                        OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                        if (OtherEstimate != SecondMatch.OtherEstimates)
                                        {
                                            SecondMatch.OtherEstimates = OtherEstimate;
                                            DBM.UpdateDividend(SecondMatch);
                                            IsMNatched = true;
                                        }

                                    }
                                    else
                                    {
                                        string OtherEstimate = FirstMatch.OtherEstimates;
                                        int indexOfBBGEsttimate = OtherEstimate.IndexOf("BBG") + 5;
                                        OtherEstimate = OtherEstimate.Remove(indexOfBBGEsttimate);
                                        OtherEstimate = OtherEstimate + " ExDate= " + BBGExDate.ToString("dd/MM/yyyy") + " Amount= " + BBGAmount;
                                        if (OtherEstimate != FirstMatch.OtherEstimates)
                                        {
                                            FirstMatch.OtherEstimates = OtherEstimate;
                                            DBM.UpdateDividend(FirstMatch);
                                            IsMNatched = true;
                                        }
                                    }

                                }
                               

                            }
                            #endregion // Spagethi Code : ToDo : Refacor
                        }
                    }
                    else
                    {
                        NotMatchCount = NotMatchCount + 1;
                    }
                }
                bloombergSecurityNames.Clear();
            }
            Console.WriteLine("Number of Not Match Bloomberg={0}",NotMatchCount);
        }
        #endregion

        #region Price
        public void UpDateDerivativePrice()
        {
            Query query = new Query();
            Management DBM = new Management();
            DateTime StartDate;
            DateTime EndDate = DateTime.Now;
            List<string> bloombergSecurityNames = new List<string>();
            dynamic[,,] result;

            var AllDerivatives = query.GetAllDerivatives();
            //Sort only actives derivatives and no index future
            var AllDerivativesPriceToUpdate = AllDerivatives.Where(p => p.ExpirationDate > DateTime.Today && (p.DerivativeType != AWS_Referential.Enumeration.DerivativeType.FUTURE)).ToList();
            foreach (var DerivatvePriceToUpdate in AllDerivativesPriceToUpdate)
            {
                var Listing = DerivatvePriceToUpdate.Listing;
                if (Listing != null)
                {
                    var LastPrice = query.GetLastCrossPrice(Listing);
                    if (LastPrice != null)
                    {
                        StartDate = LastPrice.Date.Date ;
                    }
                    else
                    {
                        StartDate = new DateTime(2020, 01, 01);
                    }
                    bloombergSecurityNames.Add(Listing.BloombergCode);
                    BBCOMDataRequest wrapper = new IntradayTickRequest(bloombergSecurityNames, StartDate,EndDate);
                    result = wrapper.Process_Full();
                    var AllPrices = query.GetCrossPrice(Listing, StartDate);
                    for(int i = 0; i < result.Length/3; i++)
                    {
                        if((DerivatvePriceToUpdate.DerivativeType == AWS_Referential.Enumeration.DerivativeType.TOTALRETURNFUTURE && result[i, 0, 2] > 0) || (DerivatvePriceToUpdate.DerivativeType == AWS_Referential.Enumeration.DerivativeType.DIVIDENDFUTURE && result[i, 0, 2] >= 100))
                        {
                            Price PriceToInsert = new Price(Listing, "", AWS_Referential.Enumeration.PriceType.CROSS,result[i,0,0],result[i,0,1],result[i,0,2]);
                            if (IsPriceAlreadyInDb(AllPrices, PriceToInsert) == false) {
                                DBM.InsertPrice(PriceToInsert);
                            }
                        }

                    }
                    bloombergSecurityNames.Clear();
                }
            }
        }
        #endregion

        #region Earning
        public void UpdateEarningEstimateByIndex(string IndexName)
        {
            Query query = new Query();
            Management DBM = new Management();
            List<string> bloombergSecurityNames = new List<string>();
            List<string> bloombergFieldNames = new List<string>();
            dynamic[,,] result;
            var IndexListing = query.GetIndexCompositionListing(IndexName, DateTime.Today);
            bloombergSecurityNames = IndexListing.Select(p => p.BloombergCode).ToList();
            //Fields
     
            bloombergFieldNames.Add("RR057");



            //Send Requets
            BBCOMDataRequest wrapper = new ReferenceDataRequest(bloombergSecurityNames, bloombergFieldNames);
            result = wrapper.Process_Full();
            //Process Request
            for (int i=0; i < result.Length/30; i++) {

                try { 
                    EarningEstimate earningEstimate = new EarningEstimate();
                    Stock earningEstimateStock = query.GetStockByBBG(bloombergSecurityNames.ElementAt(i));

                    earningEstimate.Last_Update = DateTime.Today;

                    DBM.InsertOrUpdateEarningEstimate(earningEstimate);
                }
                catch(Exception e )
                {
                    log.FatalFormat("Issuer When trying to InsertOrUpdateEarning For Ticker", bloombergSecurityNames.ElementAt(i).ToString());
                }
            }


        }
        #endregion

        #region tools

        private DateTime ConvertBBGDate(Bloomberglp.Blpapi.Datetime BBGDate)
        {
            DateTime ConvertDate = new DateTime(BBGDate.Year, BBGDate.Month, BBGDate.DayOfMonth);
            return ConvertDate;
        }

        private decimal MatchGennerator(Dividend DividendToKeeep, decimal BBGAmount)
        {
            if (DividendToKeeep.GrossAmount != 0)
            {
                return Math.Abs(1 - (DividendToKeeep.GrossAmount / BBGAmount));
            }
            else
            {
                return 1;
            }
        }

        private bool IsPriceAlreadyInDb(List<Price> PricesInDb, Price PriceToCheck)
        {
            bool IsInDb = false;
            if(PricesInDb.Where(p=>p.ListingId==PriceToCheck.ListingId && p.PriceType==PriceToCheck.PriceType && p.Quantity==PriceToCheck.Quantity && p.Value==PriceToCheck.Value && p.Date.ToString("yyyyMMddhhmm") ==PriceToCheck.Date.ToString("yyyyMMddhhmm") && p.Source == PriceToCheck.Source).Count() > 0)
            {
                IsInDb = true;
            }
            return IsInDb;  
        }
        #endregion
    }
}
