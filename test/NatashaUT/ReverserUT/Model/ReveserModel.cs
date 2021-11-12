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


    public class MethodModel
    {
        public void M() { }
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
        public void M()
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


    public abstract class AbstractInterfaceMethodModel : InterfaceMethodModel
    {
        public abstract void M();
    }

    public class OverrideVirtualMethodModel : VirtualMethodModel
    {

        public override void M()
        {
            throw new NotImplementedException();
        }

    }

    public class OverrideAbstractMethodModel : AbstractMethodModel
    {
        public override void M()
        {
            throw new NotImplementedException();
        }
    }


    public class NewMethodModel : MethodModel
    {
        public new void M()
        {

        }
    }


    public class FFieldModel
    {
        public const int constA = 0;
        public readonly int readonlyA;

    }


    public class G1 {

        public void Test(string? name) { }
    
    }
    public abstract class G2 { }
    public interface G3 { }
    public interface G4 { }


    public class UnmanagedT<T> where T : unmanaged 
    {

    }
    public class StructT<T> where T: struct
    { 
    
    }
    public class ClassT<T> where T : class
    {

    }
    public class NewT<T> where T : new()
    {

    }
    public class ClassNewT<T> where T : class,new()
    {

    }
    public class NotNull<T> where T: notnull,new()
    { }


    public class ClassNewClassInterfaceT<T> where T : G2, G3, G4, new()
    {

    }

    public interface InInterfaceT<in T>
    {

    }

    public interface OutInterfaceT<out T>
    {

    }

    public interface InOutInterfaceT<in T, out S> where T : notnull, G2, G3, G4, new() where S : notnull, G2?, G3, G4, new()
    {

    }

}
