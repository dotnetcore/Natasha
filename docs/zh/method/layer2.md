<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/layer2.html"> English </a>
</p> 


在第一层操作类的基础上，又封装了一下，让操作简单一些：

```C#
NewMethod.Create(builder);     //返回弱类型委托
NewMethod.Create<T>(builder);  //返回强类型委托
```  

这里的 builder 是FastMethodOperator实例。 
