using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.Component.Domain
{
    public enum LoadVersionResultEnum
    {
        NoAction,
        UseBefore,
        UseAfter,
        PassToNextHandler
    }
}
