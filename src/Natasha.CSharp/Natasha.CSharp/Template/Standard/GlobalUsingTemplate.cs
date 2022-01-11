using Natasha.CSharp.Domain.Utils;
using Natasha.Template;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.Template
{
    /// <summary>
    /// 记录模板
    /// </summary>
    /// <typeparam name="T">LINK返回的类型</typeparam>
    public class GlobalUsingTemplate<T> : ScriptTemplate<T> where T : GlobalUsingTemplate<T>, new()
    {

        public readonly StringBuilder _script;
        public readonly NatashaUsingCache UsingRecorder;


        public GlobalUsingTemplate()
        {
            _useGlobalUsing = true;
            UsingRecorder = new();
            _script = new StringBuilder(200);

        }


        internal T RecordUsing(NatashaUsingCache usingCache)
        {
            UsingRecorder.Using(usingCache._usings);
            return Link;
        }



        public virtual T RecordUsing(Type type)
        {

            UsingRecorder.Using(type);
            return Link;

        }



        public virtual T RecordUsing(IEnumerable<Type> types)
        {

            UsingRecorder.Using(types);
            return Link;

        }


        private bool _useGlobalUsing;

        public T NoGlobalUsing()
        {
            _useGlobalUsing = false;
            return Link;
        }
        public T UseGlobalUsing()
        {
            _useGlobalUsing = true;
            return Link;
        }

        public StringBuilder GetUsingBuilder()
        {
            var  usingScript = new StringBuilder();
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

                //使用全局Using
                
                foreach (var @using in AssemblyBuilder.Domain.UsingRecorder._usings)
                {
                    if (!DefaultUsing.HasElement(@using))
                    {

                        usingScript.AppendLine($"using {@using};");

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
            return usingScript;
        }


        public override string GetScript()
        {
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif
            //Mark : 138ms
            _script.Clear();
            BuilderScript();
            _cache = _script.ToString();
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[ Script ]", $"{this.GetType().Name} 中脚本拼接耗时", 2);
#endif
            return _cache;
        }


        private string? _cache;

        public string ScriptCache
        {

            get
            {

                if (_cache == default)
                {
                   return GetScript();
                }
                return _cache;

            }

        }


    }

}
