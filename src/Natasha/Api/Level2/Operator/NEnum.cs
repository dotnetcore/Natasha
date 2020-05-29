using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{

    public class NEnum : NHandler<NEnum>
    {

        public NEnum()
        {

            Link = this;
            this.Enum();

        }




        /// <summary>
        /// 设置枚举字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public NEnum EnumField(string name, int value)
        {

            if (BodyScript.Length > 0)
            {
                BodyAppendLine(",");
            }
            BodyAppend($"{name}={value}");
            return Link;

        }




        /// <summary>
        /// 设置枚举字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NEnum EnumField(string name)
        {

            if (BodyScript.Length > 0)
            {
                BodyAppendLine(",");
            }
            BodyAppend(name);
            return Link;

        }

    }

}
