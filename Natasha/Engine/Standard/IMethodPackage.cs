using System;
using System.Collections.Generic;

namespace Natasha
{
    /// <summary>
    /// 方法打包接口
    /// </summary>
    public interface IMethodPackage
    {
        (string Flag,IEnumerable<Type> Types, string Script,Type Delegate) Package();
    }
}
