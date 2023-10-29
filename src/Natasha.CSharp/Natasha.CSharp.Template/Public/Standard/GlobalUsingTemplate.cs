using Natasha.CSharp.Using;
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
    public partial class GlobalUsingTemplate<T> : ScriptTemplate<T> where T : GlobalUsingTemplate<T>, new()
    {

        public readonly StringBuilder _script;
        public readonly NatashaUsingCache UsingRecorder;



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
