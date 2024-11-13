using Natasha.CSharp.Extension.Inner;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RefAssembly.Log.Utils
{
    internal static class LogCheckHelper
    {

        static Regex removeTime = new("Time     :\t.*?\n", RegexOptions.Compiled | RegexOptions.Singleline);

        internal static void JudgeLogError(string script, string fileName, bool withNullable = true, params string[]? otherScripts)
        {
            NatashaCompilationLog? log = null;
            try
            {
                AssemblyCSharpBuilder builder = new(fileName);
                builder
                    .UseRandomLoadContext()
                    .ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(typeof(object)))
                    .WithCombineReferences(item => item.UseCustomReferences())
                    .UseSimpleMode()
                    .WithSemanticCheck()
                    .WithoutCombineUsingCode();
                if (withNullable)
                {
                    builder.ConfigCompilerOption(opt => opt.WithNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Enable));
                }
                else
                {
                    builder.ConfigCompilerOption(opt => opt.WithNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Disable));
                }
                builder.Add(script);
                if (otherScripts != null)
                {
                    for (int i = 0; i < otherScripts.Length; i++)
                    {
                        builder.Add(otherScripts[i]);
                    }
                }
                builder.CompileFailedEvent += (compilation, errors) =>
                {
                    log = compilation.GetNatashaLog();
                };
                builder.GetAssembly();
            }
            catch (Exception ex)
            {

                var expectedLog = GetText(fileName);
                Assert.NotNull(log);
                Assert.True(ex is NatashaException);
                var actualLog = log!.ToString();
                CompareLogEqual(expectedLog, actualLog);

            }
        }
        internal static void JudgeLogSuccess(string script, string fileName, bool withNullable = true, params string[]? otherScripts)
        {
            NatashaCompilationLog? log = null;
            try
            {
                AssemblyCSharpBuilder builder = new(fileName);
                builder
                    .UseRandomLoadContext()
                    .ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(typeof(object)))
                    .WithCombineReferences(item => item.UseCustomReferences())
                    .UseSimpleMode()
                    .WithoutCombineUsingCode();
                if (withNullable)
                {
                    builder.ConfigCompilerOption(opt => opt.WithNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Enable));
                }
                else
                {
                    builder.ConfigCompilerOption(opt => opt.WithNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Disable));
                }
                builder.Add(script);
                if (otherScripts != null)
                {
                    for (int i = 0; i < otherScripts.Length; i++)
                    {
                        builder.Add(otherScripts[i]);
                    }
                }
                builder.CompileSucceedEvent += (compilation, errors) =>
                {
                    log = compilation.GetNatashaLog();
                };
                builder.GetAssembly();
                var expectedLog = GetText(fileName);
                Assert.NotNull(log);
                var actualLog = log!.ToString();
                CompareLogEqual(expectedLog, actualLog);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);

            }
        }
        private static void CompareLogEqual(string expected, string actual)
        {
            Assert.Equal(removeTime.Replace(expected, ""), removeTime.Replace(actual, ""));
        }

        private static string GetText(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "LogInfo", "LogFile", fileName + ".txt");
            return File.ReadAllText(path);
        }
    }
}
