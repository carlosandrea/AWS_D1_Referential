using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomberg;
using Markit;
using MarkitAvailableCompositions;
using MarkitComposition;
using MarkitDividend;

namespace test1
{
    class Program
    {
        static async Task Main(string[] args)
        {
           SolaRestWrapper solaRestWrapper = new SolaRestWrapper();
           Task<String> additionalProductDetials = solaRestWrapper.GetCompositon("364453");
            await Task.WhenAll(additionalProductDetials);

            //var markitCompositionObject = MarkitCompositionObject.FromJson(additionalProductDetials.Result);
           // //File.WriteAllText(@"U:\charlesandre\Dev\Data Provider\csv\compositionsx5e.json", additionalProductDetials.Result);
           // //MarkitAvailableComposition compo = MarkitAvailableComposition.FromJson(additionalProductDetials.Result);
           // //var toto = dividend.Result.Where(p => p.Bloomberg == "CS FP Equity");
            List<string> bloombergSecurityNames;
            List<string> bloombergFieldNames;
           // List<string> bloombergOverrideFields;
           // List<string> bloombergOverrideValues;
             dynamic[,,] result;
           // //// create securities
           bloombergSecurityNames = new List<string>();
           bloombergSecurityNames.Add("SHL GY Equity");
           // //bloombergSecurityNames.Add("CS FP Equity");

           // ////
           // //// create fields
            bloombergFieldNames = new List<string>();
            bloombergFieldNames.Add("LAST_PRICE");

           // bloombergOverrideFields = new List<string>();
           // bloombergOverrideValues = new List<string>();
           // bloombergOverrideFields.Add("DIR");
           // bloombergOverrideValues.Add("V");

           // bloombergOverrideFields.Add("CondCodes");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("ExchCode");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("IntrRw");
           // bloombergOverrideValues.Add("True");

           // bloombergOverrideFields.Add("QRM");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("BrkrCodes");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("ActionCodes");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("TradeTime");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("RPSCodes");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("IndicatorCodes");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("UpfrontPrice");
           // bloombergOverrideValues.Add("S");

           // bloombergOverrideFields.Add("BICMICCodes");
           // bloombergOverrideValues.Add("S");

           // //bloombergOverrideFields.Add("Dts");
           // //bloombergOverrideValues.Add("S");
           //DateTime start = new DateTime(2020, 11, 10, 00, 00, 00);
           // DateTime end = new DateTime(2020, 11, 11, 23, 59, 00);


           // BBCOMDataRequest wrapper = new IntradayTickRequest(bloombergSecurityNames, start,end);
           // ////
           // //// create wrapper object, retrieve and print data
           // //BBCOMDataRequest wrapper = new BulkDataRequest(bloombergSecurityNames, "DV141");
           // result = wrapper.Process_Full();

           // //"Dir=V"; "CondCodes=S"; "ExchCode=S"; "IntrRw=true"; "QRM=S"; "BrkrCodes=S"; "ActionCodes=S"; "TradeTime=S"; "RPSCodes=S"; "IndicatorCodes=S"; "UpfrontPrice=S"; "BICMICCodes=S"
            DateTime start = new DateTime(2022, 02, 15);
            DateTime end = new DateTime(2022, 02, 15) ;
            BBCOMDataRequest wrapper = new HistoricalDataRequest(bloombergSecurityNames, bloombergFieldNames, start, end, true);
            result = wrapper.Process_Full();
            var titi = 232;

        }
    }
}
