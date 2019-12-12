<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/ndomain-method.html"> English </a>
</p> 


NDomain的静态方法，可以动态构建出Func / Action系列的委托。

```C#
//在随机域创建一个委托
var func1 = NDomain.Random().Func<int>("return 111;");
//创建一个叫 Jim 的域, 并在域内创建一个委托
var func2 = NDomain.Create("Jim").Func<int,string>("return arg.ToString();");
//直接在系统域创建一个委托
var func3 = NDomain.Default.Func<int,int,string>("return (arg1+arg2).ToString();");
.....
```  
 
 <br/>  
 
Func / AsyncFunc / UnsafeFunc / AsyncUnsafeFunc 的第二个参数是命名空间，可以直接扔一个 Assembly ,或者精确的传 Type ,或者直接写 String .
精准传参可以有助于 Natasha 解决二义性引用命名空间的问题

```C#

method(script, "System", assembly, tyypeof(Console)); 

例如：
NDomain.Default.Func<int,int,string>("return (arg1+arg2).ToString();","System","System.IO");
```  
 
 <br/>  
 
由于是可变参数，所以你可以传多种多个  

```C#  

method(script, "System", "System", "System"); 
method(script, assembly, assembly, assembly); 
method(script, tyypeof(Console), tyypeof(Console), tyypeof(Console));   

```
