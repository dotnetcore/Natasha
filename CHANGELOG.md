<!--
This project adheres to [Semantic Versioning](https://semver.org).
Note: In this file, do not use the hard wrap in the middle of a sentence for compatibility with GitHub comment style markdown rendering.


发布日志节点案例格式如下(支持多版本发布):
Example:

## [1.0.2] - 2023-03-10 
 
###  mynuget _ v4.0.0.0:
- 初步版本 简单实现 还未优化
###  xxx.xxx _ v1.2.0:
- 初步版本 简单实现 还未优化
- fix warning 
###  myproject1 _ v3.2.0-beta.2:
- 初步版本 简单实现 还未优化
- fix warning 
###  myproject2 _ 1.0.0:
- 初步版本 简单实现 还未优化
- fix warning 

## [5.2.0.0] - 2023-04-02

###  DotNetCore.Natasha.CSharp _ v5.2.0.0:
- 使用 NMS Template 管道接管 CI 功能.
- 取消 SourceLink.GitHub 的继承性.
- 增加 全局 using 配置文件以支持全局 using 引用.
- 增加初始化 PE 信息判断, 不加载无效 DLL 文件.
- 整改 AssemblyCSharpBuilder, 修改几处 API:
 - 增加 GetAvailableCompilation, 开发者使用此API可以进行单独编译信息整合以及语义语法修剪,其结果为 Compilation 属性, 为下一步编译程序集做准备.
 - 增加 ClearCompilationCache 移除当前 编译单元的编译信息, 运行 GetAvailableCompilation/GetAssembly 将重新构建编译信息.
 - 增加 WithRandomAssenblyName 将当前编译单元的程序集名更改为 GUID 随机名.
 - 增加 ClearScript 清除当前编译单元储存的所有C#脚本代码.
 - 增加 Clear 清除脚本代码,清除编译信息,清除程序集名.
- **[破坏性更改]** 下列 API, 从 AssemblyCSharpBuilder 的扩展方法 更改为 Assembly 类型的扩展方法:
 - GetTypeFromShortName / GetTypeFromFullName, 
 - GetMethodFromShortName / GetMethodFromFullName
 - GetDelegateFromShortName / GetDelegateFromFullName
> 使用迁移: builder.GetDelegateFromShortName() 更改为 builder.GetAssembly().GetDelegateFromShortName();
> builder.GetAssembly() 仍然不可多次编译, 请及时缓存结果.
- 新增 Type 的扩展 API:
 - GetDelegateFromType , 参考 GetDelegateFromShortName 的用法.
###  DotNetCore.Natasha.Domain _ v5.1.0.0:
- 取消 SourceLink.GitHub 的继承性.
- 增加 全局 using 配置文件以支持全局 using 引用.


-->
## [5.1.0.0] - 2023-04-02

###  DotNetCore.Natasha.CSharp _ v5.1.0.0:
- Github 补充发布 Release.

###  DotNetCore.Natasha.Domain _ v5.0.0.0:
- Github 补充发布 Release.

