using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{

    public class DefinedNameTemplate<T> : DefinedNullableTemplate<T> where T : DefinedNameTemplate<T>, new()
    {

        public string NameScript = string.Empty;


        public T UseRandomName()
        {

            NameScript = "N" + Guid.NewGuid().ToString("N");
            return Link;

        }




        /// <summary>
        /// 设置定义的名称
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public T Name(string name)
        {
            NameScript = name;
            return Link;
        }




        /// <summary>
        /// 根据成员元数据来反射定义类型
        /// </summary>
        /// <param name="typeInfo">成员元数据</param>
        /// <returns></returns>
        public T Name(MemberInfo memberInfo)
        {
            if (memberInfo.Name.IndexOf('<') == 0 && memberInfo.Name.IndexOf('.') != -1)
            {
                return Name("Invoke");
            }
            return Name(memberInfo.Name);

        }





        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [{this}]{}
            base.BuilderScript();
            _script.Append(NameScript);
            return Link;

        }

    }

}
