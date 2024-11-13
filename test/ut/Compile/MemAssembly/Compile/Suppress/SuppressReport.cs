using MemAssembly.Compile.Suppress.Utils;

namespace MemAssembly.Compile.Suppress
{
    [Trait("基础编译(REF) TODO", "禁断信息")]
    public class SuppressReport : CompilePrepareBase
    {
        private readonly string _script = @"public class A{ 
[System.Diagnostics.CodeAnalysis.SuppressMessage(""CodeQuality"", ""IDE0051:Remove unused private members"", Justification = ""Not production code."")] 
private int Age;
public string? Name;  }";
        [Fact(DisplayName = "忽略")]
        public void TestNullable1()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .AddSupperess("CS0169"));
            Assert.Empty(diagnostics);
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051: Remove unused private members", Justification = "Not production code.")] 
        [Fact(DisplayName = "启用")]
        public void TestNullable2()
        {

            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script, out var succeed,
                opt => opt
                //.AddSupperess("CS0169")
                //.WithDiagnosticLevel(ReportDiagnostic.Suppress)
                .WithSuppressReportor());
            Assert.True(succeed);
            Assert.NotEmpty(diagnostics);
            Assert.False(diagnostics[0].IsSuppressed);
        }

        [Fact(DisplayName = "抑制")]
        public void TestNullable3()
        {

            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script, out var succeed,
                opt => opt
                //.AddSupperess("CS0169")
                //.WithDiagnosticLevel(ReportDiagnostic.Suppress)
                .WithoutSuppressReportor());
            Assert.True(succeed);
            Assert.NotEmpty(diagnostics);
            Assert.False(diagnostics[0].IsSuppressed);
        }
    }
}
