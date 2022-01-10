using System;
using System.Collections.Generic;
using System.Text;

[Flags]
public enum ConstraintFlags
{
    Class,
    Struct,
    Unmanaged,
    New
}
