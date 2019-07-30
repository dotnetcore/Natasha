using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Clone
{
    public static class CloneExtension
    {
        public static T Clone<T>(this T instance)
        {
            return CloneOperator.Clone(instance);
        }
    }
}
