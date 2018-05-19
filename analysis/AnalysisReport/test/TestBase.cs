using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisReport
{
    class TestBase
    {
        public static void Show<T>()
        {
            ReflectionReportor report = new ReflectionReportor(typeof(T));
            report.Analysis();
            report.Show();
        }
    }
}
