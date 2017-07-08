using Natasha;
using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT
{
    [Trait("数组", "遍历")]
    public class ArrayTest
    {
        [Fact(DisplayName = "测试整形数组叠加操作")]
        public void TestInt()
        {
            int[] testArray = new int[] { 1, 2, 3, 4, 5 };
            Delegate ShowDelegate = EHandler.CreateMethod<int>((il) =>
            {
                EVar result = EVar.CreateVar<int>();
                EArray Model = testArray;
                ELoop.For(Model, (loadCurrentElement) =>
                {
                    result.Store(result + loadCurrentElement);
                });
                result.Load();
            }).Compile();
            Assert.Equal(15, ((Func<int>)ShowDelegate)());
        }
        [Fact(DisplayName ="测试字符串数组输出操作")]
        public void TestString()
        {
            string[] testArray = new string[] {"0", "1", "2", "3", "4", "5" };
            for (int i = 0; i < testArray.Length; i+=1)
            {
                Delegate ShowDelegate = EHandler.CreateMethod<string>((il) =>
                {
                    EArray Model = testArray;
                    Model.LoadArray(i);
                }).Compile();
                Assert.Equal(i.ToString(), ((Func<string>)ShowDelegate)());
            }
        }
    }
}
