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

            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder.UseSmartMode();
            builder.WithDebugPlusCompile(debugger=>debugger.ForStandard());
            builder.Add("public static class A{ public static void Show(){  Console.WriteLine(1); } }");
            var action = builder.GetAssembly().GetDelegateFromShortName<Action>("A", "Show");
            action();

            //CS0104Test();
            Console.ReadKey();
        }

        public static void CS0104Test()
        {
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder.UseSmartMode();
            builder.AppendExceptUsings("System.IO");
            builder.WithDebugPlusCompile(debugger => debugger.ForStandard());
            builder.Add("public static class A{ public static void Show(){ Console.WriteLine(File.Exists(\"1\")); } }");
            var action = builder.GetAssembly().GetDelegateFromShortName<Action>("A", "Show");
            action();
        }
    }
}
