using System.Text;

namespace Natasha.CSharp.Template
{

    //temp 临时的，后面再改，排除掉原来的类改动太大了
    public class MethodBodyTemplate<T> : ParameterTemplate<T> where T : MethodBodyTemplate<T>, new()
    {

        public StringBuilder BodyScript;
        public MethodBodyTemplate()
        {
            BodyScript = new StringBuilder();
        }




        /// <summary>
        /// 设置方法体
        /// </summary>
        /// <param name="body">方法体字符串</param>
        /// <returns></returns>
        public T Body(string bodyString)
        {

            BodyScript.Clear();
            BodyScript.Append('{');
            BodyScript.Append(bodyString);
            BodyScript.Append("}");
            return Link;

        }
        public T BodyAppend(string bodyString)
        {

            if (BodyScript[0] == '{')
            {
                BodyScript.Insert(BodyScript.Length - 1,bodyString);
            }
            else
            {
                Body(bodyString);
            }
            return Link;

        }
        public T NoBody(string suffix)
        {
            BodyScript.Clear().Append(suffix);
            return Link;
        }




        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [modifier] [type] [Name]([Parameter])[{this}]
            base.BuilderScript();
            _script.Append(BodyScript);
            return Link;

        }


    }

}
