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

    public class InheritanceTest: IPT, IPT2
    {

    }
}
