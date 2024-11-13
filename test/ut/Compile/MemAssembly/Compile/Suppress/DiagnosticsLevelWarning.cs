using Microsoft.CodeAnalysis;
using MemAssembly.Compile.Suppress.Utils;

namespace MemAssembly.Compile.Suppress
{
    [Trait("基础编译(REF)", "空引用")]
    public class DiagnosticsLevelWarning : CompilePrepareBase
    {
        private readonly string _script = "public class A{  public string Name; public int? Age;  }";
        [Fact(DisplayName = "关")]
        public void TestNullable1()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt=>opt.WithNullableCompile(NullableContextOptions.Disable));
            Assert.Empty(diagnostics);
        }


        [Fact(DisplayName = "错误")]
        public void TestNullable2()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Error)
                .WithNullableCompile(NullableContextOptions.Enable));
            Assert.Single(diagnostics);
            Assert.Equal("CS8618", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.True(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[0].DefaultSeverity);
        }

        [Fact(DisplayName = "警告")]
        public void TestNullable3()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Warn)
                .WithNullableCompile(NullableContextOptions.Enable));
            Assert.Single(diagnostics);
            Assert.Equal("CS8618", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[0].DefaultSeverity);
        }

        [Fact(DisplayName = "信息")]
        public void TestNullable4()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Info)
                .WithNullableCompile(NullableContextOptions.Enable));
            Assert.Single(diagnostics);
            Assert.Equal("CS8618", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[0].DefaultSeverity);

        }

        [Fact(DisplayName = "禁断")]
        public void TestNullable5()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Suppress)
                .WithNullableCompile(NullableContextOptions.Enable));
            Assert.Empty(diagnostics);

        }

        [Fact(DisplayName = "隐藏")]
        public void TestNullable6()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Hidden)
                .WithNullableCompile(NullableContextOptions.Enable));
            Assert.Single(diagnostics);
            Assert.Equal("CS8618", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[0].DefaultSeverity);
        }

    }

}
