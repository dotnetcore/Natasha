using System;

namespace AnalysisUtils
{
    public class AnalysisTool
    {
        public static void Test()
        {
            Test_Internal_No.Test();
            Test_Pulibc_No.Test();

            System.Console.ReadKey();
        }
    }
}
