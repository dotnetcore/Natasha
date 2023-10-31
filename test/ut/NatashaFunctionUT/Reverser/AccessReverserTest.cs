using Natasha.CSharp.Template.Reverser;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Reverser
{
    [Trait("基础功能测试", "反解器")]
    public class AccessReverserTest
    {
        public static readonly BindingFlags ReflectBindFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance;
        private static void Test(string access)
        {
            var eventInfo = typeof(AccessReverserTestModel).GetEvent(access + "Event", ReflectBindFlag)!;
            var fieldInfo = typeof(AccessReverserTestModel).GetField(access + "Field", ReflectBindFlag)!;
            var propertyInfo = typeof(AccessReverserTestModel).GetProperty(access + "Property", ReflectBindFlag)!;
            var methodInfo = typeof(AccessReverserTestModel).GetMethod(access + "Method", ReflectBindFlag)!;

            Assert.Equal(access, AccessReverser.GetAccess(fieldInfo).Replace(" ",""));
            Assert.Equal(access, AccessReverser.GetAccess(propertyInfo).Replace(" ", ""));
            Assert.Equal(access, AccessReverser.GetAccess(methodInfo).Replace(" ", ""));
            Assert.Equal(access, AccessReverser.GetAccess(eventInfo).Replace(" ", ""));

        }


        [Fact(DisplayName ="私有成员测试")]
        public void PrivateTest()
        {
            Test("private");
        }


        [Fact(DisplayName = "公有成员测试")]
        public void PublicTest()
        {
            Test("public");
        }


        [Fact(DisplayName = "保护成员测试")]
        public void ProtectedTest()
        {
            Test("protected");
        }


        [Fact(DisplayName = "内部成员测试")]
        public void InternalTest()
        {
            Test("internal");
        }


        [Fact(DisplayName = "内部保护成员测试")]
        public void InternalProtectedTest()
        {
            Test("internalprotected");
        }



        public class AccessReverserTestModel
        {
            private event Action? privateEvent;
            private string? privateField;
            private string? privateProperty { get; set; }
            private string? privateMethod() { return privateField; }

            public event Action? publicEvent;
            public string? publicField;
            public string? publicProperty { get; set; }
            public string? publicMethod() { return publicField; }

            protected event Action? protectedEvent;
            protected string? protectedField;
            protected string? protectedProperty { get; set; }
            protected string? protectedMethod() { return protectedField; }

            internal event Action? internalEvent;
            internal string? internalField;
            internal string? internalProperty { get; set; }
            internal string? internalMethod() { return internalField; }

            protected internal event Action? internalprotectedEvent;
            protected internal string? internalprotectedField;
            protected internal string? internalprotectedProperty { get; set; }
            protected internal string? internalprotectedMethod() { return protectedField; }
        }


    }
}
