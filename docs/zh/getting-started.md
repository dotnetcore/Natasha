## 简介

Natasha 支持您在运行时进行脚本交互，创建动态方法，典型场景例如类型映射，远程调用。
如果您还没有用过此类的应用可以尝试一些例如AutoMapper/Dapper/Json.net之类的名库，
体验一下它们的快捷与方便，如果您继续深入探索便会发现一些OpCodes样式的代码,那便是用于动态构建的代码。

.NET允许在你程序运行时，根据逻辑再次生成另外一些功能，从技术角度来看，
程序运行的周期大致如下，C#代码，被编译成IL代码，被转化成本地指令，
当您的程序开始运行时，IL代码变开始起了作用，并且.NET虚拟机支持你在运行时再次注入IL代码，
这有点像插件，在运行时加载到程序中运行。  

<br/>  

## 场景

Emit和表达式树的使用场景，Natasha均适用。  


<br/>  

## 使用群体

首先来说本类库并不是为初学者准备的，而是需要有一定的封装基础，有一定的动态编程技巧的人。  
尽管Natasha入门十分简单，但如果您没有基础知识和经验的话还是不知道它能用在何处。  


<br/>  


## 安装

如果您的项目是.NetCore项目，那么您可以使用Natasha.    

 - 命令  
 `Install-Package DotNetCore.Natasha -Version 1.3.4`  
 
 - nuget  
  `DotNetCore.Natasha`  


<br/>  


## 准备工作

您需要在工程文件中添加一些引用所需的标签：

```C#

  <PropertyGroup>
  
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    
    //要加上这句话
    <PreserveCompilationContext>true</PreserveCompilationContext> 
    //WEB发布一定要加
    <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish> 
    
  </PropertyGroup>
 
```  


<br/>  

##  第一个HelloWorld

```C#

//你需要准备一个字符串
string script = "Console.WriteLine(""Hello World!"")";


//然后像这样用
var action = NAction.Delegate(script,typeof(Console));
action();


//或者这样
var action = script.Delegate<Action>(typeof(Console));
action();

```

<br/>  
