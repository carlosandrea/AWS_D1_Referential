using AWS_Referential.DataBase;
using System;
using System.Linq;

namespace AWS_Referential
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AWS_Referential.Query.Query query = new AWS_Referential.Query.Query();
            query.GetBasketPricesById(1391);
        }
    }
}
