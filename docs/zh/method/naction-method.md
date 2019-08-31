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
