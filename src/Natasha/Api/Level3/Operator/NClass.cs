using Natasha.Builder;
using System;

namespace Natasha
{

    /// <summary>
    /// 默认创建一个公有的类
    /// </summary>
    public class NClass : NHandler<NClass>
    {

        public NClass()
        {

            Link = this;
            Public.ChangeToClass();

        }



    }

}
