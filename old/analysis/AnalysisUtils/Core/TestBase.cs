using System;

namespace AnalysisUtils
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
