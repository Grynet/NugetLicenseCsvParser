using NugetLicenseCsvParser.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace NugetLicenseCsvParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var filePath = @"E:\Fredrik\Dev\Licenses.csv";

            var parser = new LicenseCsvParser();
            var dict = parser.ParseLicenseCsv(filePath);

            stopWatch.Stop();
            PrintResults(dict, stopWatch);
        }

        private static void PrintResults(ConcurrentDictionary<string, HashSet<NuGet>> dict, Stopwatch stopWatch)
        {
            Console.WriteLine("THIRD PARTY LICENCES");
            Console.WriteLine("============================\n\n");
            foreach (var pair in dict)
            {
                foreach (var nuget in pair.Value)
                {
                    Console.WriteLine($"{nuget.Name} v{nuget.Version}");
                }

                Console.WriteLine("============================");
                Console.WriteLine($"{pair.Key}");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine($"Parsed file in: {stopWatch.ElapsedMilliseconds}ms");
        }
    }
}
