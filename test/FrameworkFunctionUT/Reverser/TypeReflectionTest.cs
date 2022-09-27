using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace FrameworkFunctionUT.Reverser
{

    [Trait("基础功能测试", "反解器")]
    public class TypeReflectionTest
    {
        [Fact(DisplayName = "共有类反射测试")]
        public void Test1()
        {

            Assert.True(typeof(PublicTR).IsPublic);

            Assert.False(typeof(PublicTR).IsSealed);
            Assert.False(typeof(PublicTR).IsAbstract);
            Assert.False(typeof(PublicTR).IsValueType);
            Assert.False(typeof(PublicTR).IsInterface);
            Assert.False(typeof(PublicTR).IsNested);
            Assert.False(typeof(PublicTR).IsNestedAssembly);
            Assert.False(typeof(PublicTR).IsNestedFamANDAssem);
            Assert.False(typeof(PublicTR).IsNestedFamily);
            Assert.False(typeof(PublicTR).IsNestedPrivate);
            Assert.False(typeof(PublicTR).IsNestedFamORAssem);

        }

        [Fact(DisplayName = "密封类反射测试")]
        public void Test6()
        {

            Assert.True(typeof(PSTR).IsPublic);
            Assert.True(typeof(PSTR).IsSealed);


            Assert.False(typeof(PSTR).IsAbstract);
            Assert.False(typeof(PSTR).IsValueType);
            Assert.False(typeof(PSTR).IsInterface);
            Assert.False(typeof(PSTR).IsNested);
            Assert.False(typeof(PSTR).IsNestedAssembly);
            Assert.False(typeof(PSTR).IsNestedFamANDAssem);
            Assert.False(typeof(PSTR).IsNestedFamily);
            Assert.False(typeof(PSTR).IsNestedPrivate);
            Assert.False(typeof(PSTR).IsNestedFamORAssem);

        }


        [Fact(DisplayName = "抽象类反射测试")]
        public void Test7()
        {

            Assert.True(typeof(PATR).IsAbstract);
            Assert.True(typeof(PATR).IsPublic);

            Assert.False(typeof(PATR).IsNested);
            Assert.False(typeof(PATR).IsSealed);
            Assert.False(typeof(PATR).IsValueType);
            Assert.False(typeof(PATR).IsInterface);
            Assert.False(typeof(PATR).IsNestedAssembly);
            Assert.False(typeof(PATR).IsNestedFamANDAssem);
            Assert.False(typeof(PATR).IsNestedFamily);
            Assert.False(typeof(PATR).IsNestedPrivate);
            Assert.False(typeof(PATR).IsNestedFamORAssem);

        }


        [Fact(DisplayName = "静态类反射测试")]
        public void Test8()
        {

            Assert.True(typeof(STR).IsPublic);
            Assert.True(typeof(STR).IsSealed);
            Assert.True(typeof(STR).IsAbstract);

            Assert.False(typeof(STR).IsValueType);
            Assert.False(typeof(STR).IsInterface);
            Assert.False(typeof(STR).IsNested);
            Assert.False(typeof(STR).IsNestedAssembly);
            Assert.False(typeof(STR).IsNestedFamANDAssem);
            Assert.False(typeof(STR).IsNestedFamily);
            Assert.False(typeof(STR).IsNestedPrivate);
            Assert.False(typeof(STR).IsNestedFamORAssem);

        }


        [Fact(DisplayName = "接口类反射测试")]
        public void Test9()
        {

            Assert.True(typeof(IPT).IsPublic);
            Assert.True(typeof(IPT).IsInterface);
            Assert.True(typeof(IPT).IsAbstract);


            Assert.False(typeof(IPT).IsSealed);
            Assert.False(typeof(IPT).IsValueType);
            Assert.False(typeof(IPT).IsNested);
            Assert.False(typeof(IPT).IsNestedAssembly);
            Assert.False(typeof(IPT).IsNestedFamANDAssem);
            Assert.False(typeof(IPT).IsNestedFamily);
            Assert.False(typeof(IPT).IsNestedPrivate);
            Assert.False(typeof(IPT).IsNestedFamORAssem);

        }


        [Fact(DisplayName = "枚举类反射测试")]
        public void Test10()
        {

            Assert.True(typeof(EPT).IsPublic);
            Assert.True(typeof(EPT).IsEnum);
            Assert.True(typeof(EPT).IsSealed);
            Assert.True(typeof(EPT).IsValueType);

            Assert.False(typeof(EPT).IsAbstract);
            Assert.False(typeof(EPT).IsInterface);
            Assert.False(typeof(EPT).IsNested);
            Assert.False(typeof(EPT).IsNestedAssembly);
            Assert.False(typeof(EPT).IsNestedFamANDAssem);
            Assert.False(typeof(EPT).IsNestedFamily);
            Assert.False(typeof(EPT).IsNestedPrivate);
            Assert.False(typeof(EPT).IsNestedFamORAssem);

        }

        [Fact(DisplayName = "普通类反射测试")]
        public void Test2()
        {

            Assert.True(typeof(InternalTR).IsNotPublic);

            Assert.False(typeof(InternalTR).IsPublic);
            Assert.False(typeof(InternalTR).IsSealed);
            Assert.False(typeof(InternalTR).IsAbstract);
            Assert.False(typeof(InternalTR).IsValueType);
            Assert.False(typeof(InternalTR).IsInterface);
            Assert.False(typeof(InternalTR).IsNested);
            Assert.False(typeof(InternalTR).IsNestedAssembly);
            Assert.False(typeof(InternalTR).IsNestedFamANDAssem);
            Assert.False(typeof(InternalTR).IsNestedFamily);
            Assert.False(typeof(InternalTR).IsNestedPrivate);
            Assert.False(typeof(InternalTR).IsNestedFamORAssem);

        }


        [Fact(DisplayName = "内部保护类反射测试")]
        public void Test3()
        {

            Assert.True(typeof(PPTR).IsNested);
            Assert.True(typeof(PPTR).IsNestedFamily);


            Assert.False(typeof(PPTR).IsPublic);
            Assert.False(typeof(PPTR).IsNotPublic);
            Assert.False(typeof(PPTR).IsSealed);
            Assert.False(typeof(PPTR).IsAbstract);
            Assert.False(typeof(PPTR).IsValueType);
            Assert.False(typeof(PPTR).IsInterface);
            Assert.False(typeof(PPTR).IsNestedAssembly);
            Assert.False(typeof(PPTR).IsNestedFamANDAssem);
            Assert.False(typeof(PPTR).IsNestedPrivate);
            Assert.False(typeof(PPTR).IsNestedFamORAssem);

        }

        [Fact(DisplayName = "内部私有类反射测试")]
        public void Test4()
        {

            Assert.True(typeof(PPrivateTR).IsNested);
            Assert.True(typeof(PPrivateTR).IsNestedPrivate);

            Assert.False(typeof(PPrivateTR).IsPublic);
            Assert.False(typeof(PPrivateTR).IsNotPublic);
            Assert.False(typeof(PPrivateTR).IsSealed);
            Assert.False(typeof(PPrivateTR).IsAbstract);
            Assert.False(typeof(PPrivateTR).IsValueType);
            Assert.False(typeof(PPrivateTR).IsInterface);
            Assert.False(typeof(PPrivateTR).IsNestedAssembly);
            Assert.False(typeof(PPrivateTR).IsNestedFamANDAssem);
            Assert.False(typeof(PPrivateTR).IsNestedFamily);
            Assert.False(typeof(PPrivateTR).IsNestedFamORAssem);

        }

        

        [Fact(DisplayName = "内部程序集内友元类反射测试")]
        public void Test15()
        {

            Assert.True(typeof(OITR).IsNested);
            Assert.True(typeof(OITR).IsNestedAssembly);

            Assert.False(typeof(OITR).IsNestedFamANDAssem);
            Assert.False(typeof(OITR).IsPublic);
            Assert.False(typeof(OITR).IsNotPublic);
            Assert.False(typeof(OITR).IsSealed);
            Assert.False(typeof(OITR).IsAbstract);
            Assert.False(typeof(OITR).IsValueType);
            Assert.False(typeof(OITR).IsInterface);
            Assert.False(typeof(OITR).IsNestedFamORAssem);
            Assert.False(typeof(OITR).IsNestedFamily);
            Assert.False(typeof(OITR).IsNestedPrivate);

        }

        [Fact(DisplayName = "内部友元类反射测试")]
        public void Test5()
        {

            Assert.True(typeof(IIPTR).IsNested);
            Assert.True(typeof(IIPTR).IsNestedFamORAssem);

            Assert.False(typeof(IIPTR).IsPublic);
            Assert.False(typeof(IIPTR).IsNotPublic);
            Assert.False(typeof(IIPTR).IsSealed);
            Assert.False(typeof(IIPTR).IsAbstract);
            Assert.False(typeof(IIPTR).IsValueType);
            Assert.False(typeof(IIPTR).IsInterface);
            Assert.False(typeof(IIPTR).IsNestedAssembly);
            Assert.False(typeof(IIPTR).IsNestedFamANDAssem);
            Assert.False(typeof(IIPTR).IsNestedFamily);
            Assert.False(typeof(IIPTR).IsNestedPrivate);
            
        }



        [Fact(DisplayName = "只读结构体类反射测试")]
        public void Test11()
        {


            Assert.True(typeof(RDSTRUCT).IsPublic);
            //Assert.NotNull(typeof(RDSTRUCT).GetCustomAttribute<IsReadOnlyAttribute>());
            Assert.True(typeof(RDSTRUCT).IsSealed);
            Assert.True(typeof(RDSTRUCT).IsValueType);


            Assert.False(typeof(RDSTRUCT).IsNotPublic);
            Assert.False(typeof(RDSTRUCT).IsAbstract);
            Assert.False(typeof(RDSTRUCT).IsInterface);
            Assert.False(typeof(RDSTRUCT).IsNested);
            Assert.False(typeof(RDSTRUCT).IsNestedAssembly);
            Assert.False(typeof(RDSTRUCT).IsNestedFamANDAssem);
            Assert.False(typeof(RDSTRUCT).IsNestedFamily);
            Assert.False(typeof(RDSTRUCT).IsNestedPrivate);
            Assert.False(typeof(RDSTRUCT).IsNestedFamORAssem);
            Assert.False(typeof(RDSTRUCT).IsByRef);
            //Assert.False(typeof(RDSTRUCT).IsByRefLike);
            Assert.False(typeof(RDSTRUCT).IsMarshalByRef);
            
        }
        [Fact(DisplayName = "栈上只读结构体类反射测试")]
        public void Test14()
        {

            //Assert.NotNull(typeof(RPDSTRUCT).GetCustomAttribute<IsReadOnlyAttribute>());
            Assert.True(typeof(RPDSTRUCT).IsPublic);
            Assert.True(typeof(RPDSTRUCT).IsValueType);
            Assert.True(typeof(RPDSTRUCT).IsSealed);
            //Assert.True(typeof(RPDSTRUCT).IsByRefLike);

            Assert.False(typeof(RPDSTRUCT).IsNotPublic);
            Assert.False(typeof(RPDSTRUCT).IsAbstract);
            Assert.False(typeof(RPDSTRUCT).IsInterface);
            Assert.False(typeof(RPDSTRUCT).IsNested);
            Assert.False(typeof(RPDSTRUCT).IsNestedAssembly);
            Assert.False(typeof(RPDSTRUCT).IsNestedFamANDAssem);
            Assert.False(typeof(RPDSTRUCT).IsNestedFamily);
            Assert.False(typeof(RPDSTRUCT).IsNestedPrivate);
            Assert.False(typeof(RPDSTRUCT).IsNestedFamORAssem);
            Assert.False(typeof(RPDSTRUCT).IsByRef);
            Assert.False(typeof(RPDSTRUCT).IsMarshalByRef);

        }
        [Fact(DisplayName = "普通结构体类反射测试")]
        public void Test12()
        {
            //Assert.Null(typeof(PSTRUCT).GetCustomAttribute<IsReadOnlyAttribute>());
            Assert.True(typeof(PSTRUCT).IsPublic);
            Assert.True(typeof(PSTRUCT).IsSealed);
            Assert.True(typeof(PSTRUCT).IsValueType);

            Assert.False(typeof(PSTRUCT).IsNotPublic);
            Assert.False(typeof(PSTRUCT).IsAbstract);
            Assert.False(typeof(PSTRUCT).IsInterface);
            Assert.False(typeof(PSTRUCT).IsNested);
            Assert.False(typeof(PSTRUCT).IsNestedAssembly);
            Assert.False(typeof(PSTRUCT).IsNestedFamANDAssem);
            Assert.False(typeof(PSTRUCT).IsNestedFamily);
            Assert.False(typeof(PSTRUCT).IsNestedPrivate);
            Assert.False(typeof(PSTRUCT).IsNestedFamORAssem);
            Assert.False(typeof(RDSTRUCT).IsByRef);
            //Assert.False(typeof(RDSTRUCT).IsByRefLike);
            Assert.False(typeof(RDSTRUCT).IsMarshalByRef);
        }




        [Fact(DisplayName = "引用结构体类反射测试")]
        public void Test13()
        {

            //Assert.Null(typeof(RPSTRUCT).GetCustomAttribute<IsReadOnlyAttribute>());
            Assert.True(typeof(RPSTRUCT).IsPublic);
            Assert.True(typeof(RPSTRUCT).IsSealed);
            Assert.True(typeof(RPSTRUCT).IsValueType);
            //Assert.True(typeof(RPSTRUCT).IsByRefLike);

            Assert.False(typeof(RPSTRUCT).IsNotPublic);
            Assert.False(typeof(RPSTRUCT).IsAbstract);
            Assert.False(typeof(RPSTRUCT).IsInterface);
            Assert.False(typeof(RPSTRUCT).IsNested);
            Assert.False(typeof(RPSTRUCT).IsNestedAssembly);
            Assert.False(typeof(RPSTRUCT).IsNestedFamANDAssem);
            Assert.False(typeof(PSTRUCT).IsNestedFamily);
            Assert.False(typeof(RPSTRUCT).IsNestedPrivate);
            Assert.False(typeof(RPSTRUCT).IsNestedFamORAssem);
            Assert.False(typeof(RPSTRUCT).IsByRef);
            Assert.False(typeof(RPSTRUCT).IsMarshalByRef);

        }

        protected class PPTR { }
        private class PPrivateTR { }
        internal protected class IIPTR { }
        internal class OITR
        {

            protected class OIPTR { }
        }

    }





    internal class InternalTR
    {


    }

    public sealed class PSTR { }
    public abstract class PATR { }
    public static class STR { }
    public interface IPT { }
    public interface IPT2 { }
    public enum EPT { }
    public readonly struct RDSTRUCT { }
    public struct PSTRUCT { }
    public ref struct RPSTRUCT { }
    public readonly ref struct RPDSTRUCT { }
    public class InheritanceTest : IPT, IPT2
    {

    }

    public class PublicTR
    {

    }
}
