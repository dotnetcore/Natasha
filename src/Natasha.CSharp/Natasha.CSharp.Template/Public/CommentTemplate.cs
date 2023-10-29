using System;
using System.Collections.Generic;

namespace Natasha.CSharp.Template
{
    public class CommentTemplate<T> : GlobalUsingTemplate<T> where T : CommentTemplate<T>, new()
    {

        public string CommentScript;
        private string _summary;
        private string _summaryReturn;
        private List<string> _summaryParameters;
        public CommentTemplate()
        {
            CommentScript = string.Empty;
            _summary = string.Empty;
            _summaryReturn = string.Empty;
            _summaryParameters = new List<string>();
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


        public T Summary(string comment)
        {
            _summary = @$"
/// <summary>
/// {comment.Replace(Environment.NewLine, "")}
/// </summary>";
            return Link;
        }


        public T SummaryReturn(string comment)
        {
            _summaryReturn = $"/// <returns>{comment.Replace(Environment.NewLine, "")}</returns>";
            return Link;
        }

        /// <summary>
        /// 添加 summary 注释的参数注释
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public T SummaryParameter(string paramName,string comment)
        {
            _summaryParameters.Add($"/// <param name=\"{paramName}\">{comment.Replace(Environment.NewLine, "")}</param>");
            return Link;
        }



        
        /// <summary>
        /// 根据类型反射得到保护级别
        /// </summary>
        /// <param name="type">外部类型</param>
        /// <returns></returns>
        public T Comment(string comment)
        {

            CommentScript = $"//{comment}";
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
            if (_summary!=string.Empty)
            {
                _script.AppendLine(_summary);
                foreach (var item in _summaryParameters)
                {
                    _script.AppendLine(item);
                }
                _script.AppendLine(_summaryReturn);
            }
            return Link;


        }



    }
}
