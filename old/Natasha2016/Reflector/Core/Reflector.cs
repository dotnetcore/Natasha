using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{
    
    public class Reflector<T>
    {
        public void a() {
            var a = typeof(T);
            a.GetRuntimeField();
         }
    }


}
