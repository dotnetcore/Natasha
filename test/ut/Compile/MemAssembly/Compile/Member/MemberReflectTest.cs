using MemAssembly.Compile.Method.Utils;
using System;

namespace MemAssembly.Compile
{
    [Trait("基础编译(REF)", "成员及反射")]
    public class MemberReflectTest : CompilePrepareBase
    {

        [Fact(DisplayName = "字段")]
        public void FieldTest()
        {
            var type = MemberHelper.GetType("public string Name=\"HelloWorld\";");
            var info = type.GetField("Name");

            Assert.NotNull(info);
            Assert.Equal("A", type.Name);
            Assert.Equal("HelloWorld", info!.GetValue(Activator.CreateInstance(type)));
        }

        [Fact(DisplayName = "属性")]
        public void PropertyTest()
        {
            var type = MemberHelper.GetType("public string Name{get;set;} = \"HelloWorld\";");
            var info = type.GetProperty("Name");

            Assert.NotNull(info);
            Assert.Equal("A", type.Name);
            Assert.Equal("HelloWorld", info!.GetValue(Activator.CreateInstance(type)));
        }


        [Fact(DisplayName = "委托")]
        public void MethodTest()
        {
            var assembly = MemberHelper.GetAssembly("public string Name=\"HelloWorld\"; public static string Get(){  return (new A()).Name; }", "compileUntiTestFoDelegateAssembly");
            var func = assembly.GetDelegateFromShortName<Func<string>>("A", "Get");
            Assert.Equal("compileUntiTestFoDelegateAssembly", func.Method.Module.Assembly.GetName().Name!);
            Assert.Equal("HelloWorld", func());
        }

    }
}
