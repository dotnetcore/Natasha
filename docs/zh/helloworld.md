## 让我们开始吧


1、引入 DotNetCore.Natasha 库  

2、引入 编译环境库 ： DotNetCore.Compile.Environment  

3、向引擎中注入定制的域：  DomainManagement.RegisterDefault< AssemblyDomain >(); 

4、 敲代码：

```C#

//引擎开放之后，您可以向引擎中注入自己实现的域，这里的 AssemblyDomain 是 Natasha 实现的域
DomainManagement.RegisterDefault<AssemblyDomain>();

//使用 Natasha 的 CSharp 编译器直接编译字符串
AssemblyCSharpBuilder sharpBuilder = new AssemblyCSharpBuilder();

//给编译器指定一个随机域
sharpBuilder.Compiler.Domain = DomainManagement.Random;

//使用文件编译模式，动态的程序集将编译进入DLL文件中，当然了你也可以使用内存流模式。
sharpBuilder.UseFileCompile();

//如果代码编译错误，那么抛出并且记录日志。
sharpBuilder.ThrowAndLogCompilerError();

//如果语法检测时出错，那么抛出并记录日志。
sharpBuilder.ThrowAndLogSyntaxError();

//添加你的字符串
sharpBuilder.Syntax.Add("using System; public static class Test{ public static void Show(){ Console.WriteLine(\"Hello World!\");}}");

//编译出一个程序集
var assembly = sharpBuilder.GetAssembly();


//创建一个 Action 委托
//必须在同一域内，因此指定域
//写调用脚本，把刚才的程序集扔进去，这样会自动添加using引用
var action = NDelegate.UseDomain(sharpBuilder.Compiler.Domain).Action("Test.Show();", assembly);

//运行，看到 Hello World!
action();

```
