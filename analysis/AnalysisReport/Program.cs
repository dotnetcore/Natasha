using System;

namespace AnalysisReport
{
    class Program
    {
        static void Main(string[] args)
        {
            ReflectionReportor report = new ReflectionReportor(typeof(PrivateEntity));
            report.Analysis();
            report.Show();
            Console.ReadKey();
        }
    }
}
