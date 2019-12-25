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


 ![展示](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha.gif)
<br/>

### 类库信息(Library Info)  

[![GitHub tag (latest SemVer)](https://img.shields.io/github/tag/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/releases) ![GitHub repo size](https://img.shields.io/github/repo-size/dotnetcore/Natasha.svg) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/commits/master) [![Codecov](https://img.shields.io/codecov/c/github/dotnetcore/natasha.svg)](https://codecov.io/gh/dotnetcore/Natasha)  

| Scan Name | Status |
|--------- |------------- |
| Document | [![wiki](https://img.shields.io/badge/wiki-ch-blue.svg)](https://github.com/dotnetcore/Natasha/wiki) |
| Lang | ![Complie](https://img.shields.io/badge/script-csharp-green.svg)|
| OS | ![Windows](https://img.shields.io/badge/os-windows-black.svg) ![linux](https://img.shields.io/badge/os-linux-black.svg) ![mac](https://img.shields.io/badge/os-mac-black.svg)|
| Rumtime | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.1-blue.svg)| 

<br/>  

### 持续构建(CI Build Status)  

| CI Platform | Build Server | Master Build  | Master Test |
|--------- |------------- |---------| --------|
| Github | linux/mac/windows | [![Build status](https://img.shields.io/github/workflow/status/dotnetcore/Natasha/.NET%20Core/master)](https://github.com/dotnetcore/Natasha/actions) ||
| Azure |  Windows |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Windows)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) |
| Azure |  Linux |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Linux)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 
| Azure |  Mac |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=macOS)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|[![Build Status](https://img.shields.io/azure-devops/tests/NightMoonStudio/Natasha/3/master.svg)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master) | 


 
<br/>  

### 性能测试
      
   - [x]  **动态调用性能测试（对照组： emit, origin）**  
     ![字段性能测试](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%951.png)
   - [x]  **动态初始化性能测试（对照组： emit, origin）**  
     ![初始化性能测试](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%952.png)
   - [x]  **内存及CPU监测截图**  
     ![内存及CPU](https://github.com/dotnetcore/Natasha/blob/master/Image/%E8%B5%84%E6%BA%90%E7%9B%91%E6%B5%8B.png) 
     



<br/>    

### Wiki审核

Teng(359768998@qq.com)

<br/>    

### 代码审核

WeihanLi

<br/>    

### 发布日志  

 
 - 2019-12-05 ： 发布v2.4.0.0, 移除并发计数，增加委托操作扩展 , 现 Fake/FastMethodOperator.Random 以及 RAction/RFunc 生成的委托都存入了缓存中，可以在委托上直接调用Delete进行卸载； 默认使用所有 Using 覆盖编译代码，当发生二义性引用时使用 CS0104Helper 处理二义性问题， 模板增加 UseCustomerUsing 方法来阻止 Using 覆盖，代价是将必要的 Using 引用精确引入到构建中。 编译引擎在出现二义性引用时将进行重试策略，重试次数1次。
 
  - 2019-12-06 ： 发布v2.5.0.0, Operator 以及 N系列 API, 可以调用静态方法创建实例同时指定所属的域， 包括 Create(string) / Create(AssemblyDomain) / Random() 三种。
  
  - 2019-12-07 ： 发布v2.5.4.0, 增加 CS0234 以及 CS0246 错误自动修复机制，命名空间在无效时会自动剔除并重编译，增加 CS0104 的替换逻辑，在未手动解决二义性时，默认使用靠前的命名空间, 由 NDomain 创建的独立/随机域类型/结构/枚举/接口将支持回收。
  
  - 2019-12-10 ： 发布v2.6.1.0, 移除NFunc / RFunc / NAction / RAction / NDelegateOperator / RDelegateOperator / 字符串扩展方法 / 委托扩展方法；改 Delete 卸载方法为 DisposeDomain ，改 AddInCache 为内部方法不对外开放；三个静态创建实例的方法（Create(string) / Create(AssemblyDomain) / Random()）增加第二个参数，指定是否以文件方式进行编译，默认为 false，增加静态属性 Default 作为系统域即 Create()方法返回实例。

  - 2019-12-16 ： 发布v2.7.0.1, 分离项目，Natasha.Core 项目负责原始编译的 API 以及域操作，Natasha.Reverser 项目负责运行时信息反解操作，Natasha 项目引用了 Natasha.Core 以及 Natasha.Reverser 并组建了 Template / Builder / Operator 动态构建三件套，提供便利的 API 以便对外使用。
  
  - 2019-12-17 ： 发布v2.7.3.0, Natasha 非系统域生成的操作均进行了缓存，以便进行引用移除/域卸载等操作， Type / Delegate / Assembly 均可以使用 RemoveReferences / DisposeDomain 方法进行引用移除或者卸载域操作。
  
  - 2019-12-18 ： 发布v2.7.6.0, Natasha 方法操作类增加 Override 和 NewHidden 方法， NAssembly 增加三种域创建方式， 移除代理操作类到 NCaller 项目中。
  
  - 2019-12-25 ： 发布v2.8.0.0, Natasha 日志操作类改名 NError => NErrorLog / NSucceed => NSucceedLog / NWarning => NWarningLog； 对外开放 DomainManagement.Clear 方法方便手动清除已卸载的域； 增强和修复 CS0234 的替换规则，识别父命名空间正则移除父子命名空间；引擎语法树选项内部进行细微优化。
  
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
      
## 依赖（Dependence）

 - Microsoft.CodeAnalysis.CSharp.Workspaces Version=3.4.0
 - Microsoft.Extensions.DependencyModel Version=3.1.0
 - Microsoft.Net.Compilers Version=3.4.0
 - Microsoft.Net.Compilers.Toolset Version=3.4.0
     
