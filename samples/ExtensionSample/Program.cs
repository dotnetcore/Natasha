namespace ExtensionSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();

            var func = "return arg1+arg2;"
                .WithAssemblyBuilder(opt=>opt.AddReferenceAndUsingCode<object>())
                .ToFunc<int, int, int>()!;

            Console.WriteLine(func(1,2));
            Console.ReadKey();
        }
    }
}
