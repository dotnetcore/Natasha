<p align="center">
  <span>中文</span> |  
  <a href="https://github.com/dotnetcore/natasha/tree/master/lang/english">English</a>
</p>
<p align="center"> <span>你们的反馈是我的动力，文档还有很多不足之处；</span> </p>
<p align="center"> <span> 当你看完文档之后仍然不知道如何实现你的需求，可以在issue中提出你的需求。</span> </p>

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
| OS | ![Windows](https://img.shields.io/badge/os-windows-black.svg) ![linux](https://img.shields.io/badge/os-linux-black.svg) ![mac](https://img.shields.io/badge/os-mac-black.svg)|
| Rumtime | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.0-blue.svg) | 

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

 ### 性能测试
      
   - [x]  **动态调用性能测试（对照组： emit, origin）**  
     ![字段性能测试](https://github.com/dotnetcore/Natasha/blob/docs/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%951.png)
   - [x]  **动态初始化性能测试（对照组： emit, origin）**  
     ![初始化性能测试](https://github.com/dotnetcore/Natasha/blob/docs/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%952.png)



<br/>    

### Wiki审核

Teng(359768998@qq.com)

<br/>    

### 代码审核

WeihanLi

<br/>    

### 发布日志  

 - 2019-09-27 ： 发布v2.0.0.2，修复默认域的插件加载操作，增加域内并发控制，使用链表方式存储引用表。  
 - 2019-10-01 ： 发布v2.0.2.0，在流的基础上，恢复文件方式的编译、加载, 增强错误日志处理，增强反解器（内部类部分）。
 - 2019-10-12 ： 发布v2.0.2.1，修复插件依赖加载。  
 - 2019-10-15 ： 发布v2.0.4.0，增强构建信息操作类。 
 - 2019-10-20 ： 发布v2.0.5.0，增强NAction/NFunc/DelegateOperator等封装类的传参类型，支持string/string[]/Type/Type[]/Assembly/Asssembly[], 增强Natasha的Using方法解析。 
 - 2019-11-26 :  发布v2.1.0.0，增加随机域API, 改Operator.New为Operator.MainDomain; 区分NAction/NFunc/NDelegateOperator为默认域，RAction/RFunc/RDelegateOperator为随机域API;增加随机域延迟回收机制。
 
 <br/>  
 
 
 ### 升级日志
 
 - [[2019]](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/update/2019.md)
  
 <br/>  
 
 

 ### 生态微信群  
 
为防止广告骚扰，微信群已关闭，进群请发送您的微信号到 2765968624@qq.com 并说明进群原因。
 
 
  <br/>  
 
---------------------  


### 使用方法(User Api)：  

 <br/>  
 
 > 更多更新的参考文档：https://natasha.dotnetcore.xyz/  

<br/>    

 
#### 首先编辑您的工程文件：

```C#

  <PropertyGroup>
  
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    
    //一定要加上这句话
    <PreserveCompilationContext>true</PreserveCompilationContext>
    
    //WEB发布要加
    <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
    
  </PropertyGroup>
 
```  

<br/>
<br/>  

  #### Natasha的动态调用模块:  已移至[【NCaller】](https://github.com/night-moon-studio/NCaller)
  #### Natasha的动态调用模块:  已移至[【DeepClone】](https://github.com/night-moon-studio/DeepClone)  
  
<br/>
<br/>    


---------------------  


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      
     
