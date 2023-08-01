using System;
using System.Collections.Generic;
using System.Text;
using AWS_Referential.Implementation;
using AWS_Referential.Query;
using System.Linq;
using Bloomberg;
using AWS_Referential.Enumeration;

namespace ReferentialChecks
{
    public class IndexChecker
    {
        public decimal CalculateLastInSameCurrency(BasketPriceComposition basketPriceComposition, CurrencyCode IndexCurrency, string IndexMic, Boolean IsTotalReturnIndex)
        {

            //this is ugly but no choice because of bloomberg and listing
            List<string> bloombergSecurityNames = new List<string>();
            List<string> bloombergFieldNames = new List<string>();
            List<string> CurrencyList = new List<string>();
            Dictionary<string, decimal> DividendsExToday = new Dictionary<string, decimal>();

            bloombergFieldNames.Add("LAST_PRICE");
            Query query = new Query();
            decimal Last = 0;
            dynamic[,,] result;
            //No choice because of Lazy Loading
            basketPriceComposition = query.GetBasketPriceCompositionById(basketPriceComposition.UniqueId);
            var Stocks = query.GetAllStocks();
            //First Get All Last price for each basketpricecomponent.
            foreach (var BasketPriceComponent in basketPriceComposition.Composition)
            {
                string BBGCode="";
                string Currency = "";
                //Get Listing and Addit 
                //Ugly Fix
                var stock = query.GetStockById(BasketPriceComponent.InstrumentId);
                var AllListings = Stocks.Where(p => p.UniqueId == BasketPriceComponent.InstrumentId).FirstOrDefault().Listings.ToList();
                var StockDividendExToday = stock.Dividends.Where(p => p.ExDate == DateTime.Today).ToList();
                if (AllListings.Count > 1)
                {
                    //Multiple Listing Need to choose the right one
                    if (IndexMic == "None")
                    {
                        //10-12-2021 Not Sure about this
                        //var StockListing = AllListings.Where(p => p.CurrencyCode == IndexCurrency).FirstOrDefault();
                        //if (StockListing == null)
                        //{
                        var  StockListing = AllListings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault();
                        //}
                        BBGCode = StockListing.BloombergCode;
                        Currency = StockListing.CurrencyCode.ToString();
                    }
                    else
                    {
                        var StockListing = AllListings.Where(p => p.Mic == IndexMic).FirstOrDefault();
                        if (StockListing == null)
                        {
                            StockListing = AllListings.Where(p => p.CurrencyCode == IndexCurrency).FirstOrDefault();
                            if (StockListing == null)
                            {
                                StockListing = AllListings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault();
                            }
                        }
                        BBGCode = StockListing.BloombergCode;
                        Currency = StockListing.CurrencyCode.ToString();
                    }
                }
                else
                {
                    //only one listing
                    var StockListing = AllListings.FirstOrDefault();
                    BBGCode = StockListing.BloombergCode;
                    Currency = StockListing.CurrencyCode.ToString();
                }
                CurrencyList.Add(Currency);
                bloombergSecurityNames.Add(BBGCode);
                if (IsTotalReturnIndex && (StockDividendExToday.Count()>0))
                {
                    DividendsExToday.Add(BBGCode, StockDividendExToday.Select(p=>p.GrossAmount).Sum());
                }
            }
            int Offset = GetWorkingDay(DateTime.Today);
            BBCOMDataRequest wrapper = new HistoricalDataRequest(bloombergSecurityNames, bloombergFieldNames, DateTime.Today.AddDays(Offset), DateTime.Today.AddDays(Offset),false);//there
            result = wrapper.Process_Full();
            var FxDic = GetFxDictionnary(CurrencyList,IndexCurrency.ToString());

            int i = 0;
            foreach (var BasketPriceComponent in basketPriceComposition.Composition)
            {
                var CurrentListing = query.GetListingByBBG(bloombergSecurityNames[i]);
                decimal multiplier = Convert.ToDecimal(CurrentListing.CurrencyMultiplier);
                decimal DividendAmountExToday = DividendsExToday.ContainsKey(bloombergSecurityNames[i])==false ? 0 : DividendsExToday[bloombergSecurityNames[i]];
                string currency = CurrentListing.CurrencyCode.ToString();
                Last = Last+ FxDic[currency] * BasketPriceComponent.Units * BasketPriceComponent.PriceAdjustmentFactor * BasketPriceComponent.FreeFloatAdjustmentFactor*BasketPriceComponent.OtherAdjustmentFactor1*BasketPriceComponent.OtherAdjustmentFactor2 *(Convert.ToDecimal( result[i, 0, 1])- DividendAmountExToday )* multiplier;
                i++;
            }
            Last = Last / basketPriceComposition.Divisor;
            return Math.Round( Last,2);
        }

        public Dictionary<string, List<decimal>> CheckAllIndexesLast()
        {
            //get all basketprices
            Query query = new Query();
            dynamic[,,] result;
            var AllBasketPrices = query.GetAllBasketPrices();
            Dictionary<string, List<decimal>> CheckResult = new Dictionary<string, List<decimal>>();
            int Offset = GetWorkingDay(DateTime.Today);
            foreach (var BasketPrice in AllBasketPrices)
            {
                //Get the last BastPriceComposition
                var BasketPriceComposition = BasketPrice.PriceComposition.Where(p => p.CompositionEndDate == null).FirstOrDefault();
                var Listing = query.GetListingsFromInstrumentId(BasketPrice.UniqueId).FirstOrDefault();
                decimal MyLast = 0;
                //try catch in case there is a bank holiday yesterday, bloom return empty
                try
                {
                    if (BasketPrice.Name == "DAX")
                        {
                            MyLast = CalculateLastInSameCurrency(BasketPriceComposition,Listing.CurrencyCode,Listing.Mic,true);
                        }
                    else
                    {
                        MyLast = CalculateLastInSameCurrency(BasketPriceComposition, Listing.CurrencyCode, Listing.Mic, false);
                    }
                }
                catch
                {
                    
                }
                //Ugly
                if (BasketPrice.Name=="IBEX") { MyLast = Math.Round(MyLast, 1); }
                if (BasketPrice.Name == "UKX") { MyLast = Math.Round(MyLast, 2); }
                //Get Last from BBG
                List<string> bloombergSecurityNames = new List<string>();
                List<string> bloombergFieldNames = new List<string>();
                bloombergFieldNames.Add("LAST_PRICE");
                bloombergSecurityNames.Add(Listing.BloombergCode);
                BBCOMDataRequest wrapper = new HistoricalDataRequest(bloombergSecurityNames, bloombergFieldNames, DateTime.Today.AddDays(Offset), DateTime.Today.AddDays(Offset));
                result = wrapper.Process_Full();
                decimal BBGLast = 1;
                //try catch in case there is a bank holiday yesterday, bloom return empty
                try
                {
                     BBGLast = Convert.ToDecimal(result[0, 0, 1]);
                }
                catch
                {

                }
                if (BBGLast == MyLast)
                {
                    List<decimal> results = new List<decimal>();
                    results.Add(1);
                    results.Add(BBGLast);
                    results.Add(MyLast);
                    CheckResult.Add(Listing.BloombergCode, results);
                }
                else
                {
                    List<decimal> results = new List<decimal>();
                    results.Add(0);
                    results.Add(BBGLast);
                    results.Add(MyLast);
                    CheckResult.Add(Listing.BloombergCode, results);
                }

            }
            return CheckResult;
        }

        public int GetWorkingDay(DateTime date)
        {
            if(date.DayOfWeek==DayOfWeek.Monday)
            {
                return -3;

            }
            else
            {
                return -1;
            }
        }

        public Dictionary<string, decimal> GetFxDictionnary(List<string> ListOfCurency, string BaseCurrency)
        {
            var UniqCurrency = ListOfCurency.Distinct().ToList();

            Dictionary<string, decimal> FxDic = new Dictionary<string, decimal>();

            List<string> bloombergSecurityNames = new List<string>();
            List<string> bloombergFieldNames = new List<string>();
            bloombergFieldNames.Add("PX_LAST");
            dynamic[,,] result;

            //BaseCurrencyToBaseCurrencu
            FxDic.Add(BaseCurrency, 1);
            if (UniqCurrency.Count > 1) { 
                foreach(var curr in UniqCurrency)
                {
                    if (curr != BaseCurrency) {
                    bloombergSecurityNames.Add(BaseCurrency + curr + " F110 Curncy");
                    }
                }

                int Offset = GetWorkingDay(DateTime.Today);
                BBCOMDataRequest wrapper = new HistoricalDataRequest(bloombergSecurityNames, bloombergFieldNames, DateTime.Today.AddDays(Offset), DateTime.Today.AddDays(Offset), false);
                result = wrapper.Process_Full();
                int i = 0;
                foreach (var curr in UniqCurrency)
                {
                    if (curr != BaseCurrency)
                    {
                        FxDic.Add(curr, Convert.ToDecimal(1 / result[i, 0, 1]));
                        i++;
                    }
                   
                }
            }
            return FxDic;
        }

    }
}
