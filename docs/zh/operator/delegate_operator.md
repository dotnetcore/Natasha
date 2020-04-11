## 委托操作类 DelegateOperator

该类封装了 FakeMethodOperator 将自动构建传进来的委托信息，并提供了以下方法：

 - Delegate 原版委托
 - AsyncDelegate 异步委托
 - UnsafeDelegate 非安全委托
 - UnsafeAsyncDelegate 非安全异步委托
 
 以上方法的参数均为：
 
 
  string content,     委托字符串内容  
  DomainBase domain = default,   当前所在的域  
  Action<AssemblyCSharpBuilder> option = default,   对编译器的配置  
  params NamespaceConverter[] usings   自定义的命名空间引用  
  
  使用案例：
```C#  

  public delegate void ValueDelegate(CallModel model, in DateTime value);
  DelegateOperator<ValueDelegate>.Delegate("model.CreateTime=value;"); 
```
