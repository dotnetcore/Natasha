using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaUT.Model
{
    public class ReveserModel
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
        public int publicA;
        protected int protectedA;
        internal int internalA;
        private int privateA;
        protected internal int protectedinternalA;
        internal protected int internalprotectedA;
        public readonly int G;
        public static readonly int H;
    }

    public class TPropertyClass
    {
        public int publicA { get; set; }
        protected int protectedA { get; set; }
        internal int internalA { get; set; }
        private int privateA { get; set; }
        protected internal int E { get; set; }
        internal protected int F { get; set; }
        public int GetD { get { return privateA; } }
    }

    public class TMethodClass 
    {

        public int publicA() { return default; }
        protected int protectedA() { return default; }
        internal int internalA() { return default; }
        private int privateA() { return default; }
        protected internal int protectedinternalA() { return default; }
        internal protected int internalprotectedA() { return default; }

    }


    public class MFieldClass
    {
        public static int staticA;
        public readonly int readonlyA;
        public const int constA = 1;
    }



    public class VirtualMethodModel
    {
        public virtual void M() { }
    }

    public interface InterfaceMethodModel
    {
        void M();
    }


    public abstract class AbstractMethodModel
    {
        public abstract void M();
    }


    public class ImplementMethodModel : InterfaceMethodModel
    {
        public virtual void M()
        {
            throw new NotImplementedException();
        }
    }

    public class VirtualInterfaceMethodModel : InterfaceMethodModel
    {
        public virtual void M()
        {
            throw new NotImplementedException();
        }
    }

    public class OverrideVirtualMethodModel : VirtualMethodModel
    {
        public override void M()
        {
            throw new NotImplementedException();
        }
    }

    public class OverrideMethodModel : AbstractMethodModel
    {
        public override void M()
        {
            throw new NotImplementedException();
        }
    }


    public class NewMethodModel : ImplementMethodModel
    {
        public new void M()
        {

        }
    }

}
