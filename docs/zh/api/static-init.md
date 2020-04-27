
<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/static-init.html">English</a>
</p>  


## 静态初始化操作

Natasha 的所有模板均继承自 ComplierTemplate ，ComplierTemplate 本身会提供静态构造方法。因此上层 API 也会被支持。

<br/>

## 使用

 NDelegate / NAssembly / NClass.. / xxx_Oerator 等等以下称为 “Handler”.
 
<br/>

#### 静态初始化代码：

```C#

//使用 domain 域
Handler.UseDomain(domian, compiler => { 编译器配置 });


//使用某编译器的域
Handler.UseCompiler(assemblyCSharpCompiler, compiler => { 编译器配置 }));


//创建一个 "domainJim" 域
Handler.CreateDomain("domianJim", compiler => { 编译器配置 });


//使用默认域
Handler.DefaultDomain(compiler => { 编译器配置 });


//使用随机域
Handler.RandomDomain(compiler => { 编译器配置 });

```  
<br/>  

#### 编译器配置：

```C#
builder => 
{ 
     builder
       .CustomerUsing()                    //使用用户自定义的Using
       .SetAssemblyName("MyAssemblyName")  //设置程序集名
       .ThrowAndLogCompilerError()         //抛出并记录编译器的异常 
       .ThrowSyntaxError()                 //抛出语法树异常
       .UseStreamCompile();                //使用流编译
}
```

