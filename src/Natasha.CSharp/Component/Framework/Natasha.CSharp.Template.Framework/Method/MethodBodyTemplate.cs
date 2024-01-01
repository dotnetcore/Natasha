
namespace Natasha.CSharp.Template
{

    //temp 临时的，后面再改，排除掉原来的类改动太大了
    public class MethodBodyTemplate<T> : MethodConstraintTemplate<T> where T : MethodBodyTemplate<T>, new()
    {

        private bool _useNoBody;
        public string BodyScript
        {
            get { return _useNoBody ? _bodyScript : $"{{{_bodyScript}}}"; }
        }
        private string _bodyScript;
        public MethodBodyTemplate()
        {
            _bodyScript = string.Empty;
        }




        /// <summary>
        /// 设置方法体
        /// </summary>
        /// <param name="body">方法体字符串</param>
        /// <returns></returns>
        public T Body(string bodyString)
        {
            _bodyScript = bodyString;
            return Link;

        }
        public T BodyAppend(string bodyString)
        {
            _bodyScript += bodyString;
            return Link;

        }
        public T NoBody(string suffix)
        {
            _useNoBody = true;
            _bodyScript = suffix;
            return Link;
        }




        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [name]([parameter])[{this}]
            base.BuilderScript();
            _script.Append(BodyScript);
            return Link;

        }


    }

}
