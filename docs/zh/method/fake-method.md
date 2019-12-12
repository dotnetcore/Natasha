<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/fake-method.html"> English </a>
</p> 

域操作

```C#
FakeMethodOperator.Default             //系统域
FakeMethodOperator.Create("MyDomain")  //创建一个新的独立域
FakeMethodOperator.Random()            //使用一个随机域

//如果方法里传 bool 类型则可以告诉编译器，是否编译成DLL文件，默认是编译到内存。
```

我好累，简单的写吧。

先反射出MethodInfo, 这样用：

```C#

FakeMethodOperator.Default
.StaticMethodContent(methodInfo)
.Complie()
```
这样你就能得到一个Delegate类型的结果，想运行就强转。 （Action）result,这样。

或者这样：

```C#
FakeMethodOperator.Default
.StaticMethodContent(methodInfo)
.Complie<Action>()
```

DelegateOperator也是基于FakeMethodOperator
