using Natasha.CSharp.Reverser;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT.ReverserUT
{

    [Trait("反解器", "访问控制")]
    public class AccessReverserUT
    {

        private void TestEqual(string memberName, string? key = null)
        {
            var field = typeof(TFieldClass).GetField(memberName + "A", 
                System.Reflection.BindingFlags.NonPublic 
                | System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.Instance);
            var method = typeof(TMethodClass).GetMethod(memberName + "A", 
                System.Reflection.BindingFlags.NonPublic 
                | System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.Instance);

            if (key == null)
            {
                key = memberName;
            }
            Assert.Equal(key + " ", AccessReverser.GetAccess(field!));
            Assert.Equal(key + " ", AccessReverser.GetAccess(method!));
        }


        [Fact(DisplayName = "公有")]
        public void PublicTest()
        {

            TestEqual("public");

        }


        [Fact(DisplayName = "私有")]
        public void PrivateTest()
        {
            TestEqual("private");
        }


        [Fact(DisplayName = "保护")]
        public void ProtectedTest()
        {
            TestEqual("private");
        }

        [Fact(DisplayName = "程序集内")]
        public void InternalTest()
        {
            TestEqual("internal");
        }

        [Fact(DisplayName = "程序集内保护")]
        public void InternalProtectedTest()
        {
            TestEqual("internalprotected", "internal protected");
        }
        [Fact(DisplayName = "程序集内保护")]
        public void ProtectedInternalTest()
        {
            TestEqual("protectedinternal", "internal protected");
        }
    }
}
