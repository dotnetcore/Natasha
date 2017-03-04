using Natasha.Core;
using System;

namespace Natasha.Utils
{
    //链式调用
    public class ELink
    {
        public static EModel GetLink(Type type)
        {
             return  EModel.CreateModelFromAction(null, type);
        }
    }
}
