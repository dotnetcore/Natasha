using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NatashaUT
{
    public class PrepareTest
    {
        static PrepareTest()
        {
            NatashaInitializer.InitializeAndPreheating();
            Thread.Sleep(3000);
        }
    }
}
