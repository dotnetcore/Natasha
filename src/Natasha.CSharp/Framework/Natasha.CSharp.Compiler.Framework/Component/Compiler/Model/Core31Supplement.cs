using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{

#if !NET5_0_OR_GREATER
    public sealed class SkipLocalsInitAttribute : Attribute { }
#endif

}