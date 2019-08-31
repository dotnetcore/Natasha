<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/delegate-method.html"> English </a>
</p> 

再FakeMethodOperator基础上，构建了DelegateOperator<T>操作类。
 
 
 ```C#
 var action = DelegateOpeartor<Func<string>>.Delegate("return \"1\";");
 action(); //结果是1
 ```
 
 同时还有几个方法：
 ```C#
//异步方法
DelegateOpeartor<T>.AsyncDelegate
//非托管方法
DelegateOpeartor<T>.UnsafeDelegate
//异步非托管方法
DelegateOpeartor<T>.UnsafeAsyncDelegate
 ```
