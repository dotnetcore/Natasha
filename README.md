# 本库将于 2021年1月1日 下架 Natasha.CSharp.All 2.0 之前的版本，望各位互相转告，与贡献者们及时沟通并做出调整。  

<p align="center">
  <span>中文</span> |  
  <a href="https://github.com/dotnetcore/natasha/tree/master/lang/english">English</a>
</p>
<p align="center"> <span>你们的反馈是我的动力，文档还有很多不足之处；</span> </p>
<p align="center"> <span> 当你看完文档之后仍然不知道如何实现你的需求，您可以查看<a href="https://github.com/dotnetcore/Natasha/blob/master/docs/FAQ.md"> FAQ </a>或者在issue中提出你的需求。</span> </p>

# Natasha 

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![NuGet Badge](https://buildstats.info/nuget/DotNetCore.Natasha.CSharp.All?includePreReleases=true)](https://www.nuget.org/packages/DotNetCore.Natasha.CSharp.All)
[![Gitter](https://badges.gitter.im/dotnetcore/natasha.svg)](https://gitter.im/dotnetcore/Natasha?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Badge](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu/#/zh_CN)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/blob/master/LICENSE)

&ensp;&ensp;&ensp;&ensp;基于 [Roslyn](https://github.com/dotnet/roslyn) 的 C# 动态程序集构建库，该库允许开发者在运行时使用 C# 代码构建域 / 程序集 / 类 / 结构体 / 枚举 / 接口 / 方法等，使得程序在运行的时候可以增加新的模块及功能。Natasha 集成了域管理/插件管理，可以实现域隔离，域卸载，热拔插等功能。 该库遵循完整的编译流程，提供完整的错误提示， 可自动添加引用，完善的数据结构构建模板让开发者只专注于程序集脚本的编写，兼容 netcoreapp2.0+ / netcoreapp3.0+, 跨平台，统一、简便的链式 API。 且我们会尽快修复您的问题及回复您的 [issue](https://github.com/dotnetcore/Natasha/issues/new).   
[更多的动图展示](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/gif.md)


 ![展示](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha.gif)  
 
<br/>
<br/>

### 类库信息(Library Info)  
 

[![GitHub tag (latest SemVer)](https://img.shields.io/github/tag/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/releases) ![GitHub repo size](https://img.shields.io/github/repo-size/dotnetcore/Natasha.svg) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/dotnetcore/natasha.svg)](https://github.com/dotnetcore/Natasha/commits/master) [![Codecov](https://img.shields.io/codecov/c/github/dotnetcore/natasha.svg)](https://codecov.io/gh/dotnetcore/Natasha) 

| Script | .NET Env | Document |
| ------ | -------- | -------- |  
| ![Compile](https://img.shields.io/badge/script-csharp-green.svg) | ![standard](https://img.shields.io/badge/platform-standard2.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.0-blue.svg) ![standard](https://img.shields.io/badge/platform-netcore3.1-blue.svg)| [![wiki](https://img.shields.io/badge/wiki-ch-blue.svg)](https://natasha.dotnetcore.xyz/)  |

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
| 3 | 写代码 | 使用说明 [https://natasha.dotnetcore.xyz/](https://natasha.dotnetcore.xyz/) | 可联系作者: 2765968624@qq.com 或 [查看FAQ](https://github.com/dotnetcore/Natasha/blob/master/docs/FAQ.md) 或 [提出问题](https://github.com/dotnetcore/Natasha/issues/new) |

<br/>  
<br/>  

### 发布日志  
- 2019年发布日志 [[已归档]](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/update/2019.md)
- 2020年发布日志 [[进行中]](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/update/2020.md)  
  - 2020-08-06 ： 发布 v4.1.0.0，支持域内动态编程时插件 Using 引用覆盖 以及 动态生成程序集的 Using 引用覆盖， 减少开发者在域内编程时对 Using 的过多关注。 升级 Natasha.Framework; 添加 GetReferenceElements API 以便返回当前域所有的引用，增加 AddAssemblyEvent/RemoveAssemblyEvent 事件，在程序集加载与移除操作时触发。 增加方法返回值的 ref 修饰的反解。增加 Natasha.CSharp.All 库，提供组件库的自动引用。  
    
  - 2020-10-10 ： 发布 Natasha.CSharp.All v2.0.0.0，重整项目结构，分离出 C# 相关组件，修复域管理操作类对域的弱引用关系，完善周边类库。   
    
  - 2020-10-28 ： 发布 Natasha.CSharp.All v2.0.1.1，使用初始化开关，避免多次初始化调用，支持 netcore3.1 Runtime 版本。  
    
<br/>  
<br/>  
  
### 生态周边  
| 项目名称 | 项目地址 | 项目简介 |
|------------- |-----------| --------|
| NatashaPad | [Github](https://github.com/night-moon-studio/NatashaPad) | 由 Roslyn 和 Natasha 支持的另一种dotnet调试工具，如 LinqPad 和 dotnet fiddle。 |
| NCaller | [Github](https://github.com/night-moon-studio/NCaller) | 基于 Natasha 和 查找树算法的高速反射类，可以操作对象的属性以及字段。 |
| DeepClone | [Github](https://github.com/night-moon-studio/DeepClone) | 由 Natasha 的高性能深度克隆库。 |
| BTFindTreee | [Github](https:https://github.com/dotnet-lab/BTFindTreee) | 快速查找算法的构建，包括哈希二分查找，字串模糊查找，字串归并精确查找。 |
| Papper | [Github](https:https://github.com/dotnet-lab/Papper) | 对语法树解析库，主要目标时服务于 SG(Source Generator)技术。 |
| RuntimeToDynamic | [Github](https://github.com/night-moon-studio/RuntimeToDynamic) | 将运行时数据压入到动态代理类中，以方便其他动态构建时对其进行复用。|
| DynamicCache | [Github](https://github.com/night-moon-studio/DynamicCache) | 高速动态缓存，在只读并发场景中提供超高性能的数据查找功能。|
| Aries | [Github](https://github.com/night-moon-studio/Aries) | 对 FreeSql 的高度封装，提供高性能、直观的 外联 / 乐观锁 / CURD 操作。|

<br/>
<br/>  

### 开发计划
#### 2.0+ 计划  

 - [ ] 调研 .NET5 中性能优化的新特性。
 - [ ] 完善 UT 测试
 - [ ] 挣钱, 生存下来
 
#### 周边项目计划

 - [ ] BTF 算法
    - [ ] 每周定时跑算法随机测试程序
    - [ ] 持续评估 `span` 序列比较方法 及 指针转换比较 的性能
    - [ ] 评估 Trie 及变种 与 BTF 算法的性能差距
 - [ ] 元数据
    - [ ] 优化性能
    - [ ] 精确解析
    - [x] 持续评估封装架构的设计方案
 - [ ] 改造 [NCaller](https://github.com/night-moon-studio/NCaller) 
    - [x] 持续优化性能
    - [ ] 私有支持
    - [ ] 设计动态变现功能和实现
    - [ ] 设计只读功能维度和实现
    - [x] 评估 [NCaller](https://github.com/night-moon-studio/NCaller) 代理方式 和 [DynmaicCache](https://github.com/night-moon-studio/DynamicCache) 代理方式 在 R2D 模板下的异同及抽象
    - [x] 优化静态自动机代码
    - [x] 实用委托指针优化性能
    
 - [ ] 依赖还原库
    - [ ] NET 模块
    - [ ] NUGET 模块
    - [ ] FOLDER SCAN 模块
    - [ ] 跨平台 模块
    
 - [ ] 定制语法/语法糖 to Natasha
 - [ ] 持续评估 Natasha 在灵活授权模型上的应用
 - [ ] 持续搜集反编译的需求
 - [ ] 调研 JAVA to C#
 - [ ] 调研 GO to C#
 - [ ] 考虑要不要调研 PHP to C#
 - [ ] 谁能来帮我一起搞，帮我点上左边的小对号？
 
> 欢迎大家提交PR 

<br/>  
<br/>  

### 性能测试
      
   - [x]  **动态调用性能测试（对照组： emit, origin）**  
     ![字段性能测试](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%951.png)
   - [x]  **动态初始化性能测试（对照组： emit, origin）**  
     ![初始化性能测试](https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha%E6%80%A7%E8%83%BD%E6%B5%8B%E8%AF%952.png)
   - [x]  **内存及CPU监测截图**  
     ![内存及CPU](https://github.com/dotnetcore/Natasha/blob/master/Image/%E8%B5%84%E6%BA%90%E7%9B%91%E6%B5%8B.png)      

<br/>    
<br/> 

### 赞助：

<img width=200 src="./Image/%E8%B5%9E%E5%8A%A9.jpg" title="Scan and donate"/>

#### 捐助明细  

- ****天下 10元  
- **航 5元

<br/>  

---------------------  


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      

