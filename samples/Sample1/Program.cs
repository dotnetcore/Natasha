// See https://aka.ms/new-console-template for more information

//预热
NatashaManagement.Preheating();
NatashaManagement.AddGlobalUsing("System.IO");
NatashaManagement.AddGlobalReference(typeof(int));
var domain = NatashaManagement.CreateRandomDomain();

//创建一个动态的方法
var action = NDelegate
    .UseDomain(domain)
    .ConfigClass(item=>item
        .Name("myTestClass")
        .NoGlobalUsing() //不加载全局Using
        .NotLoadDomainUsing()) //不加载当前域编译产生的Using
    .Func<int, int, int>("return arg1+arg2;");
Console.WriteLine(DefaultUsing.UsingScript);
Console.WriteLine(action(1, 2));


//复用上面的类和方法
var func = NDelegate
    .UseDomain(domain)
    .Func<int>("return myTestClass.Invoke(3,4);");
Console.WriteLine(func());
Console.ReadKey();
