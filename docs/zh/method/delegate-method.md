<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/delegate-method.html"> English </a>
</p> 

再FakeMethodOperator基础上，构建了DelegateOperator<T>操作类。
 
 ```C#
 
   Delegate(
     string content, 
     params NamespaceConverter[] usings
   )
   
   Delegate(
     string content, 
     AssemblyDomain domain = default, 
     bool inCache = false,
     bool complieInFile = false, 
     params NamespaceConverter[] usings
   )
   
```
 
 ```C#
 var action = DelegateOpeartor<Func<string>>.Delegate("return \"1\";");
 action(); //结果是1
 ```  
 
 <br/>  
 
 同时还有几个方法：
 ```C#
//异步方法
DelegateOpeartor<T>.AsyncDelegate
//非托管方法
DelegateOpeartor<T>.UnsafeDelegate
//异步非托管方法
DelegateOpeartor<T>.UnsafeAsyncDelegate
 ```  
  
 <br/>  
 
NamespaceConverter，可以直接扔一个Assembly,或者精确的传Type,或者直接写String.

```C#

method(script, "System", assembly, tyypeof(Console)); 

```  
 
 <br/>  
 
由于是可变参数，所以你可以传多种多个  

```C#  

method(script, "System", "System", "System"); 
method(script, assembly, assembly, assembly); 
method(script, tyypeof(Console), tyypeof(Console), tyypeof(Console));   

```
