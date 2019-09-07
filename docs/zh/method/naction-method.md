<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/naction-method.html"> English </a>
</p> 


NAction静态方法，可以动态构建出Action系列的委托。

```C#
var action = NAction.Delegate("Console.WriteLine(11);");
var action1 = NAction<int>.Delegate("Console.WriteLine(arg);");
var action2 = NAction<int,string>.Delegate("Console.WriteLine(arg1);Console.WriteLine(arg2);");
.....
```
 
 <br/>  
 
NAction的第二个参数是命名空间，可以直接扔一个Assembly,或者精确的传Type,或者直接写String.

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
