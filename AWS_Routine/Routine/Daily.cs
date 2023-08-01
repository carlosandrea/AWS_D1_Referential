using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReferentialFeeding;
using Reporting;

namespace Routine
{
    public class Daily
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //There is nowhere shere we stock the markit Id of the Index
        //private List<string> _AllCompoToUdateId = new List<string>() { "158780", "35406","303696","297074","286514","228942","355979","348389" };
        private Dictionary<string, long> _AllCompoToUdate = new Dictionary<string, long>()
        {
            //{"297074",887871 },
            {"567039",2477303 }, // CAC60
            {"158780",392630 }, // SX5E
            {"35406",98677 },// CAC
            {"303696",895457 }, // DAX
            //{"297074",887871 }, // FTSEMIB
            {"286514",865405 }, // AEX
            {"228942",607529 }, // UKX
            {"355979",1770946 }, // IBEX
            {"348389",1378437 }, // SMI
            {"374608",1893573 }, // SXXP
            {"161887",399575 }, // SX7E
            {"402501",1970989 }, // SD3E
            {"19845",79550 } // SPX

        };

        private List<String> IndexDividendModificationReport = new List<string>()
        {
            "SX5E Index",
        };


        private Dictionary<string, List<string>> IndexEarningModificationReportAndUSer = new Dictionary<string, List<string>>()
        {
           {"SPX Index", new List<string> { "receiver_1", "receiver_2" } },


        };

        private Dictionary<string, List<string>> IndexDividendsModificationReportAndUSer = new Dictionary<string, List<string>>()
        {
           {"SX5E Index", new List<string> { "receiver_1", "receiver_2" } },

        };

        private int GetWorkingDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                return -3;

            }
            else
            {
                return -1;
            }
        }

        private DateTime EndDate = new DateTime(2024, 12, 20);

        public async Task Run()
        {
            FeedingFromMarkit feeding = new FeedingFromMarkit();
            Console.WriteLine("Updating Index");
            foreach (var Compo in _AllCompoToUdate)
            {
                try
                {

                    switch (Compo.Key)
                    {
                        case "567039":
                            Task feed = feeding.InsertCompositionShittyIndices(Compo.Key, Compo.Value);
                            await Task.WhenAll(feed);
                            break;
                        default:
                            Task feed1 = feeding.InsertComposition(Compo.Key, Compo.Value);
                            await Task.WhenAll(feed1);
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    log.FatalFormat("Issue when trying to Update the compositon for Index={0}", Compo.Key);
                }

            }
            Console.WriteLine("Updating Dividend From Markit");
            try
            {
                Task feedsdiv = feeding.InsertAllDividends();
                await Task.WhenAll(feedsdiv);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                log.FatalFormat("Issue when trying to update dividend from markit");
            }
            


            FeedingFromBloomberg FeedFromBBG = new FeedingFromBloomberg();


            foreach (var IndexEarningReport in IndexEarningModificationReportAndUSer)
            {
                try
                {
                    FeedFromBBG.UpdateEarningEstimateByIndex(IndexEarningReport.Key);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    log.FatalFormat("Issue when trying to Update Earning Report for Index={0}", IndexEarningReport.Key);
                }
                
            }


            Console.WriteLine("Updating Cross Price");
            try
            {
                FeedFromBBG.UpDateDerivativePrice();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                log.FatalFormat("Issue when trying to Update Cross Price");
            }
            Console.WriteLine("Generating Report");
            var Report = new Report();
            try
            {
                Report.GenerateAndSendReferentialReport(DateTime.Today);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                log.FatalFormat("Issue when trying to Generate Referential Report");
            }
            

            Console.WriteLine("Generating Index Dividends Report");
            foreach (var IndexDivReport in IndexDividendsModificationReportAndUSer)
            {
                try
                {
                    Report.GenerateIndexDividendModificationReport(IndexDivReport.Key, DateTime.Today, EndDate, DateTime.Today, IndexDividendsModificationReportAndUSer[IndexDivReport.Key], true);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    log.FatalFormat("Issue when trying to Generate Index Dividend Modification for Index={0}", IndexDivReport.Key);
                }
                
            }

            Console.WriteLine("Generating Index Earnings Report");
            foreach (var IndexEarningReport in IndexEarningModificationReportAndUSer)
            {
                try
                {
                    Report.GenerateIndexEarningModificationReport(IndexEarningReport.Key, DateTime.Today, IndexEarningModificationReportAndUSer[IndexEarningReport.Key], true);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    log.FatalFormat("Issue when trying to Generate Index Earning Modification Report for Index={0}", IndexEarningReport.Key);
                }
                
            }


            Console.WriteLine("All Done");

        }
    }
}
