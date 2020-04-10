### Natasha 3.0+ 抽象了引擎结构，分离出了框架以及各个模块。  
  
<br/>  

#### 各模块的标准封装在 Natasha.Framework 中：

 - DomainBase: 继承自 AssemblyLoadContext 类，完成了部分域功能及部分抽象标准。
   
    - AssemblyReferences ： 编译需要的引用库，该字段以程序集/引用字典的形式储存了程序集对应的引用。
    - GetInstance ： 该方法需要重载，以便于 DomainManagement 类的 Create 操作， 需要返回你当前类的实例，需要重载。
    - Default_Resolving ： 在系统域加载时触发的方法，需要重载。
    - Default_ResolvingUnmanagedDll ： 在系统域加载非安代码时触发的方法，需要重载。
    - CompileStreamHandler ： 当以流的方式成功编译时触发的方法，需要重载。
    - CompileFileHandler : 当以文件的方式成功编译时触发的方法，需要重载。
    - Remove ： 删除引用时触发的方法，需要重载。
    
    - GetDefaultReferences ： 该方法返回一个系统域的所有引用，可重载。
    - GetCompileReferences ： 该方法返回了系统域及非系统域组合的引用， 可重载。
    - LoadPluginFromFile ： 当插件以文件方式加载时触发的方法，可重载。
    - LoadPluginFromStream ： 当插件以流方式加载时触发的方法，可重载。
  
<br/>  

 - SyntaxBase: 作为语法转换的基础类，提供了代码及语法书缓存，规定了抽象方法，实现了自动添加缓存的方法。
 
    - TreeCache ： 存放字符串代码及语法书的缓存。
    - LoadTreeFromLauguage ： 每个语言都有自己的转换方法，但最终需要返回 SyntaxTree ,当您继承该类时，需要实现该方法，返回对应语言的语法树，需要重载。
    - AddTreeToCache ： 该方法会自动将对应语言的代码及语法树缓存起来，可重载。
  
<br/>  
   
 - CompilerBase<T>: 编译器抽象， T 被约束为 Compilation 类型，该类为编译的基础类，在构建编译信息时，每种语言都会对改类进行继承改造，因此它是编译基础。
 
    - AssemblyName : 编译器会对当前代码进行整程序集编译，需要指定程序集名。
    - Assembly : 编译结果。
    - AssemblyOutputKind ：Assembly 输出的方式，编译到文件 file / 编译到流 stream。
    - Domain : 该属性承载了 DomainBase 实例。
    - PreComplier ： 该方法在编译前执行，如果返回 false 将阻止编译， 可重写。
     - CompileToFile ： 该方法实现了将以上信息编译到文件的功能，可重写。
    - CompileToStream : 该方法实现了将以上信息编译到流的功能，可重写。
    - Compile ：该方法实现了根据输出方式（AssemblyOutputKind）进行自动编译，file 调用 CompileToFile 方法， stream 调用 CompileToStream 方法。
    - CompileTrees ： 需要被编译的语法树。
    - GetCompilation ：返回不同语言的编译信息集，必须重写。
    
    - FileCompileSucceedHandler ： 当文件形式编译成功之后引发的事件。
    - StreamCompileSucceedHandler ：当流形式编译成功之后引发的事件。
    
    - FileCompileFailedHandler ： 当文件形式编译失败之后引发的事件。
    - StreamCompileFailedHandler ： 当流形式编译失败之后引发的事件。
    


<br/>  

 对以上类进行重写，即可完成一门语言的动态编译，详情请看 Engine 实现篇。
