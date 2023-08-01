using AWS_Referential.DataBase;
using AWS_Referential.Implementation;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace AWS_Referential.Query
{
    public class Query
    {
        #region Instrument
        public Instrument GetInstrumentByIsin(string Isin)
        {
            using (var db = new DataBaseContext())
            {
                return db.InstrumentTable.Where(p => p.Isin == Isin).FirstOrDefault();


            }
        }
        public Instrument GetInstrumentByBBG(string BBG)
        {
            using (var db = new DataBaseContext())
            {
                var Listing = db.ListingTable.Where(p => p.BloombergCode == BBG).FirstOrDefault();
                return db.InstrumentTable.Where(p => p.UniqueId == Listing.InstrumentId).FirstOrDefault();


            }
        }

        public Stock GetStockByBBG(string BBG)
        {
            using (var db = new DataBaseContext())
            {
                var Listing = db.ListingTable.Where(p => p.BloombergCode == BBG).FirstOrDefault();
                return db.StockTable.Where(p => p.UniqueId == Listing.InstrumentId).Include(p => p.Dividends).Include(p => p.Listings).FirstOrDefault();


            }
        }
        public Stock GetStockByIsin(string Isin)
        {
            using (var db = new DataBaseContext())
            {
                return db.StockTable.Where(p => p.Isin == Isin).Include(p => p.Dividends).Include(p => p.Listings).FirstOrDefault();

            }
        }
        public Stock GetStockById(int UniqueId)
        {
            using (var db = new DataBaseContext())
            {
                return db.StockTable.Where(p => p.UniqueId == UniqueId).Include(p => p.Dividends).Include(p => p.Listings).FirstOrDefault();


            }
        }
        public List<String> GetAllInstrumentsIsin()
        {
            using (var db = new DataBaseContext())
            {
                return db.InstrumentTable.Select(p => p.Isin).Distinct().ToList();


            }
        }
        public List<String> GetAllStocktsIsin()
        {
            using (var db = new DataBaseContext())
            {
                return db.StockTable.Include(p => p.Dividends).Include(p => p.Listings).Select(p => p.Isin).Distinct().ToList();


            }
        }
        public List<Instrument> GetAllInstruments()
        {
            using (var db = new DataBaseContext())
            {
                return db.InstrumentTable.Distinct().ToList();
            }
        }
        public List<Stock> GetAllStocks()
        {
            using (var db = new DataBaseContext())
            {
                return db.StockTable.Distinct().Include(p => p.Listings).Include(p => p.Dividends).ToList();
            }
        }
        public List<Derivative> GetAllDerivatives()
        {
            using (var db = new DataBaseContext())
            {
                return db.DerivativeTable.Distinct().Include(p => p.Listing).ToList();
            }
        }
        #endregion

        #region Listing
        public List<Listing> GetListingsFromInstrumentId(int UniqueId)
        {
            using (var db = new DataBaseContext())
            {
                return db.ListingTable.Where(p => p.InstrumentId == UniqueId).ToList();
            }
        }

        public Listing GetListingByBBG(string BBGCode)
        {
            using (var db = new DataBaseContext())
            {
                return db.ListingTable.Include(p=>p.Instrument).Where(p => p.BloombergCode == BBGCode).FirstOrDefault();
            }
        }
        #endregion

        #region Derivative
        public Derivative GetDerivativeByIsin(string Isin)
        {
            using (var db = new DataBaseContext())
            {
                return db.DerivativeTable.Where(p => p.Isin == Isin).Include(p => p.Underlying).Include(p=>p.Listing).FirstOrDefault();


            }
        }
        #endregion

        #region BasketPrice
        public List<BasketPrice> GetAllBasketPrices()
        {
            using (var db = new DataBaseContext())
            {
                return db.BasketPriceTable.Distinct().Include(p => p.Modifications).Include(p => p.PriceComposition).ThenInclude(p => p.Composition).ThenInclude(p => p.Instrument).ToList();
            }
        }
        public BasketPrice GetBasketPricesById(int UniqueId)
        {
            using (var db = new DataBaseContext())
            {
                return db.BasketPriceTable.Where(p => p.UniqueId == UniqueId).Include(p => p.Listing).Include(p => p.Modifications).Include(p => p.PriceComposition).ThenInclude(p=>p.Composition).ThenInclude(p=>p.Instrument).FirstOrDefault();
            }
        }

        public BasketPrice GetBasketPricesByIsin(string Isin)
        {
            using (var db = new DataBaseContext())
            {
                return db.BasketPriceTable.AsNoTracking().Where(p => p.Isin == Isin).Include(p => p.Listing).Include(p => p.Modifications).Include(p => p.PriceComposition).ThenInclude(p => p.Composition).ThenInclude(p => p.Instrument).FirstOrDefault();
            }

        }
        public BasketPrice GetBasketPricesByRic(string Ric)
        {
            using (var db = new DataBaseContext())
            {
                var Listing = db.ListingTable.Where(p => p.Ric == Ric).FirstOrDefault();
                if (Listing != null)
                {
                    var BasketPrice = db.BasketPriceTable.Where(p => p.UniqueId == Listing.InstrumentId).Include(p => p.Listing).Include(p => p.Modifications).Include(p => p.PriceComposition.Select(q => q.Composition)).Include(p => p.PriceComposition.Select(q => q.Composition.Select(r => r.Instrument))).FirstOrDefault();
                    if (BasketPrice != null)
                    {
                        BasketPrice.Listing = Listing;
                    }
                    return BasketPrice;
                }
                else
                {
                    return null;
                }



            }
        }

        public BasketPrice GetBasketPricesByIsinWithDividends(string Isin)
        {
            using (var db = new DataBaseContext())
            {
                //There is some modification to do 
                var BasketPrice = db.BasketPriceTable.Where(p => p.Isin == Isin).Include(p => p.Listing).Include(p => p.Modifications).Include(p => p.PriceComposition.Select(q => q.Composition)).Include(p => p.PriceComposition.Select(q => q.Composition.Select(r => r.Instrument))).FirstOrDefault();
                for (int i = 0; i < BasketPrice.PriceComposition.Count; i++)
                {
                    var composition = BasketPrice.PriceComposition.ToList()[i];
                    for (int j = 0; j < composition.Composition.Count; j++)
                    {
                        var instrument = composition.Composition.ToList()[j];
                        BasketPrice.PriceComposition.ToList()[i].Composition.ToList()[j].Instrument = GetStockById(composition.Composition.ToList()[j].UniqueId);
                    }
                }
                return BasketPrice;

            }
        }
        #endregion

        #region BasketPriceComposition
        public BasketPriceComposition GetBasketPriceCompositionById(int UniqueId)
        {
            using (var db = new DataBaseContext())
            {
                return db.BasketPriceCompositionTable.Where(p => p.UniqueId == UniqueId).Include(p => p.Composition).ThenInclude(p=>p.Instrument).FirstOrDefault();

            }
        }
        #endregion

        #region dividend
        public Dividend GetDividendByMarkitId(string MarkitId)
        {
            using (var db = new DataBaseContext())
            {
                return  db.DividendTable.Where(p => p.MarkitId == MarkitId).Include(p => p.Modifications).FirstOrDefault();

            }

        }
        public List<Dividend> GetAlldDividends()
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendTable.Include(p => p.Instrument).ToList();


            }
        }

        public List<Dividend> GetAlldDividendsWithModifications()
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendTable.Include(p => p.Instrument).Include(p => p.Modifications).ToList();


            }
        }
        public List<Dividend> GetDividendsBetWeenDate(DateTime StartDate, DateTime EndDate)
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendTable.Where(p => p.ExDate <= EndDate && p.ExDate >= StartDate).Include(p => p.Instrument).Include(p => p.Modifications).ToList();


            }
        }

        public List<Dividend> GetDividendsBetWeenDate(int InstrumentId, DateTime StartDate, DateTime EndDate)
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendTable.Where(p => p.ExDate <= EndDate && p.ExDate >= StartDate && p.InstrumentId == InstrumentId).Include(p => p.Modifications).ToList();


            }
        }
        public List<Dividend> GetAllConfirmedDividends()
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendTable.Where(p => p.DividendStatus == Enumeration.Status.CONFIRMED).Include(p => p.Instrument).Include(p => p.Modifications).ToList();


            }
        }
        public List<DividendError> GetAllDividendErrors()
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendErrorTable.Distinct().ToList();
            }
        }
        public List<DividendError> GetAllDividendErrors(DateTime Date)
        {
            using (var db = new DataBaseContext())
            {
                return db.DividendErrorTable.Where(p => p.InsertionDate == Date).ToList();


            }
        }

        public Dividend GetDividendAsOf(Dividend Dividend, DateTime AsOf)
        {
            List<Modification> ListOfModification = (List<Modification>)Dividend.Modifications.Where(p => p.ModificationDate >= AsOf).OrderByDescending(p => p.ModificationDate).ToList();
            //Include doesnt not seem to work properly
            if (ListOfModification.Count() == 0)
            {
                using (var db = new DataBaseContext())
                {
                    ListOfModification = db.ModificationTable.Where(p => p.ObjectModification == Enumeration.ObjectModification.DIVIDEND && p.ObjectModifiedId == Dividend.UniqueId && p.ModificationDate >= AsOf).ToList();

                }
            }

            foreach (var Modification in ListOfModification)
            {
                System.Reflection.PropertyInfo prop = typeof(Dividend).GetProperty(Modification.ModifiedField);
                //At insertion of a new dividend prop is null
                if (prop == null)
                {
                    return null;
                }
                else
                {
                    var ActualValue = (prop.GetValue(Dividend));
                    if (ActualValue != null)
                    {
                        var OldValue = Cast(Modification.OldValue, ActualValue.GetType());
                        prop.SetValue(Dividend, OldValue);
                    }
                }
            }

            return Dividend;
        }
        #endregion

        #region Index
        public List<Dividend> GetIndexDividends(string BBG, DateTime StartDate, DateTime EndDate)
        {
            List<Dividend> IndexDividends = new List<Dividend>();
            using (var db = new DataBaseContext())
            {
                var Listing = db.ListingTable.Where(p => p.BloombergCode == BBG).FirstOrDefault();
                if (Listing != null)
                {
                    var Index = db.BasketPriceTable.Where(p => p.UniqueId == Listing.InstrumentId).FirstOrDefault();
                    if (Index != null)
                    {
                        var UTDBasketPriceComposition = db.BasketPriceCompositionTable.Where(p => p.BasketPriceId == Index.UniqueId && p.CompositionStartDate <= StartDate && p.CompositionEndDate == null).FirstOrDefault();
                        if (UTDBasketPriceComposition != null)
                        {
                            var BasketPriceComponents = db.BasketPriceComponentTable.Where(p => p.BasketPriceCompositionId == UTDBasketPriceComposition.UniqueId).ToList();
                            if (BasketPriceComponents != null)
                            {
                                foreach (var BasketPriceComponent in BasketPriceComponents)
                                {
                                    //Attention aux dividends special non retraite ici
                                    var BasketPriceComponentDividends = db.DividendTable.Where(p => p.InstrumentId == BasketPriceComponent.InstrumentId && p.ExDate >= StartDate && p.ExDate <= EndDate).ToList();
                                    if (BasketPriceComponentDividends != null)
                                    {
                                        IndexDividends.AddRange(BasketPriceComponentDividends);
                                    }
                                }

                            }
                            else
                            {
                                //No BasketPriceComponents
                                return null;
                            }
                        }
                        else
                        {
                            //No BasketpriceComposition
                            return null;
                        }
                    }
                    else
                    {
                        //Null instrument
                        return null;
                    }
                }
                else
                {
                    //Null Listing
                    return null;
                }
                return IndexDividends.OrderBy(p => p.ExDate).ToList();

            }
        }

        public BasketPriceComposition GetIndexComposition(string BBG, DateTime CompositionDate)
        {
            using (var db = new DataBaseContext())
            {
                var Listing = db.ListingTable.Where(p => p.BloombergCode == BBG).FirstOrDefault();
                if (Listing != null)
                {
                    var currency = Listing.CurrencyCode;
                    var Mic = Listing.Mic;
                    var Instrument = db.BasketPriceTable.Where(p => p.UniqueId == Listing.InstrumentId).FirstOrDefault();
                    if (Instrument != null)
                    {
                        var BasketpriceComposition = db.BasketPriceCompositionTable.Where(p => p.CompositionStartDate <= CompositionDate && p.CompositionEndDate >= CompositionDate && p.BasketPriceId == Instrument.UniqueId).Include(p => p.Composition).FirstOrDefault();
                        if (BasketpriceComposition == null)
                        {

                            BasketpriceComposition = db.BasketPriceCompositionTable.Where(p => p.CompositionStartDate <= CompositionDate && p.CompositionEndDate == null && p.BasketPriceId == Instrument.UniqueId).Include(p => p.Composition).FirstOrDefault();
                        }
                        if (BasketpriceComposition != null)
                        {


                            return BasketpriceComposition;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


            }
        }

        public List<Listing> GetIndexCompositionListing(string BBG, DateTime CompositionDate)
        {
            List<Listing> IndexCompositionListing = new List<Listing>();
            Query query = new Query();
            //Get Composition
            var IndexComposition = query.GetIndexComposition(BBG, DateTime.Today);
            var AllStocks = query.GetAllStocks();
            var IndexListing = query.GetListingByBBG(BBG);
            var IndexMic = IndexListing.Mic;
            var IndexCurrency = IndexListing.CurrencyCode;
            Listing StockListing = new Listing();
            foreach (var Stock in IndexComposition.Composition)
            {
                string ticker = "";
                var AllListings = AllStocks.Where(p => p.UniqueId == Stock.InstrumentId).FirstOrDefault().Listings.ToList();
                if (AllListings.Count > 1)
                {
                    //Multiple Listing Need to choose the right one
                    if (IndexMic == "None")
                    {
                        //10-12-2021 Not sure about this
                        //StockListing = AllListings.Where(p => p.CurrencyCode == IndexCurrency).FirstOrDefault();
                        //if (StockListing == null)
                        //{
                        StockListing = AllListings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault();
                        //}
                    }
                    else
                    {
                        StockListing = AllListings.Where(p => p.Mic == IndexMic).FirstOrDefault();
                        if (StockListing == null)
                        {
                            StockListing = AllListings.Where(p => p.CurrencyCode == IndexCurrency).FirstOrDefault();
                            if (StockListing == null)
                            {
                                StockListing = AllListings.Where(p => p.Mic == p.PrimaryMic).FirstOrDefault();
                            }
                        }
                    }
                }
                else
                {
                    //only one listing
                    StockListing = AllListings.FirstOrDefault();
                }


                IndexCompositionListing.Add(StockListing);
            }

            return IndexCompositionListing;
        }

        #endregion

        #region Modification
        public List<Modification> GetAllModifications(DateTime Date)
        {
            using (var db = new DataBaseContext())
            {
                return db.ModificationTable.Where(p => p.ModificationDate >= Date).ToList();


            }
        }
        #endregion

        #region Price
        public Price GetLastCrossPrice(Listing Listing)
        {
            using (var db = new DataBaseContext())
            {
                return db.PriceTable.Where(p => p.ListingId == Listing.UniqueId && p.PriceType == AWS_Referential.Enumeration.PriceType.CROSS).OrderByDescending(p => p.Date)
                       .FirstOrDefault();
            }
        }

        public List<Price> GetCrossPrice(Listing Listing, DateTime date)
        {
            using (var db = new DataBaseContext())
            {
                return db.PriceTable.Where(p => p.ListingId == Listing.UniqueId && p.PriceType == AWS_Referential.Enumeration.PriceType.CROSS && p.Date == date).OrderByDescending(p => p.Date).ToList();
            }
        }

        #endregion

        #region tools
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            switch (castTo.Name)
            {
                case "Status":
                    return (AWS_Referential.Enumeration.Status)Enum.Parse(typeof(AWS_Referential.Enumeration.Status), obj, true);


                case "DividendType":
                    return (AWS_Referential.Enumeration.DividendType)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendType), obj, true);

                case "CurrencyCode":
                    return (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), obj, true);
                case "DividendSource":
                    return (AWS_Referential.Enumeration.DividendSource)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendSource), obj, true);
                case "DividendOrigin":
                    return (AWS_Referential.Enumeration.DividendOrigin)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendOrigin), obj, true);
                case "DividendForm":
                    return (AWS_Referential.Enumeration.DividendForm)Enum.Parse(typeof(AWS_Referential.Enumeration.DividendForm), obj, true);
                case "TaxJuridiction":
                    return (AWS_Referential.Enumeration.TaxJuridiction)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxJuridiction), obj, true);
                case "TaxCode":
                    return (AWS_Referential.Enumeration.TaxCode)Enum.Parse(typeof(AWS_Referential.Enumeration.TaxCode), obj, true);

                default:
                    return Convert.ChangeType(obj, castTo);
            }
        }
        #endregion  
    }
}
