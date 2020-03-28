using Natasha.Builder;
using System;

namespace Natasha
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
                BodyScript.AppendLine(",");
            }
            BodyScript.Append($"{name}={value}");
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
                BodyScript.AppendLine(",");
            }
            BodyScript.Append(name);
            return Link;

        }

    }

}
