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
