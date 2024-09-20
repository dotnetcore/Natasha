using Microsoft.CodeAnalysis;
using RefAssembly.Compile.Suppress.Utils;

namespace RefAssembly.Compile.Suppress
{
    [Trait("基础编译(REF)", "标点错误")]
    public class DiagnosticsLevelError : CompilePrepareBase
    {
        private readonly string _script = "public class A{  public string Name B; public int? Age;  }";

        [Fact(DisplayName = "错误")]
        public void TestError2()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Error)
                );
            Assert.Equal(4,diagnostics.Count);
            Assert.Equal("CS1002", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[1].Id);
            Assert.False(diagnostics[1].IsSuppressed);
            Assert.False(diagnostics[1].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[2].Id);
            Assert.False(diagnostics[2].IsSuppressed);
            Assert.False(diagnostics[2].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[2].DefaultSeverity);

            Assert.Equal("CS8618", diagnostics[3].Id);
            Assert.False(diagnostics[3].IsSuppressed);
            Assert.True(diagnostics[3].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[3].DefaultSeverity);
        }

        [Fact(DisplayName = "警告")]
        public void TestError3()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Warn)
                );
            Assert.Equal(4, diagnostics.Count);
            Assert.Equal("CS1002", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[1].Id);
            Assert.False(diagnostics[1].IsSuppressed);
            Assert.False(diagnostics[1].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].DefaultSeverity);

            Assert.Equal("CS1519", diagnostics[2].Id);
            Assert.False(diagnostics[2].IsSuppressed);
            Assert.False(diagnostics[2].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[2].DefaultSeverity);

            Assert.Equal("CS8618", diagnostics[3].Id);
            Assert.False(diagnostics[3].IsSuppressed);
            Assert.False(diagnostics[3].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[3].DefaultSeverity);
        }

        [Fact(DisplayName = "信息")]
        public void TestError4()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Info)
                );
            Assert.Equal(4, diagnostics.Count);
            Assert.Equal("CS1002", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[1].Id);
            Assert.False(diagnostics[1].IsSuppressed);
            Assert.False(diagnostics[1].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[2].Id);
            Assert.False(diagnostics[2].IsSuppressed);
            Assert.False(diagnostics[2].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[2].DefaultSeverity);

            Assert.Equal("CS8618", diagnostics[3].Id);
            Assert.False(diagnostics[3].IsSuppressed);
            Assert.False(diagnostics[3].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[3].DefaultSeverity);
        }

        [Fact(DisplayName = "禁断")]
        public void TestError5()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Suppress)
                );
            Assert.Equal(3, diagnostics.Count);
            Assert.Equal("CS1002", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[1].Id);
            Assert.False(diagnostics[1].IsSuppressed);
            Assert.False(diagnostics[1].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[2].Id);
            Assert.False(diagnostics[2].IsSuppressed);
            Assert.False(diagnostics[2].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[2].DefaultSeverity);
        }

        [Fact(DisplayName = "隐藏")]
        public void TestError6()
        {
            var diagnostics = DiagnosticsHelper.GetDiagnostics(_script,
                opt => opt
                .WithDiagnosticLevel(ReportDiagnostic.Hidden)
                );
            Assert.Equal(4, diagnostics.Count);
            Assert.Equal("CS1002", diagnostics[0].Id);
            Assert.False(diagnostics[0].IsSuppressed);
            Assert.False(diagnostics[0].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[0].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[1].Id);
            Assert.False(diagnostics[1].IsSuppressed);
            Assert.False(diagnostics[1].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[1].DefaultSeverity);
            Assert.Equal("CS1519", diagnostics[2].Id);
            Assert.False(diagnostics[2].IsSuppressed);
            Assert.False(diagnostics[2].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Error, diagnostics[2].DefaultSeverity);

            Assert.Equal("CS8618", diagnostics[3].Id);
            Assert.False(diagnostics[3].IsSuppressed);
            Assert.False(diagnostics[3].IsWarningAsError);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostics[3].DefaultSeverity);
        }
    }

}
