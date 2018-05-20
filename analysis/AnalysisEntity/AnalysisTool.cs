using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisEntity
{
    public class AnalysisTool
    {
        public static void Test()
        {
            Test_Internal_No.Test();
            Test_Pulibc_No.Test();

            Console.ReadKey();
        }
    }
}
