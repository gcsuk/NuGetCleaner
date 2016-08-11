using System;

namespace NuGetCleaner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new AnalysisService();

            do
            {
                service.Start(@"C:\Source\Zeus\src\Orchard.Web\Modules");
                Console.WriteLine("Done. Run Again?");
            } while (Console.ReadKey().Key == ConsoleKey.Y);
        }
    }
}
