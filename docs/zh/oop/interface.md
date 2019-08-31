<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/oop/interface.html"> English </a>
</p> 



快速创建类：
```C#

//创建一个类并获取类型
var type = new OopOperator()
  .Namespace<string>()
  .ChangeToInterface()
  .OopAccess(Access.None)
  .OopName("TestUt3")
  .OopBody(@"static void Test();")
  .GetType();

```

或

```C#

//创建一个类并获取类型
var type = NewInterface.Create(builder=>builder
  .Namespace<string>()
  .ChangeToInterface()
  .OopAccess(Access.None)
  .OopName("TestUt3")
  .OopBody(@"static void Test();")
  );

```

或  

```C#

//创建一个类并获取类型
var type = new NInterface()
  .Namespace<string>()
  .Namespace<string>()
  .ChangeToInterface()
  .OopAccess(Access.None)
  .OopName("TestUt3")
  .OopBody(@"static void Test();")
  .GetType();

```
