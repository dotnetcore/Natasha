<p align="center">
  <span>中文</span> |  
  <a href="https://github.com/dotnetcore/natasha/tree/master/lang/english">English</a>
</p>
<p align="center"> <span>你们的反馈是我的动力，文档还有很多不足之处；</span> </p>
<p align="center"> <span> 当你看完文档之后仍然不知道如何实现你的需求，您可以查看<a href="https://github.com/dotnetcore/Natasha/blob/master/docs/FAQ.md"> FAQ </a>或者在issue中提出你的需求。</span> </p>
  
<br/>  

# 个性化需求请联系作者有偿定制! (按功能点作者估算 或 1K/天)  

<br/>  


# Natasha 

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![NuGet Badge](https://buildstats.info/nuget/DotNetCore.Natasha.CSharp.All?includePreReleases=true)](https://www.nuget.org/packages/DotNetCore.Natasha.CSharp.All)
[![Gitter](https://badges.gitter.im/dotnetcore/natasha.svg)](https://gitter.im/dotnetcore/Natasha?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Badge](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu/#/zh_CN)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/blob/master/LICENSE)

&ensp;&ensp;&ensp;&ensp;基于 [Roslyn](https://github.com/dotnet/roslyn) 的 C# 动态程序集构建库，该库允许开发者在运行时使用 C# 代码构建域 / 程序集 / 类 / 结构体 / 枚举 / 接口 / 方法等，使得程序在运行的时候可以增加新的模块及功能。Natasha 集成了域管理/插件管理，可以实现域隔离，域卸载，热拔插等功能。 该库遵循完整的编译流程，提供完整的错误提示， 可自动添加引用，完善的数据结构构建模板让开发者只专注于程序集脚本的编写，兼容 netcoreapp2.0+ / netcoreapp3.0+, 跨平台，统一、简便的链式 API。 且我们会尽快修复您的问题及回复您的 [issue](https://github.com/dotnetcore/Natasha/issues/new).   
[更多的动图展示](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/gif.md)

![展示](https://images.gitee.com/uploads/images/2020/1201/161046_e8f52622_1478282.gif)

<br/>
<br/>

### 类库信息(Library Info)  
 

[![GitHub tag (latest SemVer)](https://img.shields.io/github/tag/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/releases) ![GitHub repo size](https://img.shields.io/github/repo-size/dotnetcore/Natasha.svg) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/commits/master) [![Codecov](https://img.shields.io/codecov/c/github/dotnetcore/natasha.svg)](https://codecov.io/gh/dotnetcore/Natasha) 

| Script | .NET Env | Document |
| ------ | -------- | -------- |  
| ![Compile](https://img.shields.io/badge/script-csharp-green.svg) | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.1-blue.svg) ![standard](https://img.shields.io/badge/platform-net5.0-blue.svg)| [![wiki](https://img.shields.io/badge/wiki-ch-blue.svg)](https://natasha.dotnetcore.xyz/)  |

| CI Platform | Build Server | Master Build  |
|--------- |--------- |---------|
| Github |![os](https://img.shields.io/badge/os-all-black.svg)| [![Build status](https://img.shields.io/github/workflow/status/dotnetcore/Natasha/.NET%20Core/master)](https://github.com/dotnetcore/Natasha/actions) |
| Azure |![Windows](https://img.shields.io/badge/os-win-black.svg) | [![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Windows)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|
| Azure |![linux](https://img.shields.io/badge/os-linux-black.svg) |[![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=Linux)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|
| Azure |![mac](https://img.shields.io/badge/os-mac-black.svg)| [![Build Status](https://dev.azure.com/NightMoonStudio/Natasha/_apis/build/status/dotnetcore.Natasha?branchName=master&jobName=macOS)](https://dev.azure.com/NightMoonStudio/Natasha/_build/latest?definitionId=3&branchName=master)|

<br/>      
<br/>  

### 使用方法(User Api)：   
| 顺序 | 操作行为 | 操作内容 | 说明 |
|-- |-------- |--------------| --------|
| 1 | 引类库 | DotNetCore.Natasha.CSharp.All | 该页面提供最新版操作，[旧版详见](https://github.com/dotnetcore/Natasha/tree/v3.0+final) |
| 2 | 初始化 | NatashaInitializer.InitializeAndPreheating(); / Initialize(); | 预热会慢一点，后面编译就快了，也可以只初始化不预热 |
| 3 | 写代码 | 使用说明 [https://natasha.dotnetcore.xyz/](https://natasha.dotnetcore.xyz/) | 可联系作者: 1648107003@qq.com 或 [查看FAQ](https://github.com/dotnetcore/Natasha/blob/master/docs/FAQ.md) 或 [提出问题](https://github.com/dotnetcore/Natasha/issues/new) |

<br/>  
<br/>  

### 发布日志  
- 2019年发布日志 [[已归档]](https://natasha.dotnetcore.xyz/docs/release_notes/2019)
- 2020年发布日志 [[已归档]](https://natasha.dotnetcore.xyz/docs/release_notes/2020)
- 2021年发布日志 [[进行中]](https://natasha.dotnetcore.xyz/docs/release_notes/2021)  

### 最新发布  

 ## v3.1.0.0
 
 - ##### 2021/07/14
 
    - 同时支持 standard2.0 及 core2.0 以上的各版本.
 
    - 优化 DomainBase 类的插件引用管理方案, 进一步同意引用管理, 移除不必要的对外暴露的方法.
    
    - 优化 NatashaAssemblyDomain 的实现, 添加 core3.0- 版本的依赖反解, 插件行为异同: 
  
      - core3.0 + 解析 json 文件并加载程序集, 支持插件依赖文件版本覆盖, 对插件主体进行升级.
  
      - core3.0 - 仅扫描同目录下 dll 并加载程序集, 不能对插件依赖进行升级.
    
    - 优化 编译流程及事件调用.
    
    - 优化 命名空间反解流程, 屏蔽 ExportedTypes 无法导出而引发异常.
    
    - 优化 模板类 API 命名及归类, 区分 Set 和 Append 操作.
    
    - 增加 命名控件内容追加模板, 支持开发者向同一命名空间中追加代码.
    
    - 增加 Record 数据类型的语法节点自动获取的 API.
    
    - 重命名各基础类字段及事件.
    
    - Natasha.CSharp.Extension.InvisibleInstance 扩展将以 CS0103 错误为基准,对调用链进行补充.  


<br/>  
<br/>  
  
### 生态周边  
| 维护状态 | 项目名称 | 项目地址 | 项目简介 | 
| -----: | ------------- | ----------- | -------- |
| ![flag](https://img.shields.io/badge/√-darkgreen.svg) | DynamicProxy | [Github](https://github.com/night-moon-studio/DynamicProxy) | 基于 Natasha 的高性能接口动态代理,支持多参数初始化和单例模式 |
| ![flag](https://img.shields.io/badge/√-darkgreen.svg) | Libra | [Github](https://github.com/night-moon-studio/Libra) | 基于 Natasha 和 DynamicDictionary 的高性能,弱约束的 Http 协议 RPC 库 | 
| ![flag](https://img.shields.io/badge/√-blue.svg) | NatashaPad | [Github](https://github.com/night-moon-studio/NatashaPad) | 由 Roslyn 和 Natasha 支持的另一种dotnet调试工具，如 LinqPad 和 dotnet fiddle |
| ![flag](https://img.shields.io/badge/√-blue.svg) | Leo | [Github](https://github.com/night-moon-studio/NCaller) | 基于 Natasha 和 查找树算法的高速反射类，可以操作对象的属性以及字段 |
| ![flag](https://img.shields.io/badge/X-red.svg) | DeepClone | [Github](https://github.com/night-moon-studio/DeepClone) | 由 Natasha 的高性能深度克隆库 |
| ![flag](https://img.shields.io/badge/√-darkgreen.svg) | BTFindTreee | [Github](https:https://github.com/dotnet-lab/BTFindTreee) | 快速查找算法的构建，包括哈希二分查找，字串模糊查找，字串归并精确查找 |
| ![flag](https://img.shields.io/badge/X-red.svg) | Papper | [Github](https:https://github.com/dotnet-lab/Papper) | 对语法树解析库，主要目标时服务于 SG(Source Generator)技术 |
| ![flag](https://img.shields.io/badge/X-red.svg) | RuntimeToDynamic | [Github](https://github.com/night-moon-studio/RuntimeToDynamic) | 将运行时数据压入到动态代理类中，以方便其他动态构建时对其进行复用|
| ![flag](https://img.shields.io/badge/√-darkgreen.svg) | DynamicDictionary | [Github](https://github.com/night-moon-studio/DynamicCache) | 高速动态缓存，在只读并发场景中提供超高性能的数据查找功能|
| ![flag](https://img.shields.io/badge/√-blue.svg) | Aries | [Github](https://github.com/night-moon-studio/Aries) | 对 FreeSql 的高度封装，提供高性能、直观的 外联 / 乐观锁 / CURD 操作 |  
  
<br/>
  
 > **Note:**   
![flag](https://img.shields.io/badge/√-darkgreen.svg) : 维护且更新活跃,需求充足.   
![flag](https://img.shields.io/badge/√-blue.svg) : 维护但需求较少, 按需更新发布.   
![flag](https://img.shields.io/badge/X-red.svg) : 需求极少, 暂不维护.
  
<br/>
<br/>  

### 开发计划
#### 近期计划  

  - [ ] 挣钱, 生存下来
  - [ ] 调研 C# 10, 补充 OOP 模板.
  - [ ] 调研 .NET6.0, 增加性能优化点.
  - [ ] 搜集需求,增加语义扩展库.

<br/>  

### 性能测试
      
   - [x]  **动态初始化性能测试（对照组： emit, origin）**  
     ![初始化性能测试](https://images.gitee.com/uploads/images/2020/1201/161738_b54dd1ad_1478282.png)
   - [x]  **内存及CPU监测截图**  
     ![内存及CPU](https://images.gitee.com/uploads/images/2020/1201/161450_96e70709_1478282.png)      

<br/>    
<br/> 

### 赞助：

<img width=200 height=200 src="https://images.gitee.com/uploads/images/2020/1201/163955_a29c0b44_1478282.png" title="Scan and donate"/><img width=200 height=200 src="https://images.gitee.com/uploads/images/2020/1201/164809_5a67d5e2_1478282.png" title="Scan and donate"/>


#### 捐助明细  
  
- iNeuOS工业互联网平台 100 元 
- ****天下 10元  
- 文航 5元
- TonyQu 10元
- Rwing 20元  

 
  
<br/>  

---------------------  


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      

