<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/nfunc-method.html"> English </a>
</p> 


NFunc静态方法，可以动态构建出Func系列的委托。

```C#
var func1 = NFunc<int>.Delegate("return 111;");
var func2 = NFunc<int,string>.Delegate("return arg.ToString();");
var func3 = NFunc<int,int,string>.Delegate("return (arg1+arg2).ToString();");
.....
```  
 
 <br/>  
 
NFunc的第二个参数是命名空间，可以直接扔一个Assembly,或者精确的传Type,或者直接写String.

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
