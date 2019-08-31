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
