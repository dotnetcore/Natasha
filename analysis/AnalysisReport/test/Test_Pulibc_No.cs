using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisReport
{
    class Test_Pulibc_No: TestBase
    {
        public static void Test()
        {
            Show<Public_InternalEntity>();
            Show<Public_PrivateEntity>();
            Show<Public_ProtectedEntity>();
            Show<Public_ProtectedInternalEntity>();
            Show<Public_PublicEntity>();
        }
    }
}
