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
    public class GlobalUsingTemplate<T> : StringWrapperTemplate<T>, ILinkScriptBuilder<T> where T : GlobalUsingTemplate<T>, new()
    {

        public readonly StringBuilder _script;
        public readonly TypeRecoder UsingRecoder;


        public GlobalUsingTemplate()
        {

            UsingRecoder = new TypeRecoder();
            _script = new StringBuilder(200);

        }




        public virtual T RecoderType(Type type)
        {

            UsingRecoder.Add(type);
            return Link!;

        }



        public virtual T RecoderType(IEnumerable<Type> types)
        {

            UsingRecoder.Union(types);
            return Link!;

        }




        public string Script
        {

            get {
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
        }

        public virtual HashSet<string> Usings => new HashSet<string>();

        public TypeRecoder Recoder => UsingRecoder;



        private string? _cache;

        public string ScriptCache
        {

            get
            {

                if (_cache == default)
                {
                   return Script;
                }
                return _cache;

            }

        }
        /// <summary>
        /// 脚本构建
        /// </summary>
        /// <returns></returns>
        public virtual T BuilderScript()
        {

            return Link!;

        }

    }

}
