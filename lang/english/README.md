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

### User Api：   
| Order | Operation | Target | Description |
|-- |-------- |--------------| --------|
| 1 | Reference Library | DotNetCore.Natasha.CSharp.All |  |
| 2 | Initialization | NatashaInitializer.InitializeAndPreheating(); / Initialize(); | Preheating will take some time, and later compilation will be fast, or you can only initialize without preheating. |
| 3 | Write code | instructions [https://natasha.dotnetcore.xyz/](https://natasha.dotnetcore.xyz/) | Contact me: 2765968624@qq.com or [FAQ](https://github.com/dotnetcore/Natasha/blob/master/docs/FAQ.md) or [create issue](https://github.com/dotnetcore/Natasha/issues/new) |

<br/>  
<br/>  

### 发布日志  
- Release log in 2019 [[Done]](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/update/2019.md)
- Release log in 2020 [[In process]](https://github.com/dotnetcore/Natasha/blob/master/docs/zh/update/2020.md)  
  - 2020-08-06 ： 发布 v4.1.0.0，支持域内动态编程时插件 Using 引用覆盖 以及 动态生成程序集的 Using 引用覆盖， 减少开发者在域内编程时对 Using 的过多关注。 升级 Natasha.Framework; 添加 GetReferenceElements API 以便返回当前域所有的引用，增加 AddAssemblyEvent/RemoveAssemblyEvent 事件，在程序集加载与移除操作时触发。 增加方法返回值的 ref 修饰的反解。增加 Natasha.CSharp.All 库，提供组件库的自动引用。  
    
  - 2020-10-10 ： 发布 Natasha.CSharp.All v2.0.0.0，重整项目结构，分离出 C# 相关组件，修复域管理操作类对域的弱引用关系，完善周边类库。   
    
  - 2020-10-28 ： 发布 Natasha.CSharp.All v2.0.1.1，使用初始化开关，避免多次初始化调用，支持 netcore3.1 Runtime 版本。  
  
  - 2020-11-14 ： 发布 Natasha.CSharp.All v2.0.2.1，支持 .NET5 ，初始化函数增加参数，false 代表不初始化默认域的引用，此时引用需要在域中手动添加。 
  
  - 2020-11-20 ： 发布 Natasha.CSharp.All v2.0.2.2，支持 .NET5 / C# 9 ；增加 NRecord 构建模板；增加 RecordProperty<T>(name) 只读包装属性，增加 OOP 模板 Property 构建是关于 init 类型的 API； 增加支持 fixed 修饰符；.NET5 版本 增加 SkipInit 方法跳过初始化，即 SkipLocalsInit 特性（注解），可用在存储结构构建以及方法上；修复日志输出格式。
  
  - 2020-11-24 ： 发布 Natasha.CSharp.All v2.0.2.3，修复禁断警告功能，升级周边类库依赖。
    
<br/>  
<br/>  
  
### 生态周边  
| 项目名称 | 项目地址 | 项目简介 |
|------------- |-----------| --------|
| NatashaPad | [Github](https://github.com/night-moon-studio/NatashaPad) | 由 Roslyn 和 Natasha 支持的另一种dotnet调试工具，如 LinqPad 和 dotnet fiddle。 |
| Leo | [Github](https://github.com/night-moon-studio/NCaller) | 基于 Natasha 和 查找树算法的高速反射类，可以操作对象的属性以及字段。 |
| DeepClone | [Github](https://github.com/night-moon-studio/DeepClone) | 由 Natasha 的高性能深度克隆库。 |
| BTFindTreee | [Github](https:https://github.com/dotnet-lab/BTFindTreee) | 快速查找算法的构建，包括哈希二分查找，字串模糊查找，字串归并精确查找。 |
| Papper | [Github](https:https://github.com/dotnet-lab/Papper) | 对语法树解析库，主要目标时服务于 SG(Source Generator)技术。 |
| RuntimeToDynamic | [Github](https://github.com/night-moon-studio/RuntimeToDynamic) | 将运行时数据压入到动态代理类中，以方便其他动态构建时对其进行复用。|
| DynamicDictionary | [Github](https://github.com/night-moon-studio/DynamicCache) | 高速动态缓存，在只读并发场景中提供超高性能的数据查找功能。|
| Aries | [Github](https://github.com/night-moon-studio/Aries) | 对 FreeSql 的高度封装，提供高性能、直观的 外联 / 乐观锁 / CURD 操作。|

<br/>
<br/>  

### 开发计划
#### 2.0+ 计划  

 - [ ] 编写英文文档，以后将以英文文档为主
 - [x] 调研 .NET6 中性能优化的新特性
 - [ ] 完善 UT 测试
 - [ ] 挣钱, 生存下来
 
#### 周边项目计划

 - [ ] BTF 算法
    - [ ] 每周定时跑算法随机测试程序
    - [x] 持续评估 `span` 序列比较方法 及 指针转换比较 的性能
    - [ ] 评估 Trie 及变种 与 BTF 算法的性能差距
 - [ ] 元数据
    - [ ] 优化性能
    - [x] 精确解析
    - [x] 持续评估封装架构的设计方案
 - [ ] 改造 [Leo](https://github.com/night-moon-studio/Leo) 
    - [x] 持续优化性能
    - [x] 私有支持
    - [ ] 评估是否支持 AOP 
    - [x] 评估 [Leo](https://github.com/night-moon-studio/Leo) 代理方式 和 [DynmaicDictionary](https://github.com/night-moon-studio/DynmaicDictionary) 代理方式 在 R2D 模板下的异同及抽象
    - [x] 优化静态自动机代码
    - [x] 使用委托指针优化性能
    
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
      
   - [x]  **动态初始化性能测试（对照组： emit, origin）**  
     ![初始化性能测试](https://images.gitee.com/uploads/images/2020/1201/161738_b54dd1ad_1478282.png)
   - [x]  **内存及CPU监测截图**  
     ![内存及CPU](https://images.gitee.com/uploads/images/2020/1201/161450_96e70709_1478282.png)      

<br/>    
<br/> 

### 赞助：

<img width=200 height=200 src="https://images.gitee.com/uploads/images/2020/1201/163955_a29c0b44_1478282.png" title="Scan and donate"/><img width=200 height=200 src="https://images.gitee.com/uploads/images/2020/1201/164809_5a67d5e2_1478282.png" title="Scan and donate"/>


#### 捐助明细  

- ****天下 10元  
- **航 5元

<br/>  

---------------------  


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_large)          
      
       
      
     
