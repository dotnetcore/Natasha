<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/fake-method.html"> English </a>
</p> 

我好累，简单的写吧。

先反射出MethodInfo, 这样用：

```C#
FakeMethodOperator.New
.StaticMethodContent(methodInfo)
.MethodContent()

.Complie()
```
这样你就能得到一个Delegate类型的结果，想运行就强转。 （Action）result,这样。

或者这样：

```C#
FakeMethodOperator.New
.StaticMethodContent(methodInfo)
.MethodContent()

.Complie<Action>()
```

DelegateOperator也是基于FakeMethodOperator
