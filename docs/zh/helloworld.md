
## 简介

Natasha 支持您在运行时进行脚本交互，创建动态方法，典型场景例如类型映射，远程调用。
如果您还没有用过此类的应用可以尝试一些例如 AutoMapper/Dapper/Json.net 之类的名库，
体验一下它们的快捷与方便，如果您继续深入探索便会发现一些 OpCodes 样式的代码,那便是用于动态构建的代码。

.NET允许在你程序运行时，根据逻辑再次生成另外一些功能，从技术角度来看，
程序运行的周期大致如下，C# 代码，被编译成IL代码，被转化成本地指令，
当您的程序开始运行时，IL 代码便开始起了作用，并且 .NET 虚拟机支持你在运行时再次注入 IL 代码，
这有点像插件，在运行时加载到程序中运行。  

<br/>  

## 场景

Emit 和表达式树的使用场景，Natasha 均适用。  


<br/>  

## 使用群体

首先来说本类库并不是为初学者准备的，而是需要有一定的封装基础，有一定的动态编程技巧的人。  
尽管 Natasha 入门十分简单，但如果您没有基础知识和经验的话还是不知道它能用在何处。  


<br/>  

## 版本通告

请使用 DotNetCore.Natasha.CSharp.All v2.0.0.0 整合稳定版。

<br/>  

## 准备工作


- 引入 打包好的动态构建库： DotNetCore.Natasha.CSharp.All  

- 初始化操作：

  ```C#
  //仅仅注册组件
  NatashaInitializer.Initialize();
  //注册+预热组件 , 之后编译会更加快速
  await NatashaInitializer.InitializeAndPreheating();
  ```
  
 - 敲代码  
 

<br/>  


## 第一个 HelloWorld

```C#

//使用 Natasha 的 CSharp 编译器直接编译字符串
AssemblyCSharpBuilder sharpBuilder = new AssemblyCSharpBuilder();

//给编译器指定一个随机域
sharpBuilder.Compiler.Domain = DomainManagement.Random;

//使用文件编译模式，动态的程序集将编译进入DLL文件中，当然了你也可以使用内存流模式。
sharpBuilder.UseFileCompile();

//如果代码编译错误，那么抛出并且记录日志。
sharpBuilder.ThrowAndLogCompilerError();
//如果语法检测时出错，那么抛出并记录日志，该步骤在编译之前。
sharpBuilder.ThrowAndLogSyntaxError();


//添加你的字符串
sharpBuilder.Add("using System; public static class Test{ public static void Show(){ Console.WriteLine(\"Hello World!\");}}");
//编译出一个程序集
var assembly = sharpBuilder.GetAssembly();


//如果你想直接获取到类型
var type = sharpBuilder.GetTypeFromShortName("Test");
type = sharpBuilder.GetTypeFromFullName("xxNamespace.xxClassName");
//同时还有
GetMethodFromShortName
GetMethodFromFullName
GetDelegateFromFullName
GetDelegateFromFullName<T>
GetDelegateFromShortName
GetDelegateFromShortName<T>


//创建一个 Action 委托
//必须在同一域内，因此指定域
//写调用脚本，把刚才的程序集扔进去，这样会自动添加using引用
var action = NDelegate.UseDomain(sharpBuilder.Compiler.Domain).Action("Test.Show();", assembly);

//运行，看到 Hello World!
action();

```

<br/>  

## 第二个 HelloWorld

```C#

//在 MyDomain 域内创建一个委托
var func = NDelegate.CreateDomain("MyDomain").Func<string>("return \"Hello World!\";");
Console.WriteLine(func());
func.DisposeDomain();

```

<br/>  

## 第三个 HelloWorld

```C#

//在随机域内创建一个自定义 Using 的委托,并将结果写入DLL
//后面的 Using 参数可以传 Assembly/Assembly[]/Type/Type[]/string/string[]
//例子中传了 typeof(string), 参数为可变参数，可以无限追加

var func = NDelegate.RandomDomain(
builder=>builder
  .CustomUsing()
  .UseFileCompile()
  ).Func<string>("return \"Hello World!\";", typeof(string));
Console.WriteLine(func());
func.DisposeDomain();

```  
```C#
//异步委托
public static async void Test()
{
    await NDelegate
           .RandomDomain()
           .AsyncFunc<Task>("Console.WriteLine(Thread.CurrentThread.ManagedThreadId);")();
}
```
<br/>  


## 第四个 HelloWorld

```C#

var @operator = FastMethodOperator.DefaultDomain();
var actionDelegate = @operator
                .Param(typeof(string), "parameter")
                .Body("Console.WriteLine(parameter);")
                .Compile();

//运行
actionDelegate.DynamicInvoke("HelloWorld!");
actionDelegate.DisposeDomain();

// 或者这样
var action = (Action<string>)actionDelegate;
action("HelloWorld!");
action.DisposeDomain();
```  

<br/>  


## 第五个 HelloWorld

```C#
//起个类
NClass nClass = NClass.DefaultDomain();
nClass
  .Namespace("MyNamespace")
  .Public()
  .Name("MyClass")
  .Ctor(ctor=>ctor.Public().Body("MyField=\"Hello\";"))
  .Property(prop => prop
    .Type(typeof(string))
    .Name("MyProperty")
    .Public()
    OnlyGetter("return \"World!\";")
  );


//添加方法（3.4.00之前的版本请参照上述属性的API）
MethodBuilder mb = new MethodBuilder();
mb
  .Public()
  .Override()
  .Name("ToString")
  .Body("return MyField+\" \"+MyProperty;")
  .Return(typeof(string));
 nClass.Method(mb);


//添加字段（3.4.00之前的版本请参照上述属性的API）
FieldBuilder fb = nClass.GetFieldBuilder();
fb.Public()
  .Name("MyField")
  .Type<string>();


//动态调用动态创建的类
var action = NDelegate
  .RandomDomain()
  .Action("Console.WriteLine((new MyClass()).ToString());", nClass.GetType());

action();
action.DisposeDomain();
```

## 第六个 HelloWorld

```C#

var hwFunc = FastMethodOperator
                .RandomDomain()
                .Param(typeof(string), "str1")
                .Param<string>("str2")
                .Body("return str1+str2;")
                .Return<string>()
                .Compile<Func<string, string, string>>();
Console.WriteLine(hwFunc("Hello"," World!"));

```
