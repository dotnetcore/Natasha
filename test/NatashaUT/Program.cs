using Natasha;
using Natasha.Cache;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NatashaUT
{
    class Program
    {
        
        public static void Main()
        {
            FieldClass model = new FieldClass();
            model.ValueField = 100;
            model.RefField = "Test";
            FieldClass.StaticRefField = "Static";
            FieldClass.StaticValueField = 200;
            Delegate test = EHandler.CreateMethod<FieldClass>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueField", modelHandler.DLoad("ValueField").Operator + modelHandler.DLoad("StaticValueField").DelayAction);
                //modelHandler.Set("StaticValueField", modelHandler.DLoadValue("ValueField").Operator + modelHandler.DLoadValue("StaticValueField").DelayAction);
                //modelHandler.Set("RefField", modelHandler.DLoadValue("RefField").Operator + modelHandler.DLoadValue("StaticRefField").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<FieldClass> action = (Func<FieldClass>)test;
            FieldClass result = action();
        }
        public short GetShort()
        {
            Console.WriteLine(short.MaxValue);
            return short.MaxValue;
        }
        public int GetInt()
        {
            Console.WriteLine(32767);
            return 32767;
        }    }
}
