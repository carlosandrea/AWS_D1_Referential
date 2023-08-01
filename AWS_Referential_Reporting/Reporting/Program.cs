using System;
using System.Collections.Generic;

namespace Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var Report = new Report();
            List<string> Receivers = new List<string>();
            Receivers.Add("an email");

            //Report.GenerateAndSendReferentialReport(DateTime.Today);
            ///
            DateTime EndDate = new DateTime(2023, 12, 15);
            DateTime StartDate= new DateTime(2022,12,16);
            //Report.GenerateIndexDividendModificationReport("SX5E INDEX", DateTime.Today,EndDate, DateTime.Today.AddDays(-3),true);
            //Report.GenerateIndexDividendReport("SX5E INDEX", DateTime.Today, EndDate,true);
            //Report.GenerateIndexCompositionReport("SPX INDEX", DateTime.Today,true);
            Report.GenerateIndexCompositionReport("UKX Index", DateTime.Today, true);
            //Report.GenerateIndexDividendModificationReport("SX5E Index", DateTime.Today, EndDate, DateTime.Today.AddDays(-10), true);
            //Report.GenerateIndexEarningModificationReport("SPX Index", DateTime.Today, Receivers, true);
            var toto = 2;
        }
    }
}

