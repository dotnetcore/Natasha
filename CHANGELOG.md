<!--
This project adheres to [Semantic Versioning](https://semver.org).
Note: In this file, do not use the hard wrap in the middle of a sentence for compatibility with GitHub comment style markdown rendering.

发布日志节点案例格式如下(支持多版本发布):


Example:

## [5.1.0.0] - 2023-04-02

### DotNetCore.Natasha.CSharp _ v5.1.0.0:
- Github 补充发布 Release.

### DotNetCore.Natasha.Domain _ v5.0.0.0:
- Github 补充发布 Release.

-->

## [8.0.0.0] - 2024-01-10 

### DotNetCore.Natasha.DynamicLoad.Base _ v8.0.0.0:
- INatashaDynamicLoadContextBase 接口来规范域的行为.
- INatashaDynamicLoadContextCreator 接口来规范创建域以及其他 Runtime 方法的实现.


### DotNetCore.Natasha.Domain _ v8.0.0.0:
- 优化域加载时程序集比对的逻辑.
- 相同依赖不会二次加载.


### DotNetCore.Natasha.CSharp.Compiler.Domain _ v8.0.0.0:
- 实现 `DotNetCore.Natasha.DynamicLoad.Base` 接口，支持 Natasha 域操作.


### DotNetCore.Natasha.CSharp.Compiler _ v8.0.0.0:
- 新增 智能模式、轻便模式、自定义模式 三种编译方式.
- 新增 NatashaLoadContext 统一管理元数据.
- 支持 实现程序集、引用程序集两种预热方式.
- 支持 动态断点调试.
- 支持 引用程序集输出.
- 支持 隐藏的 Release 模式.
- 全面兼容 Standard2.0.
- 优化预热性能.
- 优化预热内存涨幅.


### DotNetCore.Natasha.CSharp.Template.Core _ v8.0.0.0:
- 全面兼容 Standard2.0.
- 为 `DotNetCore.Natasha.CSharp.Compiler` 提供 .NET Core3.1+ 脚本模板支持.


### DotNetCore.Natasha.CSharp.Extension.Codecov _ v8.0.0.0:
- 全面兼容 Standard2.0.
- 支持动态程序集的方法使用率统计


### DotNetCore.Natasha.CSharp.Extension.Ambiguity _ v8.0.0.0:
- 全面兼容 Standard2.0.

