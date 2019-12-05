using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Natasha;
using NatashaUT.Model;

namespace NatashaUT
{

    [Trait("快速构建", "函数")]
    public class NDomainMethodTest
    {

        [Fact(DisplayName = "快速域函数1")]
        public static void RunDelegate1()
        {
            var func = NDomain.Create("NDomain1").Func<string>("return \"1\";");
            Assert.Equal("1", func());
        }




        [Fact(DisplayName = "快速域函数2")]
        public static void RunDelegate2()
        {
            //------创建一个域（方便卸载）----//-----创建Func方法--------//
            var func = NDomain.Create("NDomain2").Func<string,string>("return arg;");
            Assert.Equal("1", func("1"));
        }




        [Fact(DisplayName = "快速域函数3")]
        public static void RunDelegate3()
        {
            var func = NDomain.Create("NDomain3").Func<string,string, string>("return arg1+arg2;");
            Assert.Equal("12", func("1","2"));
        }



        //[Fact(DisplayName = "快速域函数7")]
        //public static void RunDelegate7()
        //{
        //    var func = NDomain.Create("NDomain7").Func<string>("return OtherNameSpaceMethod.FromDate(DateTime.Now);");
        //    Assert.Equal(DateTime.Now.ToString("yyyy-MM"), func());
        //}



        [Fact(DisplayName = "快速域函数4")]
        public static void RunDelegate4()
        {
            NormalTestModel model = new NormalTestModel();
            var func = NDomain.Create("NDomain4").Action<NormalTestModel>("obj.Age=1;");
            func(model);
            Assert.Equal(1, model.Age);
        }




        [Fact(DisplayName = "快速域函数5")]
        public static void RunDelegate5()
        {
            NormalTestModel model = new NormalTestModel();
            var func = NDomain.Create("NDomain5").Action<NormalTestModel,int>("arg1.Age=arg2;");
            func(model,1);
            Assert.Equal(1, model.Age);
        }




        [Fact(DisplayName = "快速域函数6")]
        public static void RunDelegate6()
        {
            NormalTestModel model = new NormalTestModel();
            var func = NDomain.Create("NDomain6").Action<NormalTestModel, int, int>("arg1.Age=arg2+arg3;");
            func(model,1,2);
            Assert.Equal(3, model.Age);
        }
    }
}
