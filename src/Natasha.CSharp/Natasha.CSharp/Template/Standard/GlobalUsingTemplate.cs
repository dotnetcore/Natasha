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
        internal readonly NatashaUsingCache usingRecorder;


        public GlobalUsingTemplate()
        {

            usingRecorder = new();
            _script = new StringBuilder(200);

        }


        internal T RecordUsing(NatashaUsingCache usingCache)
        {
            usingRecorder.Using(usingCache._usings);
            return Link;
        }



        public virtual T RecordUsing(Type type)
        {

            usingRecorder.Using(type);
            return Link;

        }



        public virtual T RecordUsing(IEnumerable<Type> types)
        {

            usingRecorder.Using(types);
            return Link;

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
