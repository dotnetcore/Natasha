using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public interface IMethodPackage
    {
        (string Flag,IEnumerable<Type> Types, string Script,Type Delegate) Package();
    }
}
