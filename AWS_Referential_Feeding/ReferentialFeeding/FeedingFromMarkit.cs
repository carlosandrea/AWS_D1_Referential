using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using log4net.Util;
using Markit;
using MarkitComposition;
using MarkitDividend;
using AWS_Referential.DataBase;
using AWS_Referential.Enumeration;
using AWS_Referential.Implementation;
using AWS_Referential.Management;
using AWS_Referential.Query;


namespace ReferentialFeeding
{
    public class FeedingFromMarkit
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Asset
        public async Task InsertAllAssetFromComposition(string MarkitIndexId)
        {
            Management DBM = new Management();
            SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
            Query query = new Query();

            Task<String> compositiondetails = solaRestWrapper.GetCompositon(MarkitIndexId);
            await Task.WhenAll(compositiondetails);
            var markitCompositionObject = MarkitCompositionObject.FromJson(compositiondetails.Result);


            foreach (var constituent in markitCompositionObject.Result.Constituents)
            {
                var stockexit = query.GetStockByIsin(constituent.Isin);
                if (stockexit == null)
                {
                    try
                    {
                        //Create Instrument and Listing
                        Stock IndexConstituent = new Stock(constituent.Name, constituent.Isin, constituent.Cusip, constituent.Cusip);
                        AWS_Referential.Enumeration.CurrencyCode currency = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), constituent.ListingCurrency.ToString().ToUpper());
                        decimal currmultiplier = 1;
                        if (constituent.ListingCurrency.ToString().ToUpper() == "GBX")
                        {
                            currmultiplier = 0.01M;
                        }


                        AWS_Referential.Implementation.Listing IndexConstituenListing = new AWS_Referential.Implementation.Listing(constituent.Ric, constituent.Mic, constituent.Mic, constituent.Bloomberg, constituent.Sedol, constituent.Cusip, constituent.Name, currency, currmultiplier);
                        IndexConstituenListing.Instrument = IndexConstituent;
                        IndexConstituent.Listings.Add(IndexConstituenListing);
                        //Add them in referential
                        DBM.InsertStock(IndexConstituent);
                    }
                    catch
                    {
                        log.FatalFormat("There was an issue when trying to insert a new stock instrument,stock Isin=", constituent.Isin);
                    }
                }


            }
        }
        #endregion

        #region Dividend
        public async Task InsertAllDividends()
        {
            Management DBM = new Management();

            //Get all Instrument in referential 
            Query query = new Query();
            var allstock = query.GetAllStocks();
            var alldividenderrors = query.GetAllDividendErrors();
            //get All dividend
            SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
            Task<String> alldividend = solaRestWrapper.GetDividendes();
            await Task.WhenAll(alldividend);
            Dividends dividends = Dividends.FromJson(alldividend.Result);

            //Traitement 
            //For Markit if a stock can pay a dividend in different currency, there is 2 dividends insert. We don't want that so we need to check

            dividends = RemovePossibleDuplicate(dividends, allstock, alldividenderrors, DBM);
            //Loop
            foreach (var dividend in dividends.Result)
            {

                if (allstock.Any(p => p.Isin == dividend.Isin))
                {

                    var divexist = query.GetDividendByMarkitId(dividend.ProviderDividendId.ToString());
                    if (divexist == null)
                    {
                        try
                        {
                            if (IsDateCorrectlySetUp(dividend) == true)
                            {
                                //To do : Refactor

                                //get related instrument
                                var instrument = allstock.Where(p => p.Isin == dividend.Isin).FirstOrDefault();
                                // create dividend
                                //parse currency Code
                                AWS_Referential.Enumeration.CurrencyCode currency = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), dividend.CurrencyCode.ToString().ToUpper());
                                //Parse Dividend Form
                                AWS_Referential.Enumeration.DividendForm dividendForm = (AWS_Referential.Enumeration.DividendForm)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendForm), dividend.DividendForm.ToString().ToUpper());
                                //Parse Div type
                                AWS_Referential.Enumeration.DividendType dividendtype = (AWS_Referential.Enumeration.DividendType)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendType), dividend.DividendType.ToString().ToUpper());
                                //Parse Tax Code
                                AWS_Referential.Enumeration.TaxCode taxcode = (AWS_Referential.Enumeration.TaxCode)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxCode), dividend.TaxCode.ToString().ToUpper());
                                //Parse Tax Juridiction
                                AWS_Referential.Enumeration.TaxJuridiction taxJuridiction = (AWS_Referential.Enumeration.TaxJuridiction)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxJuridiction), dividend.TaxJurisdiction.ToString().ToUpper());
                                //Parse AGM Status
                                AWS_Referential.Enumeration.Status agmstatus = GetAGMStatus(dividend);
                                //Other
                                decimal fxrate = GetFxRate(dividend);
                                decimal dividendamount = dividend.GrossAmount.HasValue ? (decimal)dividend.GrossAmount.Value : 0;
                                decimal currencymultiplier = dividend.ListingCurrency == MarkitDividend.CurrencyCode.Gbx ? 0.01M : 1;
                                Boolean isscriptoptionnal = dividend.IsScripOptional == 0 ? false : true;
                                string OtherEstimates = "BNP :  /JPM :  /BBG : ";
                                //Parse Status
                                AWS_Referential.Enumeration.Status dividendstatus = GetDividendStatus(dividend);
                                DateTime AgmDate = new DateTime(1900, 01, 01);
                                if (dividend.AgmDate.HasValue)
                                {
                                    AgmDate = dividend.AgmDate.Value.UtcDateTime;
                                }


                                Dividend divtoadd = new Dividend(currency, currencymultiplier, fxrate, dividendamount, AWS_Referential.Enumeration.DividendSource.MARKIT, dividendForm, dividendstatus, dividend.XdDate.Value.UtcDateTime, dividend.PayDate.Value.UtcDateTime, dividend.RecordDate.Value.UtcDateTime, AgmDate, agmstatus, dividendtype, isscriptoptionnal, taxcode, taxJuridiction, dividend.ProviderDividendId, dividend.Notes, OtherEstimates, instrument);
                                DBM.InsertDividend(divtoadd);

                            }
                            else
                            {
                                if (IsAlreadyInError(dividend, alldividenderrors) == false)
                                {
                                    DividendError dividenderrortoadd = new DividendError(dividend.ProviderDividendId, dividend.Bloomberg, "Date are not correctly Set Up");
                                    DBM.InsertDividendError(dividenderrortoadd);
                                    log.FatalFormat("There was an issue when trying to insert a new dividend, Date are not correctly Set Up Markit Dividend Id={0}", dividend.ProviderDividendId);
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            if (IsAlreadyInError(dividend, alldividenderrors) == false)
                            {
                                DividendError dividenderrortoadd = new DividendError(dividend.ProviderDividendId, dividend.Bloomberg, e.ToString());
                                DBM.InsertDividendError(dividenderrortoadd);
                                log.FatalFormat("There was an issue when trying to Insert a new dividend, Markit Dividend Id={0}", dividend.ProviderDividendId);
                                log.FatalFormat(e.Message.ToString());
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (IsDateCorrectlySetUp(dividend) == true)
                            {
                                //get related instrument
                                var instrument = allstock.Where(p => p.Isin == dividend.Isin).FirstOrDefault();
                                // create dividend
                                //parse currency Code
                                AWS_Referential.Enumeration.CurrencyCode currency = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), dividend.CurrencyCode.ToString().ToUpper());
                                //Parse Dividend Form
                                AWS_Referential.Enumeration.DividendForm dividendForm = (AWS_Referential.Enumeration.DividendForm)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendForm), dividend.DividendForm.ToString().ToUpper());
                                //Parse Div type
                                AWS_Referential.Enumeration.DividendType dividendtype = (AWS_Referential.Enumeration.DividendType)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendType), dividend.DividendType.ToString().ToUpper());
                                //Parse Tax Code
                                AWS_Referential.Enumeration.TaxCode taxcode = (AWS_Referential.Enumeration.TaxCode)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxCode), dividend.TaxCode.ToString().ToUpper());
                                //Parse Tax Juridiction
                                AWS_Referential.Enumeration.TaxJuridiction taxJuridiction = (AWS_Referential.Enumeration.TaxJuridiction)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxJuridiction), dividend.TaxJurisdiction.ToString().ToUpper());
                                //Parse AGM Status
                                AWS_Referential.Enumeration.Status agmstatus = GetAGMStatus(dividend);
                                //Other
                                decimal fxrate = GetFxRate(dividend);
                                decimal dividendamount = dividend.GrossAmount.HasValue ? (decimal)dividend.GrossAmount.Value : 0;
                                decimal currencymultiplier = dividend.ListingCurrency == MarkitDividend.CurrencyCode.Gbx ? 0.01M : 1;
                                Boolean isscriptoptionnal = dividend.IsScripOptional == 0 ? false : true;
                                //Parse Status
                                AWS_Referential.Enumeration.Status dividendstatus = GetDividendStatus(dividend);
                                DateTime AgmDate = new DateTime(1900, 01, 01);
                                if (dividend.AgmDate.HasValue)
                                {
                                    AgmDate = dividend.AgmDate.Value.UtcDateTime;
                                }

                                Dividend divtoadd = new Dividend(currency, currencymultiplier, fxrate, dividendamount, AWS_Referential.Enumeration.DividendSource.MARKIT, dividendForm, dividendstatus, dividend.XdDate.Value.UtcDateTime, dividend.PayDate.Value.UtcDateTime, dividend.RecordDate.Value.UtcDateTime, AgmDate, agmstatus, dividendtype, isscriptoptionnal, taxcode, taxJuridiction, dividend.ProviderDividendId, dividend.Notes, divexist.OtherEstimates, instrument);
                                DBM.UpdateDividend(divtoadd);
                            }
                            else
                            {
                                if (IsAlreadyInError(dividend, alldividenderrors) == false)
                                {
                                    DividendError dividenderrortoadd = new DividendError(dividend.ProviderDividendId, dividend.Bloomberg, "Date are not correctly Set Up");
                                    DBM.InsertDividendError(dividenderrortoadd);
                                    log.FatalFormat("There was an issue when trying to insert a new dividend, Date are not correctly Set Up Markit Dividend Id={0}", dividend.ProviderDividendId);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (IsAlreadyInError(dividend, alldividenderrors) == false)
                            {
                                DividendError dividenderrortoadd = new DividendError(dividend.ProviderDividendId, dividend.Bloomberg, e.ToString());
                                DBM.InsertDividendError(dividenderrortoadd);
                                log.FatalFormat("There was an issue when trying to Update  dividend, Markit Dividend Id={0}, Internal Id={1}", dividend.ProviderDividendId, divexist.UniqueId);
                                log.FatalFormat(e.Message.ToString());
                            }

                        }
                    }

                }
            }
            log.InfoFormat("All Dividends Have been Updated");
        }

        private Status GetDividendStatus(MarkitDividend.Result dividend)
        {
            try
            {
                //Before : dividend.ListingRawGrossAmountPublishState.ToString().ToUpper() == "COMPANY"
                if (dividend.AmountPublishState.ToString().ToUpper() == "COMPANY" &&
                    dividend.PayDatePublishState.ToString().ToUpper() == "COMPANY" && dividend.XdDatePublishState.ToString().ToUpper() == "COMPANY" &&
                     dividend.RecordDatePublishState.ToString().ToUpper() == "COMPANY"
                    ) { return Status.CONFIRMED; }
                else { return Status.ESTIMATE; }
            }
            catch { return Status.UNKNOW; }

        }

        private Status GetAGMStatus(MarkitDividend.Result dividend)
        {
            try
            {
                if (dividend.AgmDatePublishState.ToString().ToUpper() == "COMPANY") { return Status.CONFIRMED; }
                else { return Status.ESTIMATE; }
            }
            catch { return Status.UNKNOW; }

        }

        private decimal GetFxRate(MarkitDividend.Result dividend)
        {
            //When a dividend is a scrip, markit put the ratio in fx rate/ We don't want this
            decimal fxrate;
            if (dividend.ListingAmountCurrencyCode == dividend.ListingCurrency && dividend.ListingCurrency == dividend.CurrencyCode)
            {
                fxrate = 1M;
            }
            else
            {
                fxrate = dividend.FxRate.HasValue ? (decimal)dividend.FxRate.Value : 1;
            }
            return fxrate;
        }

        private Dividends RemovePossibleDuplicate(Dividends dividends, List<Stock> AllStock, List<DividendError> AllDividendErrors, Management DBM)
        {
            foreach (var dividendtocheck in dividends.Result)
            {
                {
                    if (AllStock.Any(p => p.Isin == dividendtocheck.Isin))
                    {
                        var possibleduplicate = dividends.Result.Any(p => p.Isin == dividendtocheck.Isin && p.XdDate == dividendtocheck.XdDate && p.PayDate == dividendtocheck.PayDate &&
                        p.RecordDate == dividendtocheck.RecordDate && p.DividendType == dividendtocheck.DividendType && p.Isin == dividendtocheck.Isin && p.ProviderDividendId != dividendtocheck.ProviderDividendId);
                        if (possibleduplicate == true)
                        {
                            //Select good listing Ugly but no choic
                            var stock = AllStock.Where(p => p.Isin == dividendtocheck.Isin).FirstOrDefault();
                            var mainlisting = stock.Listings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault();
                            if (mainlisting == null)
                            {
                                mainlisting = stock.Listings.FirstOrDefault();
                            }

                            //Is not main listinq
                            if (mainlisting.BloombergCode != dividendtocheck.Bloomberg)
                            {
                                dividends.Result = dividends.Result.Where(p => p.ProviderDividendId != dividendtocheck.ProviderDividendId).ToArray();
                                if (IsAlreadyInError(dividendtocheck, AllDividendErrors) == false)
                                {
                                    DividendError dividenderrortoadd = new DividendError(dividendtocheck.ProviderDividendId, dividendtocheck.Bloomberg, "Is Consider as duplicate");
                                    DBM.InsertDividendError(dividenderrortoadd);
                                    log.FatalFormat("There was a Dividend which was considered as duplicate in the list / Instrement Id={0},dividendId={1}", stock.UniqueId, dividendtocheck.ProviderDividendId);
                                }
                            }
                        }

                    }

                }
                //on force poas l'insertion d'une raison todelete de ne pas prendre en compte ce div, utilisation dans le cas de ABI BB pour ne pas avoir les div sur le ticker en zar
                if (AllDividendErrors.Where(p => p.MarkitId == dividendtocheck.ProviderDividendId && p.Reason == "TODELETE").FirstOrDefault() != null)
                {
                    dividends.Result = dividends.Result.Where(p => p.ProviderDividendId != dividendtocheck.ProviderDividendId).ToArray();
                }

            }
            return dividends;
        }

        private Boolean IsDateCorrectlySetUp(MarkitDividend.Result dividend)
        {
            if (dividend.XdDate != null && dividend.PayDate != null && dividend.RecordDate != null)
            {
                return true;
            }
            else { return false; }
        }

        private Boolean IsAlreadyInError(MarkitDividend.Result dividend, List<DividendError> dividendErrors)
        {
            if (dividendErrors.Where(p => p.MarkitId == dividend.ProviderDividendId).FirstOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Composition
        public async Task InsertComposition(string MarkitIndexId, long ListingId = 0)
        {
            Management DBM = new Management();
            SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
            Query query = new Query();

            Task<String> compositiondetails = solaRestWrapper.GetCompositon(MarkitIndexId);
            await Task.WhenAll(compositiondetails);
            //


            //
            var markitCompositionObject = MarkitCompositionObject.FromJson(compositiondetails.Result);

            //BasketPrice
            var BasketPrice = query.GetBasketPricesByIsin(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString());


            if (BasketPrice != null)
            {
                //A basketPriceAlreadyExist
                //get the most Up to Date Price Composition

                //to do : gerer le cas ou il y plusieurs basketpricecompositon
                var BasketPriceComposition = BasketPrice.PriceComposition.Where(p => p.CompositionEndDate == null).FirstOrDefault();
                if (BasketPriceComposition != null)
                {
                    //there is already a basketprice composition so we need to check if there are the same
                    var NewCompositionToCheck = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    decimal NewCompositionDivisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                    var AreCompoEqual = AreCompositionEquals(NewCompositionToCheck, NewCompositionDivisor, BasketPriceComposition.Composition.ToList(), BasketPriceComposition.Divisor);
                    if (AreCompoEqual == false)
                    {
                        //Update BasketPriceCompisition
                        //To do : Gestion des jours ouvres;
                        BasketPriceComposition.CompositionEndDate = DateTime.Today.AddDays(-1);
                        DBM.UpdateBasketPriceComposition(BasketPriceComposition);
                        //Insert new compo
                        BasketPriceComposition NewBasketPriceComposition = new BasketPriceComposition(BasketPrice, NewCompositionDivisor);
                        DBM.InsertBasketPriceComposition(NewBasketPriceComposition);

                        //Trying Something New
                        List<BasketPriceComponent> basketPriceComponents = new List<BasketPriceComponent>();
                        foreach (var constituent in NewCompositionToCheck)
                        {
                            basketPriceComponents.Add(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        }
                        DBM.InsertBasketPriceCompoments(basketPriceComponents);

                        //old
                        //foreach(var constituent in NewCompositionToCheck)
                        //{
                        //    DBM.InsertBasketPriceCompomemt(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        //}
                    }

                }
                else
                {
                    //TO do : refactor
                    //there is no basketpricecomposition so we need to create and insert one
                    decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                    BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor);
                    DBM.InsertBasketPriceComposition(BasketPriceComposition);
                    InsertAllConstituent(markitCompositionObject);
                    var NewComposition = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    foreach (var constituent in NewComposition)
                    {
                        DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                    }
                }
            }
            else
            {
                //TO do : refactor
                //BasketPrice Doens't exist, we need to create One
                BasketPrice = CreateBasketPriceFromMarkit(markitCompositionObject, ListingId);
                DBM.InsertBasketPrice(BasketPrice);
                BasketPrice = query.GetBasketPricesByIsin(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString());
                var BasketPriceListing = CreateBasketPriceListing(markitCompositionObject, BasketPrice, ListingId);
                DBM.InsertListing(BasketPriceListing);
                decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                var BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor);
                DBM.InsertBasketPriceComposition(BasketPriceComposition);
                InsertAllConstituent(markitCompositionObject);
                var NewComposition = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                foreach (var constituent in NewComposition)
                {
                    DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                }



            }

        }

        public async Task InsertCompositionShittyIndices(string MarkitIndexId, long ListingId = 0)
        {
            Management DBM = new Management();
            SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
            Query query = new Query();

            Task<String> compositiondetails = solaRestWrapper.GetCompositon(MarkitIndexId);
            await Task.WhenAll(compositiondetails);
            //


            //
            var markitCompositionObject = MarkitCompositionObject.FromJson(compositiondetails.Result);

            //BasketPrice
            var BasketPrice = query.GetBasketPricesByRic(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Ric).FirstOrDefault().ToString());


            if (BasketPrice != null)
            {
                //A basketPriceAlreadyExist
                //get the most Up to Date Price Composition

                //to do : gerer le cas ou il y plusieurs basketpricecompositon
                var BasketPriceComposition = BasketPrice.PriceComposition.Where(p => p.CompositionEndDate == null).FirstOrDefault();
                if (BasketPriceComposition != null)
                {
                    //there is already a basketprice composition so we need to check if there are the same
                    var NewCompositionToCheck = CreateCompositionFromMarkitCompositionObjectShittyIndices(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    decimal NewCompositionDivisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Ric == BasketPrice.Listing.Ric).FirstOrDefault().Divisor;
                    var AreCompoEqual = AreCompositionEquals(NewCompositionToCheck, NewCompositionDivisor, BasketPriceComposition.Composition.ToList(), BasketPriceComposition.Divisor);
                    if (AreCompoEqual == false)
                    {
                        //Update BasketPriceCompisition
                        //To do : Gestion des jours ouvres;
                        BasketPriceComposition.CompositionEndDate = DateTime.Today.AddDays(-1);
                        DBM.UpdateBasketPriceComposition(BasketPriceComposition);
                        //Insert new compo
                        BasketPriceComposition NewBasketPriceComposition = new BasketPriceComposition(BasketPrice, NewCompositionDivisor);
                        DBM.InsertBasketPriceComposition(NewBasketPriceComposition);

                        //Trying Something New
                        List<BasketPriceComponent> basketPriceComponents = new List<BasketPriceComponent>();
                        foreach (var constituent in NewCompositionToCheck)
                        {
                            basketPriceComponents.Add(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        }
                        DBM.InsertBasketPriceCompoments(basketPriceComponents);

                        //old
                        //foreach(var constituent in NewCompositionToCheck)
                        //{
                        //    DBM.InsertBasketPriceCompomemt(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        //}
                    }

                }
                else
                {
                    //TO do : refactor
                    //there is no basketpricecomposition so we need to create and insert one
                    decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Ric == BasketPrice.Listing.Ric).FirstOrDefault().Divisor;
                    BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor);
                    DBM.InsertBasketPriceComposition(BasketPriceComposition);
                    InsertAllConstituent(markitCompositionObject);
                    var NewComposition = CreateCompositionFromMarkitCompositionObjectShittyIndices(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    foreach (var constituent in NewComposition)
                    {
                        DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                    }
                }
            }
            else
            {
                //TO do : refactor
                //BasketPrice Doens't exist, we need to create One
                BasketPrice = CreateBasketPriceFromMarkit(markitCompositionObject, ListingId);
                DBM.InsertBasketPrice(BasketPrice);
                BasketPrice = query.GetBasketPricesByIsin(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Ric).FirstOrDefault().ToString());
                var BasketPriceListing = CreateBasketPriceListing(markitCompositionObject, BasketPrice, ListingId);
                DBM.InsertListing(BasketPriceListing);
                decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Ric == BasketPrice.Listing.Ric).FirstOrDefault().Divisor;
                var BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor);
                DBM.InsertBasketPriceComposition(BasketPriceComposition);
                InsertAllConstituent(markitCompositionObject);
                var NewComposition = CreateCompositionFromMarkitCompositionObjectShittyIndices(markitCompositionObject, BasketPrice, BasketPriceComposition);
                foreach (var constituent in NewComposition)
                {
                    DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                }



            }

        }

        public async Task InsertComposition(string MarkitIndexId, DateTime CompositionDate, long ListingId = 0)
        {
            Management DBM = new Management();
            SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
            Query query = new Query();

            Task<String> compositiondetails = solaRestWrapper.GetCompositon(MarkitIndexId, CompositionDate);
            await Task.WhenAll(compositiondetails);
            //


            //
            var markitCompositionObject = MarkitCompositionObject.FromJson(compositiondetails.Result);


            //BasketPrice
            var BasketPrice = query.GetBasketPricesByIsin(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString());


            if (BasketPrice != null)
            {
                //A basketPriceAlreadyExist
                //get the most Up to Date Price Composition

                //to do : gerer le cas ou il y plusieurs basketpricecompositon
                var BasketPriceComposition = BasketPrice.PriceComposition.Where(p => p.CompositionEndDate == null).FirstOrDefault();
                if (BasketPriceComposition != null)
                {
                    //there is already a basketprice composition so we need to check if there are the same
                    var NewCompositionToCheck = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    decimal NewCompositionDivisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                    var AreCompoEqual = AreCompositionEquals(NewCompositionToCheck, NewCompositionDivisor, BasketPriceComposition.Composition.ToList(), BasketPriceComposition.Divisor);
                    if (AreCompoEqual == false)
                    {
                        //Update BasketPriceCompisition
                        //To do : Gestion des jours ouvres;
                        BasketPriceComposition.CompositionEndDate = CompositionDate.AddDays(-1);
                        DBM.UpdateBasketPriceComposition(BasketPriceComposition);
                        //Insert new compo
                        BasketPriceComposition NewBasketPriceComposition = new BasketPriceComposition(BasketPrice, NewCompositionDivisor, CompositionDate);
                        DBM.InsertBasketPriceComposition(NewBasketPriceComposition);

                        //Trying Something New
                        List<BasketPriceComponent> basketPriceComponents = new List<BasketPriceComponent>();
                        foreach (var constituent in NewCompositionToCheck)
                        {
                            basketPriceComponents.Add(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        }
                        DBM.InsertBasketPriceCompoments(basketPriceComponents);

                        //old
                        //foreach(var constituent in NewCompositionToCheck)
                        //{
                        //    DBM.InsertBasketPriceCompomemt(new BasketPriceComponent(NewBasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                        //}
                    }

                }
                else
                {
                    //TO do : refactor
                    //there is no basketpricecomposition so we need to create and insert one
                    decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                    BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor, CompositionDate);
                    DBM.InsertBasketPriceComposition(BasketPriceComposition);
                    InsertAllConstituent(markitCompositionObject);
                    var NewComposition = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                    foreach (var constituent in NewComposition)
                    {
                        DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                    }
                }
            }
            else
            {
                //TO do : refactor
                //BasketPrice Doens't exist, we need to create One
                BasketPrice = CreateBasketPriceFromMarkit(markitCompositionObject, ListingId);
                DBM.InsertBasketPrice(BasketPrice);
                BasketPrice = query.GetBasketPricesByIsin(markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString());
                var BasketPriceListing = CreateBasketPriceListing(markitCompositionObject, BasketPrice, ListingId);
                DBM.InsertListing(BasketPriceListing);
                decimal Divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
                var BasketPriceComposition = new BasketPriceComposition(BasketPrice, Divisor, CompositionDate);
                DBM.InsertBasketPriceComposition(BasketPriceComposition);
                InsertAllConstituent(markitCompositionObject);
                var NewComposition = CreateCompositionFromMarkitCompositionObject(markitCompositionObject, BasketPrice, BasketPriceComposition);
                foreach (var constituent in NewComposition)
                {
                    DBM.InsertBasketPriceCompoment(new BasketPriceComponent(BasketPriceComposition, constituent.Instrument, (decimal)constituent.Weight, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.Units, (decimal)constituent.FreeFloatAdjustmentFactor, (decimal)constituent.OtherAdjustmentFactor1, (decimal)constituent.OtherAdjustmentFactor2));
                }



            }

        }

        private BasketPrice CreateBasketPriceFromMarkit(MarkitCompositionObject markitCompositionObject, long ListingId = 0)
        {
            string Name = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Name).FirstOrDefault().ToString().ToUpper();
            string Isin = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString().ToUpper();
            string Cusip = Isin; //Bad
            string FIGI = Isin; //Bad
            AWS_Referential.Enumeration.IndexReturnType returnType = (AWS_Referential.Enumeration.IndexReturnType)Enum.Parse(typeof(AWS_Referential.Enumeration.IndexReturnType), markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Variant).FirstOrDefault().ToString().ToUpper());
            BasketPrice basketPrice = new BasketPrice(Name, Isin, FIGI, Cusip, returnType);

            return basketPrice;

        }

        private AWS_Referential.Implementation.Listing CreateBasketPriceListing(MarkitCompositionObject markitCompositionObject, BasketPrice basketPrice, long ListingId = 0)
        {
            string Name = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Name).FirstOrDefault().ToString().ToUpper();
            string Ric = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Ric).FirstOrDefault().ToString();
            string Mic = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Mic).FirstOrDefault().ToString();
            string PrimaryMic = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Mic).FirstOrDefault().ToString();
            string BloombergCode = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Bloomberg).FirstOrDefault().ToString();
            string Isin = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.Isin).FirstOrDefault().ToString().ToUpper();
            string Sedol = Isin; //Bad
            string Cusip = Isin; //Bad
            string FIGI = Isin; //Bad
            AWS_Referential.Enumeration.CurrencyCode currencyCode = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.ListingCurrency).FirstOrDefault().ToString().ToUpper());
            decimal CurrencyMultiplier = markitCompositionObject.Result.Listings.Where(p => p.ListingId == ListingId).Select(p => p.ListingCurrency).FirstOrDefault().ToString() == "GBx" ? 0.01M : 1;
            AWS_Referential.Implementation.Listing BasketPriceListing = new AWS_Referential.Implementation.Listing(Ric, Mic, PrimaryMic, BloombergCode, Sedol, Cusip, Name, currencyCode, CurrencyMultiplier, basketPrice);

            return BasketPriceListing;
        }
        private void InsertAllConstituent(MarkitCompositionObject markitCompositionObject)
        {
            Management DBM = new Management();
            Query query = new Query();
            foreach (var constituent in markitCompositionObject.Result.Constituents)
            {
                var stockexist = query.GetStockByIsin(constituent.Isin);
                if (stockexist == null)
                {
                    try
                    {
                        //Create Instrument and Listing
                        Stock IndexConstituent = new Stock(constituent.Name, constituent.Isin, constituent.Cusip, constituent.Cusip);
                        AWS_Referential.Enumeration.CurrencyCode currency = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), constituent.ListingCurrency.ToString().ToUpper());
                        decimal currmultiplier = 1;
                        if (constituent.ListingCurrency.ToString().ToUpper() == "GBX")
                        {
                            currmultiplier = 0.01M;
                        }


                        AWS_Referential.Implementation.Listing IndexConstituenListing = new AWS_Referential.Implementation.Listing(constituent.Ric, constituent.Mic, constituent.Mic, constituent.Bloomberg, constituent.Sedol, constituent.Cusip, constituent.Name, currency, currmultiplier);
                        IndexConstituenListing.Instrument = IndexConstituent;
                        IndexConstituent.Listings.Add(IndexConstituenListing);
                        //Add them in referential
                        DBM.InsertStock(IndexConstituent);
                    }
                    catch
                    {
                        log.FatalFormat("There was an issue when trying to insert a new stock instrument,stock Isin=", constituent.Isin);
                    }
                }
                //We need to check if we have the listing if not we need to add it
                else
                {
                    var listingexist = stockexist.Listings.Where(p => p.Mic == constituent.Mic).FirstOrDefault();
                    if (listingexist == null)
                    {
                        try
                        {
                            AWS_Referential.Enumeration.CurrencyCode currency = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), constituent.ListingCurrency.ToString().ToUpper());
                            decimal currmultiplier = 1;
                            if (constituent.ListingCurrency.ToString().ToUpper() == "GBX")
                            {
                                currmultiplier = 0.01M;
                            }
                            string PrimaryMic = stockexist.Listings.Where(p => p.Mic == p.PrimaryMic).Select(p => p.Mic).FirstOrDefault();
                            AWS_Referential.Implementation.Listing IndexConstituenListing = new AWS_Referential.Implementation.Listing(constituent.Ric, constituent.Mic, PrimaryMic, constituent.Bloomberg, constituent.Sedol, constituent.Cusip, constituent.Name, currency, currmultiplier);
                            IndexConstituenListing.Instrument = stockexist;
                            DBM.InsertListing(IndexConstituenListing);
                        }
                        catch
                        {
                            log.FatalFormat("There was an issue when trying to insert a new stock listing,stock Isin=", constituent.Isin);
                        }
                    }
                }


            }
        }

        private List<BasketPriceComponent> CreateCompositionFromMarkitCompositionObject(MarkitCompositionObject markitCompositionObject, BasketPrice BasketPrice, BasketPriceComposition BasketPriceComposition)
        {
            Query query = new Query();
            List<BasketPriceComponent> NewCompositionToCheck = new List<BasketPriceComponent>();
            decimal divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Bloomberg == BasketPrice.Listing.BloombergCode).FirstOrDefault().Divisor;
            InsertAllConstituent(markitCompositionObject);
            foreach (var constituent in markitCompositionObject.Result.Constituents)
            {
                var stock = query.GetStockByIsin(constituent.Isin);
                if (stock != null)
                {
                    //Some ugly Fix There but DAX we get is ugly with some price into factor
                    decimal Factor1;
                    decimal Factor2;
                    decimal Factor3;
                    bool IsDax = false;
                    if (markitCompositionObject.Result.Listings[0].Bloomberg == "DAX Index") { IsDax = true; }
                    if (constituent.Factor1.HasValue && IsDax == false)
                    {
                        Factor1 = (decimal)constituent.Factor1;
                    }
                    else
                    {
                        Factor1 = 1;
                    }
                    if (constituent.Factor2.HasValue && IsDax == false)
                    {
                        Factor2 = (decimal)constituent.Factor2;
                    }
                    else
                    {
                        Factor2 = 1;
                    }
                    if (constituent.Factor3.HasValue)
                    {
                        Factor3 = (decimal)constituent.Factor3;
                    }
                    else
                    {
                        Factor3 = 1;
                    }
                    if (constituent.Factor4.HasValue && IsDax == true)
                    {
                        Factor1 = (decimal)constituent.Factor4;
                    }



                    NewCompositionToCheck.Add(new BasketPriceComponent(BasketPriceComposition, stock, (decimal)constituent.IndexQuantity, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.NumberOfUnits, Factor2, Factor1, Factor3));
                }

            }
            return NewCompositionToCheck;
        }

        //
        private List<BasketPriceComponent> CreateCompositionFromMarkitCompositionObjectShittyIndices(MarkitCompositionObject markitCompositionObject, BasketPrice BasketPrice, BasketPriceComposition BasketPriceComposition)
        {
            Query query = new Query();
            List<BasketPriceComponent> NewCompositionToCheck = new List<BasketPriceComponent>();
            decimal divisor = markitCompositionObject.Result.IndexPositions.Where(p => p.Ric == BasketPrice.Listing.Ric).FirstOrDefault().Divisor;
            InsertAllConstituent(markitCompositionObject);
            foreach (var constituent in markitCompositionObject.Result.Constituents)
            {
                var stock = query.GetStockByIsin(constituent.Isin);
                if (stock != null)
                {
                    //Some ugly Fix There but DAX we get is ugly with some price into factor
                    decimal Factor1;
                    decimal Factor2;
                    decimal Factor3;
                    bool IsDax = false;
                    if (markitCompositionObject.Result.Listings[0].Bloomberg == "DAX Index") { IsDax = true; }
                    if (constituent.Factor1.HasValue && IsDax == false)
                    {
                        Factor1 = (decimal)constituent.Factor1;
                    }
                    else
                    {
                        Factor1 = 1;
                    }
                    if (constituent.Factor2.HasValue && IsDax == false)
                    {
                        Factor2 = (decimal)constituent.Factor2;
                    }
                    else
                    {
                        Factor2 = 1;
                    }
                    if (constituent.Factor3.HasValue)
                    {
                        Factor3 = (decimal)constituent.Factor3;
                    }
                    else
                    {
                        Factor3 = 1;
                    }
                    if (constituent.Factor4.HasValue && IsDax == true)
                    {
                        Factor1 = (decimal)constituent.Factor4;
                    }



                    NewCompositionToCheck.Add(new BasketPriceComponent(BasketPriceComposition, stock, (decimal)constituent.IndexQuantity, (decimal)constituent.PriceAdjustmentFactor, (decimal)constituent.NumberOfUnits, Factor2, Factor1, Factor3));
                }

            }
            return NewCompositionToCheck;
        }


        private bool AreCompositionEquals(List<BasketPriceComponent> Composition1, decimal Divisor1, List<BasketPriceComponent> Composition2, Decimal Divisor2)
        {
            bool AreEqual = true;
            if (Composition1.Count() != Composition2.Count()) { AreEqual = false; }
            if (Divisor1 != Divisor2) { AreEqual = false; }
            foreach (var Component in Composition1)
            {
                var IsInCompo2 = Composition2.Where(p => p.InstrumentId == Component.InstrumentId).FirstOrDefault();
                if (IsInCompo2 == null) { AreEqual = false; }
                else
                {
                    if ((Math.Round(IsInCompo2.Weight, 15) != Math.Round(Component.Weight, 15)) || (Math.Round(IsInCompo2.PriceAdjustmentFactor, 15) != Math.Round(Component.PriceAdjustmentFactor, 15)) || (Math.Round(IsInCompo2.Units, 15) != Math.Round(Component.Units, 15)) ||
                        (Math.Round(IsInCompo2.FreeFloatAdjustmentFactor, 15) != Math.Round(Component.FreeFloatAdjustmentFactor, 15)) || (Math.Round(IsInCompo2.OtherAdjustmentFactor1, 15) != Math.Round(Component.OtherAdjustmentFactor1, 15)) || (Math.Round(IsInCompo2.OtherAdjustmentFactor2, 15) != Math.Round(Component.OtherAdjustmentFactor2, 15)))
                    {
                        AreEqual = false;
                    }
                }
            }

            foreach (var Component in Composition2)
            {
                var IsInCompo1 = Composition1.Where(p => p.InstrumentId == Component.InstrumentId).FirstOrDefault();
                if (IsInCompo1 == null) { AreEqual = false; }
                else
                {
                    if ((Math.Round(IsInCompo1.Weight, 15) != Math.Round(Component.Weight, 15)) || (Math.Round(IsInCompo1.PriceAdjustmentFactor, 15) != Math.Round(Component.PriceAdjustmentFactor, 15)) || (Math.Round(IsInCompo1.Units, 15) != Math.Round(Component.Units, 15))
                        || (Math.Round(IsInCompo1.FreeFloatAdjustmentFactor, 15) != Math.Round(Component.FreeFloatAdjustmentFactor, 15)) || (Math.Round(IsInCompo1.OtherAdjustmentFactor1, 15) != Math.Round(Component.OtherAdjustmentFactor1, 15)) || (Math.Round(IsInCompo1.OtherAdjustmentFactor2, 15) != Math.Round(Component.OtherAdjustmentFactor2, 15)))
                    {
                        AreEqual = false;
                    }
                }
            }

            return AreEqual;

        }
        #endregion
    }
}
