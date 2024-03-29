﻿using System;
using Xunit;

namespace NatashaFunctionUT.Compile
{
    [Trait("基础功能测试", "编译")]
    public class CompileReferencesTest : CompilePrepare
    {

        private const string HighVersionCode = @"using MetadataDiff;public class A{   public string Get(){ return (new MetadataModel()).Name;  }  }";
        private const string LowVersionCode = @"using MetadataDiff;public class A{   public string Get(){ return (new MetadataModel()).Old_Name;  }  }";
        private static void AssertHighVersionCode(bool compileResult, AssemblyCompareInformation referenceLoadBehavior)
        {
            (string name, string currentName,bool compileSucceed) = CompileMetadataDiffCode(HighVersionCode, referenceLoadBehavior);
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.NotEqual("Default", name);
            Assert.Equal(name, currentName);
            Assert.Equal(compileResult, compileSucceed);
            Assert.True(DomainManagement.IsDeleted(currentName));
        }
        private static void AssertLowVersionCode(bool compileResult, AssemblyCompareInformation referenceLoadBehavior)
        {
            (string name, string currentName, bool compileSucceed) = CompileMetadataDiffCode(LowVersionCode, referenceLoadBehavior);
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.NotEqual("Default", name);
            Assert.Equal(name, currentName);
            Assert.Equal(compileResult, compileSucceed);
            Assert.True(DomainManagement.IsDeleted(currentName));
        }


        [Fact(DisplayName = "[高版本引用]编译测试")]
        public void HCompile()
        {
            AssertHighVersionCode(true, AssemblyCompareInformation.UseHighVersion);
            AssertLowVersionCode(false, AssemblyCompareInformation.UseHighVersion);
        }

        [Fact(DisplayName = "[低版本引用]编译测试")]
        public void LCompile()
        {
            AssertLowVersionCode(true, AssemblyCompareInformation.UseLowVersion);
            AssertHighVersionCode(false, AssemblyCompareInformation.UseLowVersion);
        }

        [Fact(DisplayName = "[混合版本引用]编译测试")]
        public void CCompile()
        {
            AssertHighVersionCode(true, AssemblyCompareInformation.None);
            AssertLowVersionCode(false, AssemblyCompareInformation.None);
        }

        [Fact(DisplayName = "[默认域引用]编译测试")]
        public void DCompile()
        {
            AssertLowVersionCode(false, AssemblyCompareInformation.UseDefault);
            AssertHighVersionCode(true, AssemblyCompareInformation.UseDefault);
        }
    }

}
