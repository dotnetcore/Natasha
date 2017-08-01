using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.Core.Complex
{
    internal enum LinkCallOption
    {
        Default,

        Public_Field_Value_Call,
        Public_Property_Value_Call,
        Public_Static_Field_Value_Call,
        Public_Static_Property_Value_Call,

        Public_Field_Ref_Call,
        Public_Property_Ref_Call,
        Public_Static_Field_Ref_Call,
        Public_Static_Property_Ref_Call,

        Private_Field_Value_Call,
        Private_Property_Value_Call,
        Private_Static_Field_Value_Call,
        Private_Static_Property_Value_Call,

        Private_Field_Ref_Call,
        Private_Property_Ref_Call,
        Private_Static_Field_Ref_Call,
        Private_Static_Property_Ref_Call,

        Public_Value_Call,
        Public_Ref_Call,
        Private_Ref_Call,
        Private_Value_Call

    }
}
