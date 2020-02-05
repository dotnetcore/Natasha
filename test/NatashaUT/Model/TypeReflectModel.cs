using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaUT.Model
{
    public class TypeReflectModel
    {

    }

    public class PublicTR
    {

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
    public readonly ref struct RPDSTRUCT{ }
    public class InheritanceTest: IPT, IPT2
    {

    }

    public class TFieldClass
    {
        public int A;
        protected int B;
        internal int C;
        private int D;
        protected internal int E;
        internal protected int F;
        public readonly int G;
        public static readonly int H;
    }

    public class TPropertyClass
    {
        public int A { get; set; }
        protected int B { get; set; }
        internal int C { get; set; }
        private int D { get; set; }
        protected internal int E { get; set; }
        internal protected int F { get; set; }
    }
}
