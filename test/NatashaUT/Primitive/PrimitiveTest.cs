using Natasha;
using System;
using Xunit;

namespace NatashaUT.Primitive
{
    [Trait("赋值/加载", "基元类型")]
    public class PrimitiveTest
    {
        public PrimitiveTest()
        {

        }
        [Fact(DisplayName ="测试int类型操作(有/无临时变量)")]
        public void ShowInt()
        {
            //测试int上限 无临时变量
            Delegate test = EHandler.CreateMethod<int>((il) => {
                EVar temp_var = int.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<int>("WriteLine",temp_var);
                temp_var.Load();
            }).Compile();
            Func<int> action = (Func<int>)test;
            Assert.Equal(int.MaxValue, action());

            //测试int下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<int>((il) => {
                EVar temp_var = EVar.CreateVarFromObject(int.MinValue); 
                EMethod.Load(typeof(Console)).ExecuteMethod<int>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<int> action2 = (Func<int>)test2;
            Assert.Equal(int.MinValue, action2());
        }
        [Fact(DisplayName = "测试uint类型操作(有/无临时变量)")]
        public void ShowUInt()
        {
            //测试uint上限 无临时变量
            Delegate test = EHandler.CreateMethod<uint>((il) => {
                EVar temp_var = uint.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<uint>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<uint> action = (Func<uint>)test;
            Assert.Equal(uint.MaxValue, action());

            //测试uint下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<uint>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(uint.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<uint>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<uint> action2 = (Func<uint>)test2;
            Assert.Equal(uint.MinValue, action2());
        }

        [Fact(DisplayName = "测试short类型操作(有/无临时变量)")]
        public void ShowShort()
        {
            //测试short上限 无临时变量
            Delegate test = EHandler.CreateMethod<short>((il) =>
            {
                EVar temp_var = short.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<short>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<short> action = (Func<short>)test;
            Assert.Equal(short.MaxValue, action());

            //测试short下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<short>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(short.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<short>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<short> action2 = (Func<short>)test2;
            Assert.Equal(short.MinValue, action2());
        }
        
        [Fact(DisplayName = "测试ushort类型操作(有/无临时变量)")]
        public void ShowUShort()
        {
            //测试ushort上限 无临时变量
            Delegate test = EHandler.CreateMethod<ushort>((il) => {
                EVar temp_var = ushort.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<ushort>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<ushort> action = (Func<ushort>)test;
            Assert.Equal(ushort.MaxValue, action());

            //测试short下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<ushort>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(ushort.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<ushort>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<ushort> action2 = (Func<ushort>)test2;
            Assert.Equal(ushort.MinValue, action2());
        }
        [Fact(DisplayName = "测试byte类型操作(有/无临时变量)")]
        public void ShowByte()
        {
            //测试byte上限 无临时变量
            Delegate test = EHandler.CreateMethod<byte>((il) =>
            {
                EVar temp_var = byte.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<byte>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<byte> action = (Func<byte>)test;
            Assert.Equal(byte.MaxValue, action());

            //测试byte下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<byte>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(byte.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<byte>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<byte> action2 = (Func<byte>)test2;
            Assert.Equal(byte.MinValue, action2());
        }
        [Fact(DisplayName = "测试sbyte类型操作(有/无临时变量)")]
        public void ShowSByte()
        {
            //测试sbyte上限 无临时变量
            Delegate test = EHandler.CreateMethod<sbyte>((il) =>
            {
                EVar temp_var = sbyte.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<sbyte>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<sbyte> action = (Func<sbyte>)test;
            Assert.Equal(sbyte.MaxValue, action());

            //测试sbyte下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<sbyte>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(sbyte.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<sbyte>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<sbyte> action2 = (Func<sbyte>)test2;
            Assert.Equal(sbyte.MinValue, action2());
        }
        [Fact(DisplayName = "测试long类型操作(有/无临时变量)")]
        public void ShowLong()
        {
            //测试long上限 无临时变量
            Delegate test = EHandler.CreateMethod<long>((il) =>
            {
                EVar temp_var = long.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<long>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<long> action = (Func<long>)test;
            Assert.Equal(long.MaxValue, action());

            //测试long下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<long>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(long.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<long>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<long> action2 = (Func<long>)test2;
            Assert.Equal(long.MinValue, action2());
        }
        [Fact(DisplayName = "测试ulong类型操作(有/无临时变量)")]
        public void ShowULong()
        {
            //测试ulong上限 无临时变量
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EVar temp_var = ulong.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<ulong> action = (Func<ulong>)test;
            Assert.Equal(ulong.MaxValue, action());

            //测试ulong下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<ulong>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(ulong.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<ulong>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<ulong> action2 = (Func<ulong>)test2;
            Assert.Equal(ulong.MinValue, action2());
        }
        [Fact(DisplayName = "测试double类型操作(有/无临时变量)")]
        public void ShowDouble()
        {
            //测试double上限 无临时变量
            Delegate test = EHandler.CreateMethod<double>((il) =>
            {
                EVar temp_var = double.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<double>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<double> action = (Func<double>)test;
            Assert.Equal(double.MaxValue, action());

            //测试double下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<double>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(double.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<double>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<double> action2 = (Func<double>)test2;
            Assert.Equal(double.MinValue, action2());
        }

        [Fact(DisplayName = "测试float类型操作(有/无临时变量)")]
        public void ShowFloat()
        {
            //测试float上限 无临时变量
            Delegate test = EHandler.CreateMethod<float>((il) =>
            {
                EVar temp_var = float.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<float>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<float> action = (Func<float>)test;
            Assert.Equal(float.MaxValue, action());

            //测试float下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<float>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(float.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<float>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<float> action2 = (Func<float>)test2;
            Assert.Equal(float.MinValue, action2());
        }
        [Fact(DisplayName = "测试string类型操作(有/无临时变量)")]
        public void ShowString()
        {
            //测试string 无临时变量
            Delegate test = EHandler.CreateMethod<string>((il) =>
            {
                EVar temp_var = string.Empty;
                EMethod.Load(typeof(Console)).ExecuteMethod<string>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<string> action = (Func<string>)test;
            Assert.Equal(string.Empty, action());

            //测试string 有临时变量
            Delegate test2 = EHandler.CreateMethod<string>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(string.Empty);
                EMethod.Load(typeof(Console)).ExecuteMethod<string>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<string> action2 = (Func<string>)test2;
            Assert.Equal(string.Empty, action2());
        }

        [Fact(DisplayName = "测试char类型操作(有/无临时变量)")]
        public void ShowChar()
        {
            //测试char 无临时变量
            Delegate test = EHandler.CreateMethod<char>((il) =>
            {
                EVar temp_var = 'a';
                EMethod.Load(typeof(Console)).ExecuteMethod<char>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<char> action = (Func<char>)test;
            Assert.Equal('a', action());

            //测试string 有临时变量
            Delegate test2 = EHandler.CreateMethod<char>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject('`');
                EMethod.Load(typeof(Console)).ExecuteMethod<char>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<char> action2 = (Func<char>)test2;
            Assert.Equal('`', action2());
        }
        [Fact(DisplayName = "测试decimal类型操作(有/无临时变量)")]
        public void ShowDecimal()
        {
            //测试float上限 无临时变量
            Delegate test = EHandler.CreateMethod<decimal>((il) =>
            {
                EVar temp_var = decimal.MaxValue;
                EMethod.Load(typeof(Console)).ExecuteMethod<decimal>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<decimal> action = (Func<decimal>)test;
            Assert.Equal(decimal.MaxValue, action());

            //测试float下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<decimal>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(decimal.MinValue);
                EMethod.Load(typeof(Console)).ExecuteMethod<decimal>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<decimal> action2 = (Func<decimal>)test2;
            Assert.Equal(decimal.MinValue, action2());
        }
        [Fact(DisplayName = "测试bool类型操作(有/无临时变量)")]
        public void ShowBoolean()
        {
            //测试float上限 无临时变量
            Delegate test = EHandler.CreateMethod<bool>((il) =>
            {
                EVar temp_var = true;
                EMethod.Load(typeof(Console)).ExecuteMethod<bool>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<bool> action = (Func<bool>)test;
            Assert.Equal(true, action());

            //测试float下限 有临时变量
            Delegate test2 = EHandler.CreateMethod<bool>((il) =>
            {
                EVar temp_var = EVar.CreateVarFromObject(false);
                EMethod.Load(typeof(Console)).ExecuteMethod<bool>("WriteLine", temp_var);
                temp_var.Load();
            }).Compile();
            Func<bool> action2 = (Func<bool>)test2;
            Assert.Equal(false, action2());
        }
    }

    [Trait("运算", "基元类型")]
    public class PrimitiveOperator
    {
        [Fact(DisplayName = "测试sbyte加减乘除运算(有/无临时变量)")]
        public void SByteOperator()
        {
            Delegate test = EHandler.CreateMethod<sbyte>((il) =>
            {
                EVar temp_var1 = (sbyte)15;
                EVar temp_var2 = EVar.CreateVarFromObject((sbyte)10);
                EVar temp_var3 = (sbyte)2;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 25);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<sbyte> action = (Func<sbyte>)test;
            Assert.Equal(sbyte.MaxValue, action());
        }
        [Fact(DisplayName = "测试byte加减乘除运算(有/无临时变量)")]
        public void ByteOperator()
        {
            Delegate test = EHandler.CreateMethod<byte>((il) =>
            {
                EVar temp_var1 = (byte)15;
                EVar temp_var2 = EVar.CreateVarFromObject((byte)10);
                EVar temp_var3 = (byte)5;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 50);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<byte> action = (Func<byte>)test;
            Assert.Equal(byte.MaxValue, action());
        }
        [Fact(DisplayName = "测试short加减乘除运算(有/无临时变量)")]
        public void ShortOperator()
        {
            Delegate test = EHandler.CreateMethod<short>((il) =>
            {
                EVar temp_var1 = (short)15;
                EVar temp_var2 = EVar.CreateVarFromObject((short)10);
                EVar temp_var3 = (short)2;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 6553);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<short> action = (Func<short>)test;
            Assert.Equal(short.MaxValue, action());
        }
        [Fact(DisplayName = "测试ushort加减乘除运算(有/无临时变量)")]
        public void UShortOperator()
        {
            Delegate test = EHandler.CreateMethod<ushort>((il) =>
            {
                EVar temp_var1 = (ushort)15;
                EVar temp_var2 = EVar.CreateVarFromObject((ushort)10);
                EVar temp_var3 = (ushort)0;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 13107);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<ushort> action = (Func<ushort>)test;
            Assert.Equal(ushort.MaxValue, action());
        }
        [Fact(DisplayName = "测试int加减乘除运算(有/无临时变量)")]
        public void IntOperator()
        {
            Delegate test = EHandler.CreateMethod<int>((il) =>
            {
                EVar temp_var1 = (int)15;
                EVar temp_var2 = EVar.CreateVarFromObject((int)10);
                EVar temp_var3 = (int)2;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 429496729);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<int> action = (Func<int>)test;
            Assert.Equal(int.MaxValue, action());
        }
        [Fact(DisplayName = "测试uint加减乘除运算(有/无临时变量)")]
        public void UIntOperator()
        {
            Delegate test = EHandler.CreateMethod<uint>((il) =>
            {
                EVar temp_var1 = (uint)15;
                EVar temp_var2 = EVar.CreateVarFromObject((uint)10);
                EVar temp_var3 = (uint)0;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / 5);
                temp_var2.Store(temp_var2 * 858993459);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<uint> action = (Func<uint>)test;
            Assert.Equal(uint.MaxValue, action());
        }
        [Fact(DisplayName = "测试long加减乘除运算(有/无临时变量)")]
        public void LongOperator()
        {
            Delegate test = EHandler.CreateMethod<long>((il) =>
            {
                EVar temp_var1 = (long)15;
                EVar temp_var2 = EVar.CreateVarFromObject((long)10);
                EVar temp_var3 = (long)2;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / (long)5);
                temp_var2.Store(temp_var2 * 1844674407370955161);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<long> action = (Func<long>)test;
            Assert.Equal(long.MaxValue, action());
        }
        [Fact(DisplayName = "测试ulong加减乘除运算(有/无临时变量)")]
        public void ULongOperator()
        {
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EVar temp_var1 = (ulong)15;
                EVar temp_var2 = EVar.CreateVarFromObject((ulong)10);
                EVar temp_var3 = (ulong)0;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / (ulong)5);
                temp_var2.Store(temp_var2 * 3689348814741910323);
                temp_var2.Store(temp_var2 + temp_var3);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<ulong> action = (Func<ulong>)test;
            Assert.Equal(ulong.MaxValue, action());
        }
        [Fact(DisplayName = "测试float加减乘除运算(有/无临时变量)")]
        public void FloatOperator()
        {
            Delegate test = EHandler.CreateMethod<float>((il) =>
            {
                EVar temp_var1 = (float)15;
                EVar temp_var2 = EVar.CreateVarFromObject((float)10);
                EVar temp_var3 = (float)3.5;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / (float)5);
                temp_var2.Store(temp_var2 - temp_var3);
                temp_var2.Store(temp_var2 * 3.0);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<float> action = (Func<float>)test;
            Assert.Equal((float)4.5, action());
        }
        [Fact(DisplayName = "测试double加减乘除运算(有/无临时变量)")]
        public void DoubleOperator()
        {
            Delegate test = EHandler.CreateMethod<double>((il) =>
            {
                EVar temp_var1 = (double)15;
                EVar temp_var2 = EVar.CreateVarFromObject((double)10);
                EVar temp_var3 = 3.5;
                temp_var2.Store(temp_var1 + temp_var2);
                temp_var2.Store(temp_var2 / (double)5);
                temp_var2.Store(temp_var2 - temp_var3);
                temp_var2.Store(temp_var2 * 3.0);
                temp_var2--;
                temp_var2++;
                temp_var2.Load();
            }).Compile();
            Func<double> action = (Func<double>)test;
            Assert.Equal(4.5, action());
        }

        [Fact(DisplayName = "测试string加运算(有/无临时变量)")]
        public void StringOperator()
        {
            Delegate test = EHandler.CreateMethod<string>((il) =>
            {
                EVar temp_var1 = "Hello ";
                EVar temp_var2 = EVar.CreateVarFromObject("World");
                (temp_var1 + temp_var2)();
            }).Compile();
            Func<string> action = (Func<string>)test;
            Assert.Equal("Hello World", action());
        }
    }
}
