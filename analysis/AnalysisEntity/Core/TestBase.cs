using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisEntity
{
    public class TestBase
    {
        internal static void Show<T>()
        {
            ReflectionReportor report = new ReflectionReportor(typeof(T));
            report.Analysis();
            report.Show();
        }
    }
}
