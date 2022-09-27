using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FrameworkFunctionUT
{
    public static class OSStringCompare
    {
        public static void Equal(string expected,string actual)
        {
            Assert.Equal(expected.Replace("\r\n","\n"), actual.Replace("\r\n", "\n"));

        }
    }
}
