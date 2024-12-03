namespace DebugSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NatashaManagement
                .GetInitializer()
                .WithMemoryUsing()
                .WithMemoryReference()
                .Preheating<NatashaDomainCreator>();

            AssemblyCSharpBuilder builder = new();
            builder.GetException();
            builder.UseRandomLoadContext();
            builder.UseSmartMode();
            //builder.WithFileOutput();
            builder.WithDebugCompile(debugger => debugger.ForAssembly());
            //builder.WithDebugCompile(debugger=>debugger.ForStandard());
            builder.Add(@"public static class A{ 
public static long Show(int i,short j,double z){  return (long)(i + j + z + 10); } 
public static long Show2(int i,short j,double z){  return (long)(i + j + z + 10); }
public static long Show3(int i,short j,double z){  return (long)(i + j + z + 10); }
public static long Show4(int i,short j,double z){  return (long)(i + j + z + 10); }
}");
            var action = builder
                .GetAssembly()
                .GetDelegateFromShortName<Func<int, short, double, long>>("A", "Show");
            var a = action(1,2,1.2);

            //CS0104Test();
            Console.ReadKey();
        }

        public static void CS0104Test()
        {
            AssemblyCSharpBuilder builder = new();
            builder.UseSmartMode();
            builder.AppendExceptUsings("System.IO");
            builder.WithDebugPlusCompile(debugger => debugger.ForStandard());
            builder.Add("public static class A{ public static void Show(int i,int j){ return i+j; } }");
            var action = builder.GetAssembly().GetDelegateFromShortName<Action>("A", "Show");
            action();
        }
    }
}
