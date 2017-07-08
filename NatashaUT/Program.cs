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
            //ulong? i = null;
            //object t = i;
            //Console.WriteLine(t.GetType());
            //Console.WriteLine(i.Value);

            ulong? i = null;
            string str = null;
            object t1 = str;
            //Console.WriteLine(model.ValueProperty.Value);

            ENatasha.Initialize();



            //PropertyClass model = new PropertyClass();
            //model.ValueProperty = ulong.MaxValue;
            //model.RefProperty = "Test";
            //PropertyClass.StaticRefProeprty = "Static";
            //PropertyClass.StaticValueProperty = ulong.MinValue;
            //Delegate test = EHandler.CreateMethod<PropertyClass>((il) =>
            //{
            //    //EModel modelHandler = EModel.CreateModelFromObject(model);
            //    EModel modelHandler = EModel.CreateModel<PropertyClass>().UseDefaultConstructor();
            //    modelHandler.Set("<StaticValueProperty>k__BackingField", ulong.MaxValue);
            //    //modelHandler.Set("StaticValueProperty", ulong.MinValue);
            //    //modelHandler.Set("RefProperty", "1");
            //    //modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
            //    modelHandler.Load();
            //}).Compile();
            //Func<PropertyClass> action = (Func<PropertyClass>)test;
            //PropertyClass result = action();


            //T2();
            T<ulong?>();
            //T<string>();









            DebugHelper.Close();
            Console.ReadKey();
        }
        public static void T2()
        {
            ulong? model = 1;
            FieldInfo info= typeof(ulong?).GetField("value", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            object obj = info.GetValue(model);
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                //EModel modelHandler = EModel.CreateModel<ulong?>().UseDefaultConstructor();
                //modelHandler.Set("<ValueProperty>k__BackingField", (ulong)100);
                //modelHandler.Set("ValueField", (ulong)100);
                //modelHandler.Set("StaticValueField", (ulong)100);
                //modelHandler.Set("ValueProperty", (ulong)100);
                //modelHandler.Set("<StaticValueProperty>k__BackingField", (ulong)300);
                //modelHandler.LoadValue("<ValueProperty>k__BackingField").Packet();

                //FieldInfo info = typeof(ClassWithNullableModel).GetField("<PrivateProperty>k__BackingField", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                //il.REmit(OpCodes.Ldtoken, typeof(ClassWithNullableModel));
                //il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                //il.REmit(OpCodes.Ldstr, "<PrivateProperty>k__BackingField");
                //il.REmit(OpCodes.Ldc_I4_S, 60);
                //il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                //LocalBuilder field = il.DeclareLocal(typeof(FieldInfo));
                //il.REmit(OpCodes.Stloc_S, field.LocalIndex);
                //il.LoadBuilder(field);
                ////il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                //modelHandler.InStackAndPacket();
                //il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
                //, modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction
                modelHandler.LoadValue("value");
                //modelHandler.Set("ValueProperty", modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction); 
                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("ValueProperty").DLoad("Value").DelayAction);
                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction);
                //modelHandler.LoadValue("ValueField").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("ValueProperty").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("StaticValueField").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("StaticValueProperty").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");

                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("ValueField").DelayAction);
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine", modelHandler.DLoad("StaticValueProperty").DelayAction);
                //modelHandler.Set("ValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                //modelHandler.Set("StaticValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                // modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
                //modelHandler.Load();
            }).Compile();
            Func<ulong> action1 = (Func<ulong>)test;
            ulong result1 = action1();
        }
        public static void T<T>()
        {
            ClassWithNullableModel model = new ClassWithNullableModel();
            model.ValueField = 100;
            model.ValueProperty = null;
            ClassWithNullableModel.StaticValueField = 100;
            ClassWithNullableModel.StaticValueProperty = 200;
            Delegate test = EHandler.CreateMethod<ClassWithNullableModel>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                //EModel modelHandler = EModel.CreateModel<ClassWithNullableModel>().UseDefaultConstructor();
                //modelHandler.Set("<ValueProperty>k__BackingField", (ulong)100);
                //modelHandler.Set("ValueField", (ulong)100);
                //modelHandler.Set("StaticValueField", (ulong)100);
                //modelHandler.Set("ValueProperty", (ulong)100);
                //modelHandler.Set("<StaticValueProperty>k__BackingField", (ulong)300);
                //modelHandler.LoadValue("<ValueProperty>k__BackingField").Packet();
                //FieldInfo info = typeof(ClassWithNullableModel).GetField("<PrivateProperty>k__BackingField", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                //il.REmit(OpCodes.Ldtoken, typeof(ClassWithNullableModel));
                //il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                //il.REmit(OpCodes.Ldstr, "<PrivateProperty>k__BackingField");
                //il.REmit(OpCodes.Ldc_I4_S, 60);
                //il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                //LocalBuilder field = il.DeclareLocal(typeof(FieldInfo));
                //il.REmit(OpCodes.Stloc_S, field.LocalIndex);
                //il.LoadBuilder(field);
                ////il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                //modelHandler.InStackAndPacket();
                //il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
                //, modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction
                //modelHandler.Load("<PrivateProperty>k__BackingField").LoadValue("value");
                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine");
                //modelHandler.Set("ValueProperty", modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction); 
                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("ValueProperty").DLoad("Value").DelayAction);
                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("PrivateProperty").DLoad("Value").DelayAction);
                //modelHandler.LoadValue("ValueField").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("ValueProperty").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("StaticValueField").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");
                //modelHandler.LoadValue("StaticValueProperty").Packet();
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine");

                //EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", modelHandler.DLoad("ValueField").DelayAction);
                //EMethod.Load(typeof(Console)).ExecuteMethod<object>("WriteLine", modelHandler.DLoad("StaticValueProperty").DelayAction);
                //modelHandler.Set("ValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                //modelHandler.Set("StaticValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                // modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func< ClassWithNullableModel> action1 = (Func<ClassWithNullableModel>)test;
            ClassWithNullableModel t = action1();
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
