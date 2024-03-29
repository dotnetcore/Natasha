﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT
{
    [Trait("基础功能测试", "Using")]
    public class UsingTest : CompilerPrepareBase
    {
        [Fact(DisplayName = "默认Using的添加")]
        public void DefaultUsingTest()
        {
            Assert.True(NatashaLoadContext.DefaultContext.UsingRecorder.ToString() != string.Empty);

            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }


    }
}
