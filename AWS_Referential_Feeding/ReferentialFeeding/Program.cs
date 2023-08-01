using System;
using System.Threading.Tasks;

namespace ReferentialFeeding
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //InsertUpdate Dividend
            //FeedingFromMarkit feeding = new FeedingFromMarkit();
            //Task feed = feeding.InsertAllDividends();
            //await Task.WhenAll(feed);
            //
            //
            //ManualFeeding Mano = new ManualFeeding();
            //Mano.ManageObject();

            //Insert BasketPrice
            //{"228942",607529 },
            //FeedingFromMarkit feeding = new FeedingFromMarkit();
            //long ListingId = 79550;
            //Task feed = feeding.InsertComposition("19845", ListingId);
            //await Task.WhenAll(feed);

            FeedingFromBloomberg FeedFromBBG = new FeedingFromBloomberg();
            FeedFromBBG.UpDateDerivativePrice();
            //FeedFromBBG.UpdateEarningEstimateByIndex("SPX Index");
            //FeedFromBBG.GetAllDividendsEstimateFromBloomberg();

            //add DerivativeInstrument
            //string csvPath = @"C:\Dev\ReferentialFeeding\ManualFeeding_SpreadSheet\InsertDerivative.csv";
            //ManualFeeding InsertionOfDerivative = new ManualFeeding();
            //InsertionOfDerivative.InsertDerivativesFromCSV(csvPath);

            //Add Broker Price
            //string csvPath = @"C:\Dev\ReferentialFeeding\ManualFeeding_SpreadSheet\InsertBrokerPrice.csv";
            //ManualFeeding InsertionOfDerivative = new ManualFeeding();
            //InsertionOfDerivative.InsertBrokerPriceFromCSV(csvPath);

            //Add CrossPrice Manual
            //string csvPath = @"C:\Dev\ReferentialFeeding\ManualFeeding_SpreadSheet\InsertCrossPrice.csv";
            //ManualFeeding InsertionOfDerivative = new ManualFeeding();
            //InsertionOfDerivative.InsertCrossPriceFromCSV(csvPath);

            //Add CrossPrice Auto
            //FeedingFromBloomberg fee = new FeedingFromBloomberg();
            //fee.UpDateDerivativePrice();


            //var tot = 2;
        }
    }
}
