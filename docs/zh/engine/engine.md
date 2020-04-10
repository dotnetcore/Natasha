### Natasha.CSharpEngine 引擎采用了 Natasha.Framework 的框架，对其进行封装、集成。  

 <br/>  
 
Natasha.CSharpEngine 是由：Natasha.CSharpSyntax \ Natasha.CSharpCompiler \ Natasha.Exception \ Natasha.Domain 组成。

 <br/>  
 
 - 作为 C# 标准层，编译与语法库如下 ：
    - Natasha.CSharpSyntax： 实现了 Framework 中 SyntaxBase 标准， 重载 LoadTreeFromLauguage 方法为上层提供 C# 语法树。
    - Natasha.CSharpCompiler:实现了 Framework 中 ComplierBase 标准，重载 GetCompilation 方法为上层提供 C# 编译元。
 
 <br/>  
 
 - NatashaCSharpEngine 类为核心引擎, 由 NatashaCSharpSyntax 和 NatashaCSharpCompiler 组成，其中引擎部分注册了 ComplierBase 的事件，事件中实现了 NatashaCSharpSyntax 与 NatashaCSharpCompiler 配合自动纠错的功能：
    - NatashaCSharpSyntax : 继承自 Natasha.CSharpSyntax 中的 CSharpSyntaxBase，并增加了上层库本身需要的一些功能。
    - NatashaCSharpCompiler : 继承自 Natasha.CSharpCompiler 中的 CSharpCompilerBase，并增加了上层库本身需要的一些功能。
    - Natasha.Domain ： 实现了 Framework 中 DomainBase 标准，添加了自动向 DomainManagement 注册的功能， 添加了 插件-引用 管理，增强了动态编译与插件的互动。  
    - Natasha.Exception : 提供了 CompilationException 类，并集成在上述流程中，全程搜集编译异常。

<br/>  
  
 - AssemblyCSharpBuilder 为对外暴露的程序集编译类，该类继承自 NatashaCSharpEngine, 对语法树和编译器的配置 API 进行了封装，对编译流程进行了完整的把控。
 
<br/>  
  
  您可以使用 Natasha.CSharpSyntax 及 Natasha.CSharpCompiler 去实现自己的功能。
