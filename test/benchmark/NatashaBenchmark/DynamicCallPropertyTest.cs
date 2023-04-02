using NatashaBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace NatashaBenchmark
{
    public class DynamicCallPropertyTest
    {
        public DynamicCallPropertyTest()
        {
            Precache();
            Preheating();
        }
        public void Precache()
        {
            DynamicMethod method = new DynamicMethod("Advise", null, new Type[0]);
            ILGenerator il = method.GetILGenerator();
            var methods = typeof(CallModel).GetMethod("Runner", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, "123");
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Call, methods);
            il.Emit(OpCodes.Ret);
            //return (Action)method.CreateDelegate(typeof(Action));
        }
        public void Preheating()
        {

        }
        public void EmitTest()
        {

        }
        public void OriginTest()
        {
            CallModel model = new CallModel();
        }
        public void DynamicTest()
        {

        }
    }
}
