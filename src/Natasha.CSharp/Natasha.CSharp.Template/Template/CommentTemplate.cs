using Natasha.CSharp.Reverser;
using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{
    public class CommentTemplate<T> : GlobalUsingTemplate<T> where T : CommentTemplate<T>, new()
    {

        public string CommentScript;


        /// <summary>
        /// 不使用访问级别定义
        /// </summary>
        /// <returns></returns>
        public T NoComment()
        {

            CommentScript = default;
            return Link;

        }



        
        /// <summary>
        /// 根据类型反射得到保护级别
        /// </summary>
        /// <param name="type">外部类型</param>
        /// <returns></returns>
        public T Comment(string comment)
        {

            CommentScript = comment;
            return Link;

        }




        public override T BuilderScript()
        {

            // [{this}]
            // [attribute]
            base.BuilderScript();
            if (CommentScript != default)
            {
                _script.Append(CommentScript);
            }
            return Link;


        }



    }
}
