using System;

namespace Uozu.Utils.Test.Manual
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlMapperSpeedTest.Run();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
