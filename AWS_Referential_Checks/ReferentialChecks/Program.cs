using System;

namespace ReferentialChecks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IndexChecker test = new IndexChecker();
            test.CheckAllIndexesLast();
            //DividendChecker test = new DividendChecker();
            //var tetsttt=test.CheckPossibleDuplicateDividends();
            var toto = 3;
        }
    }
}
