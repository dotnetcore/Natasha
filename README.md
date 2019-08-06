<p align="center">
  <span>中文</span> |  
  <a href="https://github.com/dotnetcore/natasha/tree/master/lang/english">English</a>
</p>

# Natasha 

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![NuGet Badge](https://buildstats.info/nuget/DotNetCore.Natasha?includePreReleases=true)](https://www.nuget.org/packages/DotNetCore.Natasha)
[![Gitter](https://badges.gitter.im/dotnetcore/natasha.svg)](https://gitter.im/dotnetcore/Natasha?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Badge](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu/#/zh_CN)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/blob/master/LICENSE)

&ensp;&ensp;&ensp;&ensp;基于roslyn的动态编译库，为您提供高效率、高性能、可追踪的动态构建方案，兼容stanadard2.0, 只需原生C#语法不用Emit。
让您的动态方法更加容易编写、跟踪、维护。  欢迎参与讨论：[点击加入Gitter讨论组](https://gitter.im/dotnetcore/Natasha)

<br/>

### 类库信息(Library Info)  

[![GitHub tag (latest SemVer)](https://img.shields.io/github/tag/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/releases) ![GitHub repo size](https://img.shields.io/github/repo-size/dotnetcore/Natasha.svg) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/commits/master) [![Codecov](https://img.shields.io/codecov/c/github/dotnetcore/natasha.svg)](https://codecov.io/gh/dotnetcore/Natasha)  

| Scan Name | Status |
|--------- |------------- |
| Document | [![wiki](https://img.shields.io/badge/wiki-ch-blue.svg)](https://github.com/dotnetcore/Natasha/wiki) |
| Lang | ![Complie](https://img.shields.io/badge/script-csharp-green.svg)|
| Rumtime | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) | 
| OS | ![Windows](https://img.shields.io/badge/os-windows-black.svg) ![linux](https://img.shields.io/badge/os-linux-black.svg) ![mac](https://img.shields.io/badge/os-mac-black.svg)|   

<br/>  

### 持续构建(CI Build Status)  

| CI Platform | Build Server | Master Build  | Master Test |
|--------- |------------- |---------| --------|
| Travis | Linux/OSX | [![Build status](https://travis-ci.org/dotnetcore/Natasha.svg?branch=master)](https://travis-ci.org/dotnetcore/Natasha) | |
| AppVeyor | Windows/Linux |[![Build status](https://ci.appveyor.com/api/projects/status/5ydt5yvb9lwfqocw?svg=true)](https://ci.appveyor.com/project/NMSAzulX/natasha)|[![Build status](https://img.shields.io/appveyor/tests/NMSAzulX/Natasha.svg)](https://ci.appveyor.com/project/NMSAzulX/natasha)|
| Azure |  Windows |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Windows)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) |
| Azure |  Linux |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Linux)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 
| Azure |  Mac |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=macOS)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 

<br/>    

### Wiki审核

Teng(359768998@qq.com)

<br/>    

### 发布计划(Publish Plan)  

 - 2019-08-01 ： 发布v1.0.0.0, 发布稳如老狗版，抛弃Emit农耕铲，端起Roslyn金饭碗。  
 - 2019-08-02 ： 发布v1.0.4.0，支持异步方法还原与构造，增加注解构建。
 - 2019-08-04 ： 发布v1.1.0.0，优化编译引擎，区分OS字符，增加异常捕获。
 - 2019-08-05 ： 发布v1.2.0.0，支持类/结构体/接口的编译，支持字段模板，增加string扩展。

 <br/>  
 
 ### 升级日志
 
 - [[2019]](https://github.com/dotnetcore/Natasha/wiki/Update2019)
  
 <br/>  
 
---------------------  
 <br/>  
 

### 使用方法(User Api)：  
 <br/>  
 
#### 首先编辑您的工程文件：

```C#
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>   <--- 一定要加上这句话
  </PropertyGroup>
```  
<br/>
<br/> 

#### 异常捕获方法：

```C#
  var fastBuilder = FastMethodOperator.New;
  fastBuilder.Complier.Exception;             //编译后异常会进入这里
  if(fastBuilder.Complier.Exception.ErrorFlag == ComplieError.None) 
  {
        //编译成功！
  }
  
  
  var fakeBuilder = FakeMethodOpeartor.New;
  fakeBuilder.Complier.Exception;
  
  
  var classBuilder = New ClassBuilder();
  classBuilder.Complier.Exception;
  
```  
<br/>
<br/> 

#### 使用 FastMethodOperator 快速构建函数：  
  
  
```C#
var action = FastMethodOperator.New
             .Param<string>("str1")
             .Param(typeof(string),"str2")
             .MethodBody("return str1+str2;")
             .Return<Task<string>>()
             .Complie<Func<string,string,string>>();
                    
var result = action("Hello ","World!");    //result:   "Hello World!"


//增强实现与异步支持


//Complie<T>方法会检测参数以及返回类型，如果其中有任何一处没有指定，那么Complie方法会使用自己默认的参数或者返回值进行填充
//如果是Action<int> 这种带有1个参数的，请使用"arg"


var delegateAction = FastMethodOperator.New

       .UseAsync()
       .MethodBody(@"
               await Task.Delay(100);
               string result = arg1 +"" ""+ arg2; 
               Console.WriteLine(result);
               return result;")
               
       .Complie<Func<string, string, Task<string>>>();
      
string result = await delegateAction?.Invoke("Hello", "World2!");   //result:   "Hello World2!"


//另外如果想使用异步方法，请使用UseAsync方法,或者AsyncFrom<Class>(methodName)这两种方法。
//返回的参数需要您指定Task<>,以便运行时异步调用，记得外面那层方法要有async关键字哦。

```
<br/>
<br/>  

#### 使用 DelegateOperator 快速实现委托：  

```C# 
//定义一个委托
public delegate string GetterDelegate(int value);
     
     
     
//方法一     
var action = DelegateOperator<GetterDelegate>.Create("value += 101; return value.ToString();");
string result = action(1);              //result: "102"



//方法二
var action = "value += 101; return value.ToString();".Create<GetterDelegate>();
string result = action(1);              //result: "102"
     
```  

<br/>
<br/>  

#### 使用 FakeMethodOperator 快速构建函数：  

```C#
public class Test
{ 
   public string Handler(string str)
   { 
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




### 方便的扩展  
 <br/>  
 

#### 使用Natasha的类扩展:  

```C#

Example:          
         
         
        typeof(Dictionary<string,List<int>>[]).GetDevelopName();
        //result:  "Dictionary<String,List<Int32>>[]"
        
              
        typeof(Dictionary<string,List<int>>[]).GetAvailableName();
        //result:  "Dictionary_String_List_Int32____"
        
           
        typeof(Dictionary<string,List<int>>).GetAllGenericTypes(); 
        //result:  [string,list<>,int]
        
        
        typeof(Dictionary<string,List<int>>).IsImplementFrom<IDictionary>(); 
        //result: true
        
        
        typeof(Dictionary<string,List<int>>).IsOnceType();         
        //result: false
        
        
        typeof(List<>).With(typeof(int));                          
        //result: List<int>

```
<br/>
<br/>    

#### 使用Natasha的方法扩展:  

```C#

Example:  

        Using : Natasha.Method; 
        public delegate int AddOne(int value);
        
        
        var action = "return value + 1;".Create<AddOne>();
        var result = action(9);
        //result : 10
        
        
        var action = typeof(AddOne).Create("return value + 1;");
        var result = action(9);
        //result : 10
        
        
        //使用方法扩展快速动态构建委托
         @"string result = str1 +"" ""+ str2;
           Console.WriteLine(result);
           return result;".FastOperator()
               .Param<string>("str1")
               .Param<string>("str2")
               .Return<string>()
               .Complie<Func<string, string, string>>()
```
<br/>
<br/>    
 
 #### 使用Natasha的克隆扩展:  

```C#

Example:  

        Using : Natasha.Clone; 
        var instance = new ClassA();
        var result = instance.Clone();
```
<br/>
<br/>    
 
  #### 使用Natasha的快照扩展:  

```C#

Example:  

        Using : Natasha.Snapshot; 
        var instance = new ClassA();
        
        instance.MakeSnapshot();
        
        // ********
        //  do sth
        // ********
        
        var result = instance.Compare();
```

<br/>
<br/>    

  #### Natasha的动态调用模块:  已移至[【NCaller】](https://github.com/night-moon-studio/NCaller)

<br/>
<br/>    


---------------------  


- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, origin）**  
     - [ ]  **动态调用性能测试（对照组： 动态直接调用，动态代理调用，emit, origin）**  
     - [ ]  **动态克隆性能测试（对照组： origin）**
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, origin）**

---------------------  

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      
     
