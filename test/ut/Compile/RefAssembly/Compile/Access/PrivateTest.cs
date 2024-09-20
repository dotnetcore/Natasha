using System;

namespace RefAssembly.Compile.Access
{
    [Trait("基础编译(REF)", "元数据可见性")]
    public class PrivateTest : CompilePrepareBase
    {

        private readonly string _privateAssemblyName = typeof(AccessModelTest).Assembly.GetName().Name!;
        [Fact(DisplayName = "全部导入")]
        public void AccessTest1()
        {

            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt
                    .AppendCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility)
                    .WithAllMetadata());

            string text = $@"
public class A 
{{ 
    private string GetPrivate(){{  
        return new AccessModelTest().GetPrivate();  
    }} 
    internal string GetInternal(){{  
        return new AccessModelTest().GetInternal();  
    }} 
    public string GetPublic(){{  
        return new AccessModelTest().GetPublic();  
    }} 
}}";


            builder.Add(text.ToAccessPrivateTree(typeof(AccessModelTest)));
            var asm = builder
                .GetAssembly();

            var publicFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetPublic");
            Assert.NotNull(publicFunc);
            Assert.Equal("Public", publicFunc());

            var internalFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetInternal");
            Assert.NotNull(internalFunc);
            Assert.Equal("Internal", internalFunc());

            var privateFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetPrivate");
            Assert.NotNull(privateFunc);
            Assert.Equal("Private", privateFunc());
        }

        [Fact(DisplayName = "内部导入-OK")]
        public void AccessTest3()
        {

            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt
                    .AppendCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility)
                    .WithInternalMetadata());

            string text = $@"
public class A 
{{ 
    internal string GetInternal(){{  
        return new AccessModelTest().GetInternal();  
    }} 
    public string GetPublic(){{  
        return new AccessModelTest().GetPublic();  
    }} 
}}";

            builder.Add(text.ToAccessPrivateTree(typeof(AccessModelTest)));
            var asm = builder
                .GetAssembly();

            var publicFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetPublic");
            Assert.NotNull(publicFunc);
            Assert.Equal("Public", publicFunc());

            var internalFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetInternal");
            Assert.NotNull(internalFunc);
            Assert.Equal("Internal", internalFunc());
        }

        [Fact(DisplayName = "内部导入-Error")]
        public void AccessTest2()
        {

            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt
                    .AppendCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility)
                    .WithInternalMetadata());

            string text = $@"
public class A 
{{ 
    private string GetPrivate(){{  
        return new AccessModelTest().GetPrivate();  
    }} 
    internal string GetInternal(){{  
        return new AccessModelTest().GetInternal();  
    }} 
    public string GetPublic(){{  
        return new AccessModelTest().GetPublic();  
    }} 
}}";

            try
            {
                builder.Add(text.ToAccessPrivateTree(typeof(AccessModelTest)));
                var asm = builder.GetAssembly();
            }
            catch (Exception ex)
            {
                var error = ex as NatashaException;
                Assert.NotNull(error);
                Assert.Equal(NatashaExceptionKind.Compile, error!.ErrorKind);
                var diagnostic = builder.GetDiagnostics();
                Assert.Single(diagnostic!.Value);
                foreach (var item in diagnostic!)
                {
                    Assert.Equal("CS1061", item.Id);
                }
            }
        }


        [Fact(DisplayName = "公共导入-OK")]
        public void AccessTest4()
        {

            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt
                    .WithPublicMetadata());

            string text = $@"
public class A 
{{ 
    public string GetPublic(){{  
        return new AccessModelTest().GetPublic();  
    }} 
}}";

            builder.Add(text);
            var asm = builder
                .GetAssembly();

            var publicFunc = asm.GetDelegateFromShortName<Func<string>>("A", "GetPublic");
            Assert.NotNull(publicFunc);
            Assert.Equal("Public", publicFunc());

        }

        [Fact(DisplayName = "公共导入-Error")]
        public void AccessTest5()
        {

            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt
                    .AppendCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility)
                    .WithPublicMetadata());

            string text = $@"
public class A 
{{ 
    private string GetPrivate(){{  
        return new AccessModelTest().GetPrivate();  
    }} 
    internal string GetInternal(){{  
        return new AccessModelTest().GetInternal();  
    }} 
    public string GetPublic(){{  
        return new AccessModelTest().GetPublic();  
    }} 
}}";

            try
            {
                builder.Add(text.ToAccessPrivateTree(typeof(AccessModelTest)));
                var asm = builder.GetAssembly();

            }
            catch (Exception ex)
            {
                var error = ex as NatashaException;
                Assert.NotNull(error);
                Assert.Equal(NatashaExceptionKind.Compile, error!.ErrorKind);
                var diagnostic = builder.GetDiagnostics();
                Assert.Equal(2, diagnostic!.Value.Length);
                foreach (var item in diagnostic!)
                {
                    Assert.Equal("CS1061", item.Id);
                }
            }
        }

    }
}
