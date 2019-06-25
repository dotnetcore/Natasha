# Natasha

Dynamic compilation of runtime code using roslyn, high performance, traceable.  [OnlineChat](https://gitter.im/dotnetcore/Natasha)

使用roslyn动态编译运行时代码，高性能、可追踪。  欢迎参与讨论：[点击加入Gitter讨论组](https://gitter.im/dotnetcore/Natasha)


[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![NuGet Badge](https://buildstats.info/nuget/Natasha?includePreReleases=true)](https://www.nuget.org/packages/Natasha)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/Natasha.svg)](https://github.com/dotnetcore/EasyCaching/blob/master/LICENSE)
[![Gitter](https://badges.gitter.im/dotnetcore/Natasha.svg)](https://gitter.im/dotnetcore/Natasha?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_shield)  


<br/>

### 类库信息(Library Info)

| Scan Name | Status |
|--------- |------------- |
| Rumtime Support | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) | 
| Lang Support | ![Complie](https://img.shields.io/badge/script-csharp-green.svg)|
| OS Support | ![Windows](https://img.shields.io/badge/os-windows-orange.svg) ![linux](https://img.shields.io/badge/os-linux-orange.svg) ![mac](https://img.shields.io/badge/os-mac-orange.svg)|   

<br/>  

### 持续构建(CI Build Status)  

| CI Platform | Build Server | Master Status  |
|--------- |------------- |---------|
| AppVeyor |  Windows/Linux |[![Build status](https://ci.appveyor.com/api/projects/status/5ydt5yvb9lwfqocw?svg=true)](https://ci.appveyor.com/project/NMSAzulX/natasha)|  
| Azure |  Windows |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Windows)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)| 
| Azure |  Linux |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Linux)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)| 
| Azure |  Mac |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=macOS)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)| 

<br/>  

### 发布计划(Publish Plan)  

 - 2019-06-25晚 ： 将发布v0.7.1.2, 修复跨平台调用，将object类型纳入一次性赋值类型，增加类扩展方法。
---------------------

### 使用方法(User Api)：

#### 首先编辑您的工程文件

```C#
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>   <--- 一定要加上这句话
  </PropertyGroup>
```

#### 使用 FastMethodOperator 快速构建函数：  
  
  
```C#
var action = FastMethodOperator.New
             .Param<string>("str1")
             .Param(typeof(string),"str2")
             .MethodBody("return str1+str2;")
             .Return<string>()
             .Complie<Func<string,string,string>>();
                    
string result = action("Hello ","World!");    //result:   "Hello World!"
```
<br/>
<br/>  

#### 使用 DelegateOperator 快速实现委托：  

```C# 
//定义一个委托
public delegate string GetterDelegate(int value);
     
     
var action = DelegateOperator<GetterDelegate>.Create("value += 101; return value.ToString();");
     
string result = action(1);              //result: "102"
     
```  

<br/>
<br/>  

#### 使用 FakeMethodOperator 快速构建函数：  

```C#
public class Test{ 

   public string Handler(string str){ 
        retrurn null; 
   }
   
}

```
```C#
var action = FakeMethodOperator.New
             .UseMethod(typeof(Test).GetMethod("Handler"))
             .StaticMethodContent(" str += "" is xxx;"",return str; ")
             .Complie<Func<string,string>>();
                  
string result = action("xiao");              //result: "xiao is xxx;"          
```
  
<br/>
<br/>  

#### 使用Natasha的类扩展  

```C#

Example:  
        Type : Dictionary<string,List<int>>[] 
        
        typeof(Dictionary<string,List<int>>).GetDevelopName();     //result:  "Dictionary<String,List<Int32>>[]"
        typeof(Dictionary<string,List<int>>).GetAvailableName();   //result:  "Dictionary_String_List_Int32____"
        typeof(Dictionary<string,List<int>>).GetAllGenericTypes(); //result:  [string,list<>,int]
        typeof(Dictionary<string,List<int>>).IsImplementFrom<IDictionary>(); //result: true
        typeof(Dictionary<string,List<int>>).IsOnceType();         //result: false
        typeof(List<>).With(typeof(int));                          //result: List<int>

```
<br/>
<br/>  

#### 动态调用普通类  

```C#
public class A{
   public int Age;
   public DateTime Time;
   public B Outter = new B();
}
public class B{
   public string Name;
   public B(){
      Name = "小明"
   }
}
//如果是运行时动态生成类，也同样
```
调用方式一
```C#
var handler = DynamicOperator.GetOperator(typeof(A));

handler["Age"].IntValue = 100;                                    // Set Operator

Console.WriteLine(handler["Time"].DateTime);                      // Get Operator

handler["Outter"].OperatorValue["Name"].StringValue = "NewName"   // Link Operator
```
调用方式二
```C#
var handler EntityOperator.Create(typeof(A));

handler.New();

handler.Set("Age",100);                                           // Set Operator

Console.WriteLine(handler.Get<DateTime>("Time"));                  // Get Operator

handler.Get("Outter")["Name"].Set("NewName");                     // Link Operator
```
<br/>
<br/>  

#### 动态调用静态类
```C#
public static class A{
   public static int Age;
   public static DateTime Time;
   public static B Outter = new B();
}
public class B{
   public string Name;
   public B(){
      Name = "小明";
   }
}
//如果是运行时动态生成类，也同样
```
调用方式一
```C#
DynamicStaticOperator handler = typeof(A);

handler["Age"].IntValue = 100;                                        // Set Operator

Console.WriteLine(handler["Time"].DateTime);                          // Get Operator

handler.Get["Outter"].OperatorValue["Name"].StringValue = "NewName"   // Link Operator
```
调用方式二
```C#
var handler = StaticEntityOperator.Create(type);

handler["Age"].Set(100);                                          // Set Operator

Console.WriteLine(handler["Time"].Get<DateTime>());               // Get Operator

handler.Get("Outter").Set(Name,"NewName");                        // Link Operator

```
<br/>
<br/>  

- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, origin）**  
     - [ ]  **动态调用性能测试（对照组： 动态直接调用，动态代理调用，emit, origin）**  
     - [ ]  **动态克隆性能测试（对照组： origin）**
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, origin）**

        
## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      
     
