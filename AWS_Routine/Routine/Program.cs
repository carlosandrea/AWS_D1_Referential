using System;
using System.Threading.Tasks;

namespace Routine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Daily DailyRoutine = new Daily();
            Task Daily=DailyRoutine.Run();
            await Task.WhenAll(Daily);
        }
    }
}
