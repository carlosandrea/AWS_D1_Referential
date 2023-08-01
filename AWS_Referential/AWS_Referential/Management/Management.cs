using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AWS_Referential.DataBase;
using AWS_Referential.Enumeration;
using AWS_Referential.Implementation;
using AWS_Referential.Query;


namespace AWS_Referential.Management
{
    public class Management
    {
        //To do : UpdateInstrument, Delete Instrument, UpdateListing, DeleteListing

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region Instrument
        public void InsertInstrument(Instrument instrument)
        {
            try
            {
                // insert
                using (var db = new DataBaseContext())
                {
                    var dbset = db.Set<Instrument>();
                    dbset.Add(instrument);
                    db.SaveChanges();
                    log.InfoFormat("There was an Instrument added Isin={0}", instrument.Isin);
                    Modification InstrumentInsertion = new Modification(ObjectModification.INSTRUMENT, instrument.UniqueId, "Insertion Of a New Instrument", "", "");
                    var dbm = db.Set<Modification>();
                    dbm.Add(InstrumentInsertion);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                log.FatalFormat("There Was an issue will trying to insert Instruement in Database, Instrument Isin={0}", instrument.Isin);
                log.FatalFormat(e.ToString());
            }
        }
        //To check
        public void UpdpateInstrument(Instrument instrument)
        {
            using (var db = new DataBaseContext())
            {
                var instrumentoupdate = db.InstrumentTable.Where(p => p.Isin == instrument.Isin).FirstOrDefault();
                //modif
                instrumentoupdate = db.InstrumentTable.Find(instrumentoupdate.UniqueId);
                if (instrumentoupdate != null)
                {
                    foreach (PropertyInfo propertyInfo in instrumentoupdate.GetType().GetProperties())
                    {
                        if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Modifications")
                        {
                            if (!Equals(propertyInfo.GetValue(instrumentoupdate), instrument.GetType().GetProperty(propertyInfo.Name).GetValue(instrument)))
                            {
                                propertyInfo.SetValue(instrumentoupdate, instrument.GetType().GetProperty(propertyInfo.Name).GetValue(instrument));

                            }
                        }
                    }
                    //db.DividendTable.AddOrUpdate(dividendtoupdate);
                    // db.SaveChanges();

                    //I don't why but if you don't put this it won't work
                    var entry = db.Entry(instrumentoupdate);//added
                    var state = entry.State;
                    //modif
                    var mps = entry.OriginalValues.Properties.Where(pn => entry.Property(pn.Name).IsModified).ToList();

                    var manager = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager;
                    var myObjectState = manager.GetObjectStateEntry(instrumentoupdate);
                    var modifiedProperties = myObjectState.GetModifiedProperties();
                    foreach (var propName in modifiedProperties)
                    {
                        if (!(modifiedProperties.Count() == 1 && propName == "LastUpdate"))
                        {
                            Modification modification = new Modification(ObjectModification.INSTRUMENT, instrumentoupdate.UniqueId, propName, myObjectState.OriginalValues[propName].ToString(), myObjectState.CurrentValues[propName].ToString());
                            log.InfoFormat("There was a Instrument Update  InstrumentId={1}", instrumentoupdate.UniqueId);
                            InsertModification(modification);


                        }

                    }
                    db.SaveChanges();
                }


            }
        }
        #endregion

        #region Stock
        public void InsertStock(Stock Stock)
        {
            try
            {
                // insert
                using (var db = new DataBaseContext())
                {
                    var dbset = db.Set<Stock>();
                    //modif
                    dbset.Update(Stock);
                    db.SaveChanges();
                    log.InfoFormat("There was an Instrument added Isin={0}, Instrument Type=Stock", Stock.Isin);
                    Modification StockInsertion = new Modification(ObjectModification.INSTRUMENT, Stock.UniqueId, "Insertion Of a New Stock", "", "");
                    var dbm = db.Set<Modification>();
                    dbm.Add(StockInsertion);
                    db.SaveChanges();

                }
            }
            catch (Exception e)
            {

                log.FatalFormat("There Was an issue will trying to insert Instruement in Database, Instrument Type='Stock', Instrument Isin={0}", Stock.Isin);
            }
        }
        #endregion

        #region Derivative
        public void InsertDerivative(Derivative derivative)
        {
            try
            {
                // insert
                using (var db = new DataBaseContext())
                {
                    var dbset = db.Set<Derivative>();
                    //ugly fix but no other way
                    var instrument = db.InstrumentTable.Find(derivative.Underlying.UniqueId);
                    if (instrument != null)
                    {
                        derivative.Underlying = instrument;
                        //modif
                        dbset.Update(derivative);
                        db.SaveChanges();
                        log.InfoFormat("There was an Instrument added Isin={0}, Instrument Type=Derivative", derivative.Isin);
                        Modification StockInsertion = new Modification(ObjectModification.INSTRUMENT, derivative.UniqueId, "Insertion Of a New Derivative", "", "");
                        var dbm = db.Set<Modification>();
                        dbm.Add(StockInsertion);
                        db.SaveChanges();
                    }


                }
            }
            catch (Exception e)
            {

                log.FatalFormat("There Was an issue will trying to insert Instruement in Database, Instrument Type='Derivative', Instrument Isin={0}", derivative.Isin);
            }
        }
        #endregion

        #region Listing
        public void InsertListing(Listing listing)
        {
            // insert

            using (var db = new DataBaseContext())
            {

                if (listing.Instrument is Stock)
                {

                    Stock instrument = db.StockTable.Include(p=>p.Listings).Where(p=>p.UniqueId==listing.Instrument.UniqueId).FirstOrDefault();
                    if (instrument != null)
                    {
                        listing.Instrument = instrument;
                        db.ListingTable.Add(listing);
                        instrument.Listings.Add(listing);
                        db.StockTable.Update(instrument);
                        db.SaveChanges();
                        log.InfoFormat("There was a Listing added for  Stock Isin={0}, Listing id={1}", instrument.Isin, listing.UniqueId);
                        Modification ListingInsertion = new Modification(ObjectModification.INSTRUMENT, listing.UniqueId, "Insertion Of a New Listing", "", "");
                        var dbm = db.Set<Modification>();
                        dbm.Add(ListingInsertion);
                        db.SaveChanges();
                    }
                }
                else if (listing.Instrument is BasketPrice)
                {
                    BasketPrice instrument = db.BasketPriceTable.Find(listing.Instrument.UniqueId);
                    if (instrument != null) {
                        listing.Instrument = instrument;
                        db.ListingTable.Add(listing);
                        instrument.Listing = listing;
                        db.BasketPriceTable.Update(instrument);
                        db.SaveChanges();
                        log.InfoFormat("There was a Listing added for  BacketpPrice Isin={0}, Listing id={1}", listing.Instrument.Isin, listing.UniqueId);
                        Modification ListingInsertion = new Modification(ObjectModification.INSTRUMENT, listing.UniqueId, "Insertion Of a New Lsting", "", "");
                        var dbm = db.Set<Modification>();
                        dbm.Add(ListingInsertion);
                        db.SaveChanges();
                    }

                }
                else if (listing.Instrument is Derivative)
                {

                    Derivative instrument = db.DerivativeTable.Find(listing.Instrument.UniqueId);
                    if (instrument != null)
                    {
                        listing.Instrument = instrument;
                        db.ListingTable.Add(listing);
                        instrument.Listing = listing;
                        db.DerivativeTable.Update(instrument);
                        db.SaveChanges();
                        log.InfoFormat("There was a Listing added for  Derivative Isin={0}, Listing id={1}", instrument.Isin, listing.UniqueId);
                        Modification ListingInsertion = new Modification(ObjectModification.INSTRUMENT, listing.UniqueId, "Insertion Of a New Listing", "", "");
                        var dbm = db.Set<Modification>();
                        dbm.Add(ListingInsertion);
                        db.SaveChanges();
                    }
                }
            }
        }
        #endregion

        #region Price
        public void InsertPrice(Price Price)
        {
            using (var db = new DataBaseContext())
            {
                //modif
                var Listing = db.ListingTable.Find(Price.Listing.UniqueId);
                if (Listing != null)
                {
                    //Ugly no other way
                    Price.Listing = Listing;
                    Price.ListingId = Listing.UniqueId;
                    var dbset = db.Set<Price>();
                    //modif
                    dbset.Update(Price);
                    db.SaveChanges();
                    log.InfoFormat("There was a Price Which was added  For Listing UniqueId={0}", Listing.UniqueId);
                    //Not Log on Database on Price Insertion, to heavy
                }
                else
                {
                    log.FatalFormat("Trying To Insert a Price With a Listing which not exist, Listing ticker={0}", Price.Listing.BloombergCode.ToString());
                }




            }
        }
        #endregion

        #region Dividend
        public void InsertDividend(Dividend dividend)
        {
            using (var db = new DataBaseContext())
            {
                //mdofi
                Stock instrument = db.StockTable.Include(p=>p.Dividends).Where(p=>p.UniqueId==dividend.Instrument.UniqueId).FirstOrDefault();
                if (instrument != null)
                {
                    instrument.Dividends.Add(dividend);
                    db.StockTable.Update(instrument);
                    db.Entry(dividend).State= EntityState.Added;
                    db.SaveChanges();
                    log.InfoFormat("There was a dividend added for  Instrument Isin={0}, dividend id={1}", instrument.Isin, dividend.UniqueId);
                    Modification DividendInsertion = new Modification(ObjectModification.DIVIDEND, dividend.UniqueId, "Insertion Of a New Dividend", "", "");
                    var dbm = db.Set<Modification>();
                    dbm.Add(DividendInsertion);
                    db.SaveChanges();
                }

            }
        }

        public void InsertDividends(List<Dividend> dividends)
        {
            using (var db = new DataBaseContext())
            {
                foreach(var dividend in dividends) {
                    //mdofi
                    try { 
                        Stock instrument = db.StockTable.Include(p => p.Dividends).Where(p => p.UniqueId == dividend.Instrument.UniqueId).FirstOrDefault();
                        if (instrument != null)
                        {
                            instrument.Dividends.Add(dividend);
                            db.StockTable.Update(instrument);
                            db.Entry(dividend).State = EntityState.Added;
                            db.SaveChanges();
                            log.InfoFormat("There was a dividend added for  Instrument Isin={0}, dividend id={1}", instrument.Isin, dividend.UniqueId);
                            Modification DividendInsertion = new Modification(ObjectModification.DIVIDEND, dividend.UniqueId, "Insertion Of a New Dividend", "", "");
                            var dbm = db.Set<Modification>();
                            dbm.Add(DividendInsertion);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        log.FatalFormat("Issue When Trying to insert a dividend, Dividend MarkitId=", dividend.MarkitId.ToString());
                        log.FatalFormat(e.ToString());
                    }
                }
            }
        }

        public void InsertDividendError(DividendError dividenderror)
        {
            try
            {
                // insert
                using (var db = new DataBaseContext())
                {
                    var dbset = db.Set<DividendError>();
                    dbset.Add(dividenderror);
                    db.SaveChanges();
                    log.InfoFormat("There was an Dividend Error added MarkitId={0}", dividenderror.MarkitId);

                }
            }
            catch (Exception e)
            {

                log.FatalFormat("There Was an issue will trying to insert a Dividend Error in Database,  MarkitId={0}", dividenderror.MarkitId);
                log.FatalFormat(e.ToString());
            }
        }
        public void UpdateDividend(Dividend dividend)
        {
            using (var db = new DataBaseContext())
            {
                var dividendtoupdate = db.DividendTable.Where(p => p.MarkitId == dividend.MarkitId).FirstOrDefault();
                //modif
                dividendtoupdate = db.DividendTable.Find(dividendtoupdate.UniqueId);
                if (dividendtoupdate != null)
                {
                    foreach (PropertyInfo propertyInfo in dividendtoupdate.GetType().GetProperties())
                    {
                        if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Modifications" && propertyInfo.Name != "Instrument")
                        {
                            if (!Equals(propertyInfo.GetValue(dividendtoupdate), dividend.GetType().GetProperty(propertyInfo.Name).GetValue(dividend)))
                            {
                                propertyInfo.SetValue(dividendtoupdate, dividend.GetType().GetProperty(propertyInfo.Name).GetValue(dividend));

                            }
                        }
                    }

                    var changes = db.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
                    var properties = changes.Select(p => p.Properties);
                    var modifiedProperties = properties.Select(x => x.Where(p => p.IsModified == true)).ToList();

                    foreach (var modifiedproperty in modifiedProperties)
                    {
                        string propName = modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).FirstOrDefault().ToString();
                        string OldValue = modifiedproperty.Select(p => p.OriginalValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.OriginalValue).FirstOrDefault().ToString();
                        string NewValue = modifiedproperty.Select(p => p.CurrentValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.CurrentValue).FirstOrDefault().ToString();
                        if (!(propName == "LastUpdate"))
                        {
                            Modification modification = new Modification(ObjectModification.DIVIDEND, dividendtoupdate.UniqueId, propName, OldValue, NewValue);
                            log.InfoFormat("There was a Dividend Update  DividendId={0}", dividendtoupdate.UniqueId);
                            InsertModification(modification);
                        }
                    }
                    db.DividendTable.Update(dividendtoupdate);
                    db.SaveChanges();
                }
            }
        }
        public void UpdateDividends(List<Dividend> dividends)
        {
            using (var db = new DataBaseContext())
            {
                var dbset = db.Set<Modification>();
                foreach (var dividend in dividends)
                {
                    try { 
                        var dividendtoupdate = db.DividendTable.Where(p => p.MarkitId == dividend.MarkitId).FirstOrDefault();
                        if (dividendtoupdate != null)
                        {
                            foreach (PropertyInfo propertyInfo in dividendtoupdate.GetType().GetProperties())
                            {
                                if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Modifications" && propertyInfo.Name != "Instrument")
                                    if (!Equals(propertyInfo.GetValue(dividendtoupdate), dividend.GetType().GetProperty(propertyInfo.Name).GetValue(dividend)))
                                    {
                                        propertyInfo.SetValue(dividendtoupdate, dividend.GetType().GetProperty(propertyInfo.Name).GetValue(dividend));
                                    }
                                }
                            }

                            var changes = db.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified && x.Entity==dividendtoupdate);
                            var properties = changes.Select(p => p.Properties);
                            var modifiedProperties = properties.Select(x => x.Where(p => p.IsModified == true)).ToList();

                            foreach (var modifiedproperty in modifiedProperties)
                            {
                                string propName = modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).FirstOrDefault().ToString();
                                string OldValue = modifiedproperty.Select(p => p.OriginalValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.OriginalValue).FirstOrDefault().ToString();
                                string NewValue = modifiedproperty.Select(p => p.CurrentValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.CurrentValue).FirstOrDefault().ToString();
                                if (!(propName == "LastUpdate"))
                                {
                                    Modification modification = new Modification(ObjectModification.DIVIDEND, dividendtoupdate.UniqueId, propName, OldValue, NewValue);
                                    log.InfoFormat("There was a Dividend Update  DividendId={0}", dividendtoupdate.UniqueId);
                                    InsertModification(modification);
                            }
                            db.DividendTable.Update(dividendtoupdate);

                        }
                    }
                    catch (Exception e) {
                        log.FatalFormat("Issue When Trying to update a dividend, Dividend MarkitId=", dividend.MarkitId.ToString());
                        log.FatalFormat(e.ToString());
                     }   
                }
                db.SaveChanges();
            }
        }
        #endregion

        #region Basketprice
        public void InsertBasketPrice(BasketPrice basketPrice)
        {
            using (var db = new DataBaseContext())
            {
                //Modif
                db.BasketPriceTable.Update(basketPrice);
                db.SaveChanges();
                log.InfoFormat("There was a BasketPrice Which was added UniqueId={0}", basketPrice.UniqueId);
                Modification BasketPriceInsertion = new Modification(ObjectModification.BASKETPRICE, basketPrice.UniqueId, "Insertion Of A New Basket Price", "", "");
                var dbm = db.Set<Modification>();
                dbm.Add(BasketPriceInsertion);
                db.SaveChanges();


            }
        }
        public void InsertBasketPriceComposition(BasketPriceComposition basketPriceComposition)
        {
            using (var db = new DataBaseContext())
            {
                //modif
                var basketprice = db.BasketPriceTable.Find(basketPriceComposition.BasketPrice.UniqueId);
                if (basketprice != null)
                {
                    //Ugly no other way
                    basketPriceComposition.BasketPrice = basketprice;
                    basketPriceComposition.BasketPriceId = basketprice.UniqueId;
                    if (basketprice.PriceComposition == null)
                    {
                        basketprice.PriceComposition = new List<BasketPriceComposition>();
                    }
                    basketprice.PriceComposition.Add(basketPriceComposition);
                    db.SaveChanges();
                    log.InfoFormat("There was a BasketPriceComposition Which was added  For BasketPrice UniqueId={0}", basketprice.UniqueId);
                    Modification BasketPriceCompositionInsertion = new Modification(ObjectModification.COMPOSITION, basketPriceComposition.UniqueId, "Insertion Of a New BasketPrice Composition", "", "");
                    var dbm = db.Set<Modification>();
                    dbm.Add(BasketPriceCompositionInsertion);
                    db.SaveChanges();
                }




            }
        }
        public void InsertBasketPriceCompoment(BasketPriceComponent basketPriceComponent)
        {
            using (var db = new DataBaseContext())
            {
                var basketprice = db.BasketPriceTable.Include(p=>p.PriceComposition).ThenInclude(p=>p.Composition).Where(p=>p.UniqueId==basketPriceComponent.BasketPriceComposition.BasketPrice.UniqueId).FirstOrDefault();
                if (basketprice != null)
                {
                    var basketpricecomposition = db.BasketPriceCompositionTable.Where(p => p.UniqueId == basketPriceComponent.BasketPriceComposition.UniqueId).FirstOrDefault();
                    if (basketpricecomposition != null)
                    {
                        var instrument = db.InstrumentTable.Find(basketPriceComponent.Instrument.UniqueId);

                        if (instrument != null)
                        {
                            //Ugly but no other way
                            basketPriceComponent.Instrument = instrument;
                            basketPriceComponent.InstrumentId = instrument.UniqueId;
                            //
                            basketpricecomposition.Composition.Add(basketPriceComponent);
                            db.BasketPriceCompositionTable.Update(basketpricecomposition);
                            db.Entry(basketPriceComponent).State = EntityState.Added;
                            //db.BasketPriceTable.Update(basketprice);
                            db.SaveChanges();
                            log.InfoFormat("There was a BasketPriceComponent Which was added  For BasketPriceComposition UniqueId={0}", basketpricecomposition.UniqueId);
                        }
                        else
                        {
                            log.FatalFormat("There was an issue : Trying to Insert a BasketPriceComponent where BasketpriceCompnent does not exist in db, BasketPriceComponent Isin={0}", basketPriceComponent.Instrument.Isin);
                        }
                    }
                    else
                    {
                        log.FatalFormat("There was an issue : Trying to Insert a BasketPrice Component where BasketpriceComposition does not exist in db, Instrument Isin={0}", basketprice.Isin);
                    }

                }
                else
                {
                    log.FatalFormat("There was an issue : Trying to Insert a BasketPrice Component where Instrument does not exist in db, Instrument Isin={0}", basketPriceComponent.Instrument.Isin);
                }

            }

        }

        public void InsertBasketPriceCompoments(List<BasketPriceComponent> basketPriceComponents)
        {
            using (var db = new DataBaseContext())
            {
                var basketprice = db.BasketPriceTable.Include(p => p.PriceComposition).ThenInclude(p => p.Composition).Where(p => p.UniqueId == basketPriceComponents.FirstOrDefault().BasketPriceComposition.BasketPrice.UniqueId).FirstOrDefault();
                if (basketprice != null)
                {
                    var basketpricecomposition = basketprice.PriceComposition.Where(p => p.UniqueId == basketPriceComponents.FirstOrDefault().BasketPriceComposition.UniqueId).FirstOrDefault();
                    if (basketpricecomposition != null)
                    {
                        foreach (var basketPriceComponent in basketPriceComponents)
                        {
                            var instrument = db.InstrumentTable.Find(basketPriceComponent.Instrument.UniqueId);
                            if (instrument != null)
                            {
                                //Ugly but no other way
                                basketPriceComponent.Instrument = instrument;
                                basketPriceComponent.InstrumentId = instrument.UniqueId;
                                db.Entry(basketPriceComponent).State = EntityState.Added;
                                basketpricecomposition.Composition.Add(basketPriceComponent);
                                log.InfoFormat("There was a BasketPriceComponent Which was added  For BasketPriceComposition UniqueId={0}", basketpricecomposition.UniqueId);
                            }
                            else
                            {
                                log.FatalFormat("There was an issue : Trying to Insert a BasketPriceComponent where BasketpriceCompnent does not exist in db, BasketPriceComponent Isin={0}", basketPriceComponent.Instrument.Isin);
                            }
                        }
                        db.BasketPriceCompositionTable.Update(basketpricecomposition);
                        db.SaveChanges();
                    }
                    else
                    {
                        log.FatalFormat("There was an issue : Trying to Insert a BasketPrice Component where BasketpriceComposition does not exist in db, Instrument Isin={0}", basketprice.Isin);
                    }

                }
                else
                {
                    log.FatalFormat("There was an issue : Trying to Insert a BasketPrice Components");
                }

            }

        }
        //ToCheck
        public void UpdpateBasketPrice(BasketPrice basketPrice)
        {
            using (var db = new DataBaseContext())
            {
                var basketpricetoupdate = db.BasketPriceTable.Where(p => p.Isin == basketPrice.Isin).FirstOrDefault();
                //modif
                basketpricetoupdate = db.BasketPriceTable.Find(basketpricetoupdate.UniqueId);
                if (basketpricetoupdate != null)
                {
                    foreach (PropertyInfo propertyInfo in basketpricetoupdate.GetType().GetProperties())
                    {
                        if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Modifications")
                        {
                            if (!Equals(propertyInfo.GetValue(basketpricetoupdate), basketPrice.GetType().GetProperty(propertyInfo.Name).GetValue(basketPrice)))
                            {
                                propertyInfo.SetValue(basketpricetoupdate, basketPrice.GetType().GetProperty(propertyInfo.Name).GetValue(basketPrice));

                            }
                        }
                    }

                    var changes = db.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
                    var properties = changes.Select(p => p.Properties);
                    var modifiedProperties = properties.Select(x => x.Where(p => p.IsModified == true)).ToList();

                    foreach (var modifiedproperty in modifiedProperties)
                    {
                        string propName = modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).FirstOrDefault().ToString();
                        string OldValue = modifiedproperty.Select(p => p.OriginalValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.OriginalValue).FirstOrDefault().ToString();
                        string NewValue = modifiedproperty.Select(p => p.CurrentValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.CurrentValue).FirstOrDefault().ToString();
                        if (!(modifiedProperties.Count() == 1 && propName == "LastUpdate"))
                        {
                            Modification modification = new Modification(ObjectModification.INSTRUMENT, basketpricetoupdate.UniqueId, propName, OldValue, NewValue);
                            log.InfoFormat("There was a BaksetPrice Update  BasketPriceId={1}", basketpricetoupdate.UniqueId);
                            InsertModification(modification);
                        }
                    }
                    db.BasketPriceTable.Update(basketpricetoupdate);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateBasketPriceComposition(BasketPriceComposition BasketPriceComposition)
        {
            using (var db = new DataBaseContext())
            {
                var basketpricecompositiontoupdate = db.BasketPriceCompositionTable.Where(p => p.UniqueId == BasketPriceComposition.UniqueId).FirstOrDefault();
                if (basketpricecompositiontoupdate != null)
                {
                    foreach (PropertyInfo propertyInfo in basketpricecompositiontoupdate.GetType().GetProperties())
                    {
                        if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Modifications" && propertyInfo.Name != "BasketPrice" && propertyInfo.Name != "Composition")
                        {
                            if (!Equals(propertyInfo.GetValue(basketpricecompositiontoupdate), BasketPriceComposition.GetType().GetProperty(propertyInfo.Name).GetValue(BasketPriceComposition)))
                            {
                                propertyInfo.SetValue(basketpricecompositiontoupdate, BasketPriceComposition.GetType().GetProperty(propertyInfo.Name).GetValue(BasketPriceComposition));
                            }
                        }
                    }

                    var changes = db.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
                    var properties = changes.Select(p => p.Properties);
                    var modifiedProperties = properties.Select(x => x.Where(p => p.IsModified == true)).ToList();

                    foreach (var modifiedproperty in modifiedProperties)
                    {

                        string propName = modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).FirstOrDefault().ToString();
                        string OldValue = modifiedproperty.Select(p => p.OriginalValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.OriginalValue).FirstOrDefault().ToString();
                        string NewValue = modifiedproperty.Select(p => p.CurrentValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.CurrentValue).FirstOrDefault().ToString();
                        if (!(modifiedProperties.Count() == 1 && propName == "LastUpdate"))
                        {
                            Modification modification = new Modification(ObjectModification.COMPOSITION, basketpricecompositiontoupdate.UniqueId, propName, OldValue, NewValue);
                            log.InfoFormat("There was a BaksetPriceComposition Update  BasketPriceId={0}", basketpricecompositiontoupdate.BasketPriceId);
                            InsertModification(modification);
                        }
                    }
                    db.BasketPriceCompositionTable.Update(basketpricecompositiontoupdate);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region Modification
        public void InsertModification(Modification modification)
        {
            using (var db = new DataBaseContext())
            {
                var dbset = db.Set<Modification>();

                dbset.Add(modification);
                db.SaveChanges();
                log.InfoFormat("There was a modification inserted Id={0}", modification.UniqueId);

            }
        }
        #endregion

        #region EarningEstimate
        public void InsertOrUpdateEarningEstimate(EarningEstimate EarningEstimate)
        {
            using (var db = new DataBaseContext())
            {
                var Earningtoupdate = db.EarningEstimateTable.Where(p => p.UniqueId == EarningEstimate.UniqueId).FirstOrDefault();
                //Update
                if (Earningtoupdate != null)
                {
                    Earningtoupdate = db.EarningEstimateTable.Find(Earningtoupdate.UniqueId);
                    foreach (PropertyInfo propertyInfo in Earningtoupdate.GetType().GetProperties())
                    {
                        if (propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Stock")
                        {
                            if (!Equals(propertyInfo.GetValue(Earningtoupdate), EarningEstimate.GetType().GetProperty(propertyInfo.Name).GetValue(EarningEstimate)))
                            {
                                propertyInfo.SetValue(Earningtoupdate, EarningEstimate.GetType().GetProperty(propertyInfo.Name).GetValue(EarningEstimate));
                            }
                        }
                    }

                    var changes = db.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
                    var properties = changes.Select(p => p.Properties);
                    var modifiedProperties = properties.Select(x => x.Where(p => p.IsModified == true)).ToList();

                    foreach (var modifiedproperty in modifiedProperties)
                    {
                        string propName = modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.Metadata.PropertyInfo.Name).FirstOrDefault().ToString();
                        string OldValue = modifiedproperty.Select(p => p.OriginalValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.OriginalValue).FirstOrDefault().ToString();
                        string NewValue = modifiedproperty.Select(p => p.CurrentValue).DefaultIfEmpty("").First() == null ? string.Empty : modifiedproperty.Select(p => p.CurrentValue).FirstOrDefault().ToString();
                        if (propName != "UniqueId" && propName != "Stock" && !(propName == "Last_Update"))
                        {
                            Modification modification = new Modification(ObjectModification.EARNINGESTIMATE, Earningtoupdate.UniqueId, propName, OldValue, NewValue);
                            log.InfoFormat("There was a EarningEstiate Update  Stock={0}, FieldUpdate={1}", Earningtoupdate.UniqueId, propName);
                            InsertModification(modification);
                        }
                    }
                    db.EarningEstimateTable.Update(Earningtoupdate);
                    db.SaveChanges();
                }
                else
                {
                    //modif
                    var Stock = db.StockTable.Find(EarningEstimate.Stock.UniqueId);
                    if (Stock != null)
                    {
                        EarningEstimate.Stock = Stock;
                        EarningEstimate.UniqueId = Stock.UniqueId;
                        db.EarningEstimateTable.Add(EarningEstimate);       
                        db.SaveChanges();
                        log.InfoFormat("There was a EarningEstimate Which was added  For Stock UniqueId={0}", EarningEstimate.UniqueId);
                        //
                        foreach (PropertyInfo propertyInfo in EarningEstimate.GetType().GetProperties())
                        {
                            if (EarningEstimate.GetType().GetProperty(propertyInfo.Name).GetValue(EarningEstimate) != null && propertyInfo.Name != "UniqueId" && propertyInfo.Name != "Stock" && !(propertyInfo.Name == "Last_Update"))
                            {
                                Modification modification = new Modification(ObjectModification.EARNINGESTIMATE, EarningEstimate.UniqueId, propertyInfo.Name, "Insertion", EarningEstimate.GetType().GetProperty(propertyInfo.Name).GetValue(EarningEstimate).ToString());
                                InsertModification(modification);
                            }
                        }
                    }
                    else
                    {
                        log.FatalFormat("Trying To Insert an EarningEstimate With a Stock which not exist, Stock Isin={0}", EarningEstimate.Stock.Isin.ToString());
                    }
                }
            }
        }
        #endregion
    }
}
