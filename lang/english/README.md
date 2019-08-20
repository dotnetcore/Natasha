<p align="center">
  <span>English</span> |  
  <a href="https://github.com/dotnetcore/natasha">中文</a>
</p>

# Natasha 

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![NuGet Badge](https://buildstats.info/nuget/DotNetCore.Natasha?includePreReleases=true)](https://www.nuget.org/packages/DotNetCore.Natasha)
[![Gitter](https://badges.gitter.im/dotnetcore/natasha.svg)](https://gitter.im/dotnetcore/Natasha?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Badge](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu/#/zh_CN)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/blob/master/LICENSE)

&ensp;&ensp;&ensp;&ensp;This is a roslyn-based dynamic compilation library that provides you with efficient, high-performance, traceable dynamic build solutions. It is compatible with stanadard2.0, and uses only native C # syntax without Emit. 
Make your dynamic approach easier to write, track, and maintain.  Welcome to discuss with me online.：[Click and join the gitter](https://gitter.im/dotnetcore/Natasha)

<br/>

### Library Info  

[![GitHub tag (latest SemVer)](https://img.shields.io/github/tag/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/releases) ![GitHub repo size](https://img.shields.io/github/repo-size/dotnetcore/Natasha.svg) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/commits/master) [![Codecov](https://img.shields.io/codecov/c/github/dotnetcore/natasha.svg)](https://codecov.io/gh/dotnetcore/Natasha)  

| Scan Name | Status |
|--------- |------------- |
| Document | [![wiki](https://img.shields.io/badge/wiki-ch-blue.svg)](https://github.com/dotnetcore/Natasha/wiki) |
| Lang | ![Complie](https://img.shields.io/badge/script-csharp-green.svg)|
| Rumtime | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) | 
| OS | ![Windows](https://img.shields.io/badge/os-windows-black.svg) ![linux](https://img.shields.io/badge/os-linux-black.svg) ![mac](https://img.shields.io/badge/os-mac-black.svg)|   

<br/>  

### CI Build Status  

| CI Platform | Build Server | Master Build  | Master Test |
|--------- |------------- |---------| --------|
| Travis | Linux/OSX | [![Build status](https://travis-ci.org/dotnetcore/Natasha.svg?branch=master)](https://travis-ci.org/dotnetcore/Natasha) | |
| AppVeyor | Windows/Linux |[![Build status](https://ci.appveyor.com/api/projects/status/5ydt5yvb9lwfqocw?svg=true)](https://ci.appveyor.com/project/NMSAzulX/natasha)|[![Build status](https://img.shields.io/appveyor/tests/NMSAzulX/Natasha.svg)](https://ci.appveyor.com/project/NMSAzulX/natasha)|
| Azure |  Windows |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Windows)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) |
| Azure |  Linux |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Linux)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 
| Azure |  Mac |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=macOS)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 

<br/>    

### Wiki Review  

Teng(359768998@qq.com)  

 
 <br/>  
 
 
### Code Review  

WeihanLi

 <br/> 
 
 
### Publish Plan  

 - 2019-08-01 ： Release v1.0.0.0，The first official stable version.  
 - 2019-08-02 ： Release v1.0.4.0，Support asynchronous methods, support attributes.
 - 2019-08-04 ： Release v1.1.0.0，Optimize the compilation engine, distinguish OS characters, and increase exception capture.
 - 2019-08-05 ： Release v1.2.0.0，Support for compiling class/interface/struct, add FieldTemplate, add string extension method.  
 - 2019-08-06 ： Release v1.2.1.0，Scrap RuntimeComplier and switch to OopComplier. Mixed compilation of scripts and DLL files is supported.
 - 2019-08-09 ： Release v1.3.2.0，Support for compiling `unsafe` Method. MethodTemplate/OnceMethodTempalte add `UseUnsafe` Method.  
 - 2019-08-11 ： Release v1.3.4.0，Add NFunc/NAction Method, implement a delegate quickly. 
 - 2019-08-16 ： Release v1.3.6.0，Support for compiling enum type, log has added one folder names 'hh:mm'.  
 
 <br/>  
 
---------------------  
 <br/>  
 

### User Api：  

 <br/>  
 
 > More new reference documentation：https://github.com/dotnetcore/Natasha/tree/master/article/en


<br/>    

 
#### First edit your project file *.csproj：

```C#

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>   <--- Must.
    <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish> <---- WEB Publish Must.
  </PropertyGroup>
  
```  

<br/>
<br/> 


#### Mixed compilation of Script and Dll

```C#

//.dll content：
using System;

namespace ClassLibrary1
{
    public class Class1
    {
        public void Show1()
        {
            Console.WriteLine("RunShow1");
        }

        public static void Show2()
        {
            Console.WriteLine("RunShow2");
        }
    }
}

```

```C#

string text = @"
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using ClassLibrary1;
 
    namespace HelloWorld
    {
       public class Test
       {
            public Test(){
               Name=""111"";
            }

           public string Name;
           public int Age{get;set;}

            public override string ToString(){

                Class1 a = new Class1();
                a.Show1();
                Class1.Show2();
                return ""11"";

            }
       }
    }";
    
//Class1 included ClassLibrary1.dll


//add dll files/ complie
OopComplier oop = new OopComplier();
oop.LoadFile(@"D:\Project\IlTest\ClassLibrary1\bin\Debug\netstandard2.0\ClassLibrary1.dll");
Type type = oop.GetClassType(text);


//call
var a = Activator.CreateInstance(type);
Console.WriteLine(a.ToString());

```

<br/>
<br/> 



#### Catch exception：

```C#
  var fastBuilder = FastMethodOperator.New;
  fastBuilder.Complier.Exception;             
  if(fastBuilder.Complier.Exception.ErrorFlag == ComplieError.None) 
  {
        //Compiled successfully!
  }
  
  
  var fakeBuilder = FakeMethodOpeartor.New;
  fakeBuilder.Complier.Exception;
  
  
  var classBuilder = New ClassBuilder();
  classBuilder.Complier.Exception;
  
```  
<br/>
<br/> 


#### Using FastMethodOperator to build dynamic functions quickly：  
  
  
```C#
var action = FastMethodOperator.New
             .Param<string>("str1")
             .Param(typeof(string),"str2")
             .MethodBody("return str1+str2;")
             .Return<string>()
             .Complie<Func<string,string,string>>();
                    
string result = action("Hello ","World!");    //result:   "Hello World!"




//Enhanced implementation and asynchronous support


//Complie<T> ： This method detects the parameters and the return type, and if any of them is not specified, the Complie method populates it with its own default parameter or return value
//If it is a Action < int > with 1 parameter, use "arg".
var delegateAction = FastMethodOperator.New

       .UseAsync()
       .MethodBody(@"
               await Task.Delay(100);
               string result = arg1 +"" ""+ arg2;  
               Console.WriteLine(result);
               return result;")

       .Complie<Func<string, string, Task<string>>>();
  
string result = await delegateAction?.Invoke("Hello", "World2!");   //result:   "Hello World2!"


//If you want to use asynchronous methods, use either the UseAsync method or the AsyncFrom<Class>(methodName) method.
//The returned parameter requires you to specify Task < >. Remember that the outer layer method should have the async keyword.

```
<br/>
<br/>  

#### Fast implementation of delegation using DelegateOperator：  

```C# 

//Define a delegate
public delegate string GetterDelegate(int value);
     
     
     
//Usage 1    
var action = DelegateOperator<GetterDelegate>.Create("value += 101; return value.ToString();");
string result = action(1);              //result: "102"



//Usage 2
var action = "value += 101; return value.ToString();".Create<GetterDelegate>();
string result = action(1);              //result: "102"
     
```  

<br/>
<br/>  

#### Using FakeMethodOperator to build dynamic functions quickly：  

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


### Convenient extension  
 <br/>  
 

#### Class extension of Natasha:  

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

#### Method extension of Natasha:  

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


        //use string exntesion method.
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
 
 #### Cloning extension of Natasha:  

```C#

Example:  

        Using : Natasha.Clone; 
        var instance = new ClassA();
        var result = instance.Clone();
```
<br/>
<br/>    
 
  #### Snapshot extension of Natasha:  

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

  #### Dynamic call extension of Natasha:  

```C#

Example:  

        Using : Natasha.Caller; 
        var instance = new ClassA();
        
        //Get DynamicHandler on the instance.
        var handler = instance.Caller();
        
        //Get Operation
        handler.Get<string>("MemberName");
        handler["MemberName"].Get<string>();
        
        //Set Operation
        handler.Set("MemberName",AnythingValue);
        handler["MemberName"].Set(AnythingValue);

```

<br/>
<br/>    

#### Natasha Dynamic Module:  Moved to [【NCaller】](https://github.com/night-moon-studio/NCaller)
<br/>    

---------------------  


- **Benchmark test plan（Waiting for the next version of bechmark）**：
      
     - [ ]  **Dynamic function performance test（control group： emit, origin）**  
     - [ ]  **Dynamic call performance test（control group： dynamic direct call，dynamic proxy call，emit, origin）**  
     - [ ]  **Dynamic cloning performance test（control group： origin）**
     - [ ]  **Performance test of remote module（control group： emit, origin）**

---------------------  

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      
     
