using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AWS_Referential.Implementation;
using AWS_Referential.Management;
using AWS_Referential.Query;

namespace ReferentialFeeding
{
    public class ManualFeeding
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ManageObject()
        {
            Query query = new Query();
            Management DBM = new Management();
            var div = query.GetDividendByMarkitId("55835415744-98428");
            div.OtherEstimates = "BNP : 1,1 /JPM : 0 /BBG : ExDate= 24.05.2021 Amount= 2,75";    
            DBM.UpdateDividend(div);

        }

        public void InsertDerivativesFromCSV(string csvpath)
        {
            Query query = new Query();
            Management DBM = new Management();
            using (var reader = new StreamReader(csvpath))
            {
                reader.ReadLine();
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    //Verify  if derivative exist already
                    string IsinDerivative = values[2];
                    var DerivativeInstrument=query.GetInstrumentByIsin(IsinDerivative);
                    if (DerivativeInstrument == null)
                    {
                        //Check if Derivative Listing already Exist
                        var BBGCode = values[0];
                        var DerivativeListing = query.GetListingByBBG(BBGCode);
                        if (DerivativeListing == null)
                        {
                            string UnderlyingIsin = values[15];
                            var DerivativeUnderlying = query.GetInstrumentByIsin(UnderlyingIsin);
                            if (DerivativeUnderlying == null)
                            {
                                log.FatalFormat("Trying To insert a Derivative with a Underlying that doens't Exist : Underlying Isin={0}", UnderlyingIsin);
                            }
                            else
                            {
                                try { 
                                    string Name = values[1];
                                    string Isin=values[2];
                                    string FIGI = values[3];
                                    string Cusip= values[4];
                                    Derivative DerivativeToInsert = new Derivative(Name,Isin, FIGI,Cusip);

                                    //listing
                                    string Ric = values[5];
                                    string Mic = values[6];
                                    string PrimaryMic = values[7];
                                    string ListingBBGCode = values[8];
                                    string Sedol = values[9];
                                    string ListingCusip = values[10];
                                    string ListingName = values[11];
                                    string Currency = values[12];
                                    decimal Multiplier = Convert.ToDecimal( values[13]);

                                    AWS_Referential.Enumeration.CurrencyCode currencycode = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), Currency.ToString().ToUpper());
           

                                    string SDerivativeType = values[16];
                                    DateTime ExpirationbDate = Convert.ToDateTime(values[17]);
                                    double PV = Convert.ToDouble(values[18]);

                                    AWS_Referential.Enumeration.DerivativeType DerivativeType = (AWS_Referential.Enumeration.DerivativeType)Enum.Parse(typeof(AWS_Referential.Enumeration.DerivativeType), SDerivativeType.ToString().ToUpper());
                                    DerivativeToInsert.DerivativeType = DerivativeType;
                                    DerivativeToInsert.ExpirationDate = ExpirationbDate;
                                    DerivativeToInsert.PointValue = PV;
                                    DerivativeToInsert.Underlying = DerivativeUnderlying;
                                    DBM.InsertDerivative(DerivativeToInsert);


                                   

                                    Listing DerivativeToInsertListing = new Listing(Ric, Mic, PrimaryMic, ListingBBGCode, Sedol, ListingCusip, ListingName, currencycode, Multiplier);
                                    DerivativeToInsertListing.Instrument = DerivativeToInsert;
                                    DBM.InsertListing(DerivativeToInsertListing);
                                }
                                catch(Exception e)
                                {
                                    log.FatalFormat("Error While Insrerting a new Derivative ={0}", e.ToString());
                                }

                            }
                        }
                        else
                        {
                            log.FatalFormat("Trying To insert a Derivative that already Exist : Isin={0}", IsinDerivative);

                        }
                    }
                    else
                    {
                        //Check if Derivative Listing already Exist
                        var BBGCode = values[0];
                        var DerivativeListing = query.GetListingByBBG(BBGCode);
                        if (DerivativeListing == null)
                        {
                            try
                            {
                                //listing
                                string Ric = values[5];
                                string Mic = values[6];
                                string PrimaryMic = values[7];
                                string ListingBBGCode = values[8];
                                string Sedol = values[9];
                                string ListingCusip = values[10];
                                string ListingName = values[11];
                                string Currency = values[12];
                                decimal Multiplier = Convert.ToDecimal(values[13]);

                                AWS_Referential.Enumeration.CurrencyCode currencycode = (AWS_Referential.Enumeration.CurrencyCode)Enum.Parse(typeof(AWS_Referential.Enumeration.CurrencyCode), Currency.ToString().ToUpper());
                                Listing ListingToInsert = new Listing(Ric, Mic, PrimaryMic, ListingBBGCode, Sedol, ListingCusip, ListingName, currencycode, Multiplier);

                                string Isin = values[2];
                                Derivative Derivative = query.GetDerivativeByIsin(Isin);
                                if (Derivative != null)
                                {
                                    ListingToInsert.Instrument = Derivative;
                                    DBM.InsertListing(ListingToInsert);
                                }
                                else
                                {
                                    log.FatalFormat("Error While inserting Listing for Derivative={0}", Isin);
                                }
                            }
                            catch (Exception e)
                            {
                                log.FatalFormat("Error While Insertin a Derivative ={0}", e.ToString());
                            }
                        }
                        else
                        {
                            log.FatalFormat("Trying To insert a Derivative that already Exist : Isin={0}", IsinDerivative);
                        }
                    }

                }
            }
        }

        public void InsertBrokerPriceFromCSV(string csvpath)
        {
            Query query = new Query();
            Management DBM = new Management();
            using (var reader = new StreamReader(csvpath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    string Ticker = values[0];
                    var Listing = query.GetListingByBBG(Ticker);
                    if (Listing != null)
                    {
                        try
                        {
                            string Source = values[3];
                            DateTime dateTime = Convert.ToDateTime(values[4]);
                            double BidPrice = Convert.ToDouble(values[1]);
                            double AskPrice = Convert.ToDouble(values[2]);

                            Price BidPriceToInsert = new Price(Listing, Source, AWS_Referential.Enumeration.PriceType.BID, dateTime, BidPrice);
                            Price AskPriceToInsert = new Price(Listing, Source, AWS_Referential.Enumeration.PriceType.ASK, dateTime, AskPrice);

                            DBM.InsertPrice(BidPriceToInsert);
                            DBM.InsertPrice(AskPriceToInsert);

                        }
                        catch
                        {
                            log.FatalFormat("Issue when Trying to Insert a Price Listing Ticker={0}", Ticker);
                        }
                    }
                    else
                    {
                        log.FatalFormat("Trying to Insert a Price with a Listing that doesnt't exist, Listing Ticker={0}", Ticker);
                    }
                }
            }
        }
        public void InsertCrossPriceFromCSV(string csvpath)
        {
            Query query = new Query();
            Management DBM = new Management();
            using (var reader = new StreamReader(csvpath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    string Ticker = values[0];
                    var Listing = query.GetListingByBBG(Ticker);
                    if (Listing != null)
                    {
                        try
                        {
                            string Source = values[3];
                            DateTime dateTime = Convert.ToDateTime(values[4]);
                            double Price = Convert.ToDouble(values[1]);
                            double Volume = Convert.ToDouble(values[2]);

                            Price CrossPriceToInsert = new Price(Listing, Source, AWS_Referential.Enumeration.PriceType.CROSS, dateTime, Price,Volume);
                           
                            DBM.InsertPrice(CrossPriceToInsert);


                        }
                        catch
                        {
                            log.FatalFormat("Issue when Trying to Insert a Price Listing Ticker={0}", Ticker);
                        }
                    }
                    else
                    {
                        log.FatalFormat("Trying to Insert a Price with a Listing that doesnt't exist, Listing Ticker={0}", Ticker);
                    }
                }
            }
        }
    }
}