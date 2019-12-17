### 域内添加插件

```C#

//从指定域创建一个程序集操作实例
var assembly = domain.CreateAssembly("MyAssembly");


//向程序集中添加一段已经写好的类/结构体/接口/枚举
assembly.AddScript(@"using xxx; namespace xxx{xxxx}");
assembly.AddFile(@"Class1.cs");


//使用Natasha内置的操作类
assembly.CreateEnum(name=null);
assembly.CreateClass(name=null);
assembly.CreateStruct(name=null);
assembly.CreateInterface(name=null);


//使用Natasha内置的方法操作类
//并不是很推荐使用这两个方法
//建议在一个单独的程序集内编译方法 
assembly.CreateFastMethod(name=null);
assembly.CreateFakeMethod(name=null);


//使用程序集进行编译并获得程序集
var result = assembly.Complier();
//获取一个类型，注意这里的assembly不是上一步的result
assembly.GetType(name);

```
<br/>


### 程序集移除操作

```C#

            var domain = DomainManagment.Random;
            var type = NDomain.Create(domain).GetType("public class A{ public A(){Name=\"1\"; }public string Name;}");
            var func = NDomain.Create(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(func());  // result : 1

            type.RemoveReferences();  //如果不移除，下次引用A的时候会出现二义性
            type = NDomain.Create(domain).GetType("public class A{ public A(){Name=\"2\"; }public string Name;}");
            func = NDomain.Create(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(func());  // result : 2
```


### 程序域卸载

```C#

XXType.DisposeDomain();
XXAssembly.DisposeDomain();
XXDelegate.DisposeDomain();
三种方法调用之后都可以回收域。

```
