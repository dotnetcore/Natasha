using Natasha.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.Template
{
    /// <summary>
    /// 记录模板
    /// </summary>
    /// <typeparam name="T">LINK返回的类型</typeparam>
    public class GlobalUsingTemplate<T> : StringWrapperTemplate<T>, ILinkScriptBuilder<T> where T : GlobalUsingTemplate<T>, new()
    {

        public StringBuilder _script;
        public readonly TypeRecoder UsingRecoder;


        public GlobalUsingTemplate()
        {

            UsingRecoder = new TypeRecoder();
            _script = new StringBuilder(200);

        }




        public virtual T RecoderType(Type type)
        {

            UsingRecoder.Add(type);
            return Link;

        }



        public virtual T RecoderType(IEnumerable<Type> types)
        {

            UsingRecoder.Union(types);
            return Link;

        }




        public string Script
        {
            get { _script.Clear(); BuilderScript(); _cache = _script.ToString(); return _cache; }
        }

        public virtual HashSet<string> Usings => default;

        public TypeRecoder Recoder => UsingRecoder;



        private string _cache;

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

            return Link;

        }

    }

}
