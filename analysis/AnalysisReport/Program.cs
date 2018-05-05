using System;

namespace AnalysisReport
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Show<Public_InternalEntity>();
             * Show<Public_PrivateEntity>();
             * Show<Public_ProtectedEntity>();
             * Show<Public_ProtectedInternalEntity>();
             * Show<Public_PublicEntity>();
             * Show<Internal_InternalEntity>();
             * Show<Internal_PrivateEntity>();
             * Show<Internal_ProtectedEntity>();
             * Show<Internal_ProtectedInternalEntity>();
             * Show<Internal_PublicEntity>();
             */
            Show<Internal_PublicEntity>();
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
