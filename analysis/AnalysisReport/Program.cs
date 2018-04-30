using System;

namespace AnalysisReport
{
    class Program
    {
        static void Main(string[] args)
        {
            Show<Public_ProtectedInternalEntity>();
            Console.ReadKey();
        }

        public static void Show<T>()
        {
            ReflectionReportor report = new ReflectionReportor(typeof(T));
            report.Analysis();
            report.Show();
        }
    }
}
