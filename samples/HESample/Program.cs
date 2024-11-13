using HESample1;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Linq.Expressions;
using System.Reflection.Emit;
bool ab(string a) {
    return a == "b";
}
Console.WriteLine(ab(" c"));
var b = 1;
NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
Action a = () =>
{
     b++;
    //DS "AA"
    var ba = () => {
        //DS "bb" 
        //DS "CC"
    };
    ba();
    //DS 1==1
};

Console.ReadKey();

DynamicMethod dynamicMethod = new DynamicMethod("FloorDivMethod", typeof(double), new Type[] { typeof(double) }, typeof(Program).Module);

ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
ilGenerator.Emit(OpCodes.Ldarg_0);
ilGenerator.Emit(OpCodes.Ldc_R8, 0.3);
ilGenerator.Emit(OpCodes.Div);
ilGenerator.Emit(OpCodes.Call, typeof(Math).GetMethod("Floor", new Type[] { typeof(double) }));
ilGenerator.Emit(OpCodes.Ret);

Func<double, double> floorDivMethodEmit = (Func<double, double>)dynamicMethod.CreateDelegate(typeof(Func<double, double>));
Console.WriteLine(floorDivMethodEmit(5));

Console.ReadKey();

ParameterExpression valueParameter = Expression.Parameter(typeof(double), "value");

Expression divisionExpression = Expression.Divide(valueParameter, Expression.Constant(0.3));
Expression floorExpression = Expression.Call(typeof(Math), "Floor", null, divisionExpression);
Expression<Func<double, double>> expression = Expression.Lambda<Func<double, double>>(floorExpression, valueParameter);

Func<double, double> floorDivMethod = expression.Compile();

Console.WriteLine(floorDivMethod(5));

Console.ReadKey();


AssemblyCSharpBuilder builder = new();
var func = builder
    .UseRandomLoadContext()
    .UseSimpleMode()
    .ConfigLoadContext(ctx => ctx
        .AddReferenceAndUsingCode(typeof(Math))
        .AddReferenceAndUsingCode(typeof(double))
        )
    .Add("public static class A{ public static double Invoke(double value){ return Math.Floor(value/0.3);  }}")
    .GetAssembly()
    .GetDelegateFromShortName<Func<double, double>>("A", "Invoke");
Console.WriteLine(func(5));

Console.ReadKey();

NatashaManagement
    .GetInitializer()
    .WithMemoryUsing()
    //.WithRefUsing()
    //.WithMemoryReference()
    //.WithRefReference();
    .WithExcludeReferences((asm, @namespace) => !string.IsNullOrWhiteSpace(@namespace) && @namespace.StartsWith("Microsoft.VisualBasic"))
    .Preheating<NatashaDomainCreator>();

var simpleFunc = "return Math.Floor(arg1/0.3);"
    .WithMetadata(typeof(Math))
    .ToFunc<double, double>();

 simpleFunc = "return Math.Floor(arg1/0.3);"
    .WithoutUsings("MyMath")
    .ToFunc<double, double>();

Console.WriteLine(simpleFunc(5));

Console.ReadKey();
void Show()
{

    Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
    Console.WriteLine(AppContext.BaseDirectory);
    Console.WriteLine("s1");
    Console.WriteLine("s2");
    //CSharpScript.Create<Action>("a");
    Console.WriteLine("s3");
    Console.WriteLine("s4");
    Console.WriteLine("s5");
    Console.WriteLine("s6");

    var pExit = typeof(AppDomain).GetField("DomainUnload");
    var dels = ((Delegate)pExit?.GetValue(AppDomain.CurrentDomain))?.GetInvocationList();
    //DS dels == null
    //DS dels == null
    if (dels != null)
    {
        Console.WriteLine(dels.Length);
        foreach (var item in dels)
        {
            if (item != null)
            {
                AppDomain.CurrentDomain.DomainUnload -= (EventHandler)item;
            }
        }
    }


    for (global::System.Int32 i = 0; i < 16; i++)
    {
        GC.Collect();
        Thread.Sleep(200);
    }

    GC.WaitForPendingFinalizers();
    foreach (var item in DomainManagement.Cache)
    {
        if (!DomainManagement.IsDeleted(item.Key))
        {
            Console.WriteLine(item.Key + "没有被释放！");
        }
    }
    Show2();
    void Show2()
    {
        //DS "Show2"
    }

    //DS "Tail"
}


void CurrentDomain_DomainUnload(object? sender, EventArgs e)
{
    Console.WriteLine("aaa");
}
a();


Show();
await Task.Delay(2000);
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!2322");
Class1.Show();

Console.ReadKey();