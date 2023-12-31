using Natasha.CSharp;


public static class NatashaOperator
{
    public static FastMethodOperator MethodOperator { get { return new(); } }
    public static NClass ClassOperator { get { return new(); } }
    public static NEnum EnumOperator { get { return new(); } }
    public static NInterface InterfaceOperator { get { return new(); } }
    public static NStruct StructOperator { get { return new(); } }
    public static NRecord RecordOperator { get { return new(); } }
    public static NDelegate DelegateOperator { get { return new(); } }
    public static AssemblyCSharpBuilder SharpBuilder { get { return new(); } }

}

