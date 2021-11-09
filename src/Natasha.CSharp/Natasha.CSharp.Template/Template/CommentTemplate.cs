using Natasha.CSharp.Reverser;
using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{
    public class CommentTemplate<T> : GlobalUsingTemplate<T> where T : CommentTemplate<T>, new()
    {

        public string CommentScript;

        public CommentTemplate()
        {
            CommentScript = string.Empty;
        }

        /// <summary>
        /// 不使用访问级别定义
        /// </summary>
        /// <returns></returns>
        public T NoComment()
        {

            CommentScript = string.Empty;
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
            if (CommentScript != string.Empty)
            {
                _script.Append(CommentScript);
            }
            return Link;


        }



    }
}
