#if NETCOREAPP3_0_OR_GREATER
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
            _autoLoadDomainUsing = true;
            UsingRecorder = new();
            _script = new StringBuilder(200);

        }

        private bool _autoLoadDomainUsing;
        public T LoadDomainUsing()
        {
            _autoLoadDomainUsing = true;
            return Link;
        }
        public T NotLoadDomainUsing()
        {
            _autoLoadDomainUsing = false;
            return Link;
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
                //使用全局Using
                if (_autoLoadDomainUsing)
                {
                    foreach (var @using in AssemblyBuilder.Domain.UsingRecorder._usings)
                    {
                        if (!UsingRecorder.HasUsing(@using))
                        {

                            usingScript.AppendLine($"using {@using};");

                        }
                    }
                }

            }
            else
            {

                //使用全局Using
                if (_autoLoadDomainUsing && AssemblyBuilder.Domain.Name != "Default")
                {
                    foreach (var @using in AssemblyBuilder.Domain.UsingRecorder._usings)
                    {
                        if (!DefaultUsing.HasElement(@using) && !UsingRecorder.HasUsing(@using))
                        {

                            usingScript.AppendLine($"using {@using};");

                        }
                    }
                }

                //把当前域中的using全部加上
                foreach (var @using in UsingRecorder._usings)
                {

                    //如果全局已经存在using了，就不加了
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