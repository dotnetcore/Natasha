#if !NETCOREAPP3_0_OR_GREATER
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.Template
{
    /// <summary>
    /// 记录模板
    /// </summary>
    /// <typeparam name="T">LINK返回的类型</typeparam>
    public partial class GlobalUsingTemplate<T> : ScriptTemplate<T> where T : GlobalUsingTemplate<T>, new()
    {
        public GlobalUsingTemplate()
        {
            _useGlobalUsing = true;
            UsingRecorder = new();
            _script = new StringBuilder(200);

        }
        public StringBuilder GetUsingBuilder()
        {
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif
            var usingScript = new StringBuilder();

            //如果用户想使用自定义的Using
            if (!_useGlobalUsing)
            {
                foreach (var @using in UsingRecorder._usings)
                {

                    usingScript.AppendLine($"using {@using};");

                }
            }
            else
            {
                foreach (var @using in UsingRecorder._usings)
                {
                    if (!DefaultUsing.HasElement(@using))
                    {

                        usingScript.AppendLine($"using {@using};");

                    }
                }
                usingScript.Append(DefaultUsing.UsingScript);

            }
        
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[using]", "using 组合", 3);
#endif
            return usingScript;
        }

    }

}
#endif