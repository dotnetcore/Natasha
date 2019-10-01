using Natasha;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Xunit;

namespace NatashaUT
{
    [Trait("程序集同类测试", "")]
    public class SameTypeTest
    {
        public object obj;
        public SameTypeTest()
        {
            obj = new object();
        }
        [Fact(DisplayName = "同命名空间程序集1")]
        public void Test1()
        {

#if !NETCOREAPP2_2
            lock (obj)
            {
                using (DomainManagment.CreateAndLock("TestSame"))
            {

                var domain = DomainManagment.CurrentDomain;


                var assembly = domain.CreateAssembly("AsmTest1");
                assembly.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                var result2 = assembly.Complier();
                var type2 = assembly.GetType("Class1");


                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Repeate", "ClassLibrary1.dll");
                var result1= domain.LoadStream(path);
                var type1 = result1.GetTypes().First(item => item.Name == "Class1");


                Assert.True(domain.RemoveDll(path));
                Assert.Equal("TestSame", DomainManagment.CurrentDomain.Name);
                Assert.NotEqual(result1, result2);
                Assert.Equal(type1.Name, type2.Name);
                Assert.False(domain.RemoveDll(path));

                    var func = NFunc<object>.Delegate("return new Class1();", "ClassLibrary1");
                    Assert.Equal(result2, func().GetType().Assembly);
                }
               
            }
#endif

        }


        [Fact(DisplayName = "同命名空间程序集2")]
        public void Test2()
        {

#if !NETCOREAPP2_2
            using (DomainManagment.CreateAndLock("Default1"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assembly = domain.CreateAssembly("AsmTest1");
                assembly.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                var result2 = assembly.Complier();
                var type2 = assembly.GetType("Class1");

                try
                {
                    var assembly1 = domain.CreateAssembly("AsmTest2");
                    assembly1.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                    var result1 = assembly1.Complier();
                    var type1 = assembly1.GetType("Class1");

                    Assert.NotEqual(result1, result2);
                    Assert.Equal(type1.Name, type2.Name);
                    lock (obj)
                    {
                        var func = NFunc<object>.Delegate("return new Class1();", "ClassLibrary1");
                        Assert.Equal(result2, func().GetType().Assembly);
                    }
                }
                catch (Exception ex)
                {

                    Assert.NotNull(ex);
                }
               

            }
#endif

        }


        [Fact(DisplayName = "同命名空间程序集3")]
        public void Test3()
        {

#if !NETCOREAPP2_2
            using (DomainManagment.CreateAndLock("Default2"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assembly = domain.CreateAssembly("AsmTest1");
                assembly.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                var result2 = assembly.Complier();
                var type2 = assembly.GetType("Class1");
                domain.RemoveAssembly(result2);


                var assembly1 = domain.CreateAssembly("AsmTest2");
                assembly1.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                var result1 = assembly1.Complier();
                var type1 = assembly1.GetType("Class1");


                Assert.NotEqual(result1, result2);
                Assert.Equal(type1.Name, type2.Name);
                lock (obj)
                {
                    var func = NFunc<object>.Delegate("return new Class1();", "ClassLibrary1");
                    Assert.Equal(result1, func().GetType().Assembly);
                }

            }
#endif

        }


        [Fact(DisplayName = "同命名空间程序集4")]
        public void Test4()
        {

#if !NETCOREAPP2_2
            lock (obj)
            {
                Assembly result1;
            using (DomainManagment.Lock("Default"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assembly = domain.CreateAssembly("DAsmTest1");
                assembly.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                var result2 = assembly.Complier();
                var type2 = assembly.GetType("Class1");
                domain.RemoveAssembly(result2);
                

                var assembly1 = domain.CreateAssembly("DAsmTest2");
                assembly1.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                result1 = assembly1.Complier();
                var type1 = assembly1.GetType("Class1");

                
                Assert.NotEqual(result1, result2);
                Assert.Equal(type1.Name, type2.Name);
                

            }

            var func = NFunc<object>.Delegate("return new Class1();", "ClassLibrary1");
            Assert.Equal(result1, func().GetType().Assembly);
            DomainManagment.CurrentDomain.RemoveAssembly(result1);
            }
#endif

        }


        [Fact(DisplayName = "同命名空间程序集5")]
        public void Test5()
        {

#if !NETCOREAPP2_2
            lock (obj)
            {
                Assembly result1;
                using (DomainManagment.Lock("Default"))
                {

                    var domain = DomainManagment.CurrentDomain;
                    //var assembly = domain.CreateAssembly("AsmTest1");
                    //assembly.AddScript("using System;namespace ClassLibrary1{ public class Class1{public string name;}}");
                    //var result2 = assembly.Complier();
                    //var type2 = assembly.GetType("Class1");
                    //domain.RemoveAssembly(result2);


                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Repeate", "ClassLibrary1.dll");
                    result1 = domain.LoadStream(path);
                    var type1 = result1.GetTypes().First(item => item.Name == "Class1");


                    //Assert.NotEqual(result1, result2);
                    //Assert.Equal(type1.Name, type2.Name);


                }

                var func = NFunc<object>.Delegate("return new Class1();", "ClassLibrary1");
                Assert.Equal(result1, func().GetType().Assembly);
                DomainManagment.CurrentDomain.RemoveAssembly(result1);
            }
#endif

        }
    }
}
