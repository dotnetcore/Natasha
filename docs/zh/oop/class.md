<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/oop/class.html"> English </a>
</p> 


快速创建类：
```C#

//创建一个类并获取类型
var type = new OopOperator()
  .Namespace<string>()
  .OopAccess("")
  .OopName("TestUt3")
  .ChangeToInterface()
  .Ctor(item => item
    .MemberModifier(Modifiers.Static)
    .Param<string>("name")
    .Body("this.Name=name;"))
  .OopBody(@"public static void Test(){}")
  .PublicStaticField<string>("Name")
  .PrivateStaticField<int>("_age")
  .GetType();


```

或

```C#

//创建一个类并获取类型
var type = NewClass.Create(builder=>builder
  .Namespace<string>()
  .OopAccess("")
  .OopName("TestUt3")
  .ChangeToInterface()
  .Ctor(item => item
    .MemberModifier(Modifiers.Static)
    .Param<string>("name")
    .Body("this.Name=name;"))
  .OopBody(@"public static void Test(){}")
  .PublicStaticField<string>("Name")
  .PrivateStaticField<int>("_age")
  );

```

或  

```C#

//创建一个类并获取类型
var type = new NClass()
  .Namespace<string>()
  .OopAccess("")
  .OopName("TestUt3")
  .ChangeToInterface()
  .Ctor(item => item
    .MemberModifier(Modifiers.Static)
    .Param<string>("name")
    .Body("this.Name=name;"))
  .OopBody(@"public static void Test(){}")
  .PublicStaticField<string>("Name")
  .PrivateStaticField<int>("_age")
  .GetType();

```


