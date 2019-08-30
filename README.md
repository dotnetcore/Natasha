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

### 代码审核

WeihanLi

<br/>    

### 发布日志  

 - 2019-08-11 ： 发布v1.3.4.0，增加NFunc/NAction方法, 可快速动态构建方法。  
 - 2019-08-16 ： 发布v1.3.6.0，添加枚举构建及编译方法, 日志添加一级 “时分” 目录。 
 - 2019-09-01 ： 发布v2.0.0.0，支持共享域协作，支持创建、卸载、锁域操作，支持多程序集合并编译、覆盖编译，支持外部文件热加载，封装字符串解构操作。  
 
 <br/>  
 
 ### 升级日志
 
 - [[2019]](https://github.com/dotnetcore/Natasha/blob/docs/docs/zh/update/2019.md)
  
 <br/>  
 
 ### 微信群  
 
 <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/WeChart.jpg" height="450" width="250" alt="Natasha生态群"/>

 
  <br/>  
 
---------------------  
 <br/>  
 

### 使用方法(User Api)：  

 <br/>  
 
 > 更多更新的参考文档：https://github.com/dotnetcore/Natasha/tree/master/article/ch


<br/>    

 
#### 首先编辑您的工程文件：

```C#

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>   <--- 一定要加上这句话
    <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish> <---- WEB发布一定要加
  </PropertyGroup>
 
```  

<br/>
<br/> 

#### 异常捕获方法：

```C#

  var fastBuilder = FastMethodOperator.New;
  fastBuilder.Complier.Exception;             //编译后异常会进入这里

  
  var fakeBuilder = FakeMethodOpeartor.New;
  fakeBuilder.Complier.Exception;
  
  
  var oopBuilder = new OopBuilder();
  oopBuilder.Complier.Exception;
  
  
  if(builder.Complier.Exception.ErrorFlag == ComplieError.None) 
  {
        //编译成功！
  }
  
```  

<br/>
<br/> 

#### 脚本与DLL混合编译

```C#

//dll 中的内容：
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
            public override string ToString()
            {

                Class1 a = new Class1();
                a.Show1();
                Class1.Show2();
                return ""11"";

            }
       }
    }";
    
//Class1 来自于 ClassLibrary1.dll


//添加/编译
OopComplier oop = new OopComplier();
oop.LoadFile(@"D:\Project\IlTest\ClassLibrary1\bin\Debug\netstandard2.0\ClassLibrary1.dll");
Type type = oop.GetClassType(text);


//调用
var a = Activator.CreateInstance(type);
Console.WriteLine(a.ToString());

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
      
     
