快速创建枚举：  

```C#

var script = new OopOperator()
    .HiddenNameSpace().ChangeToEnum()
    .OopAccess(AccessTypes.Public).OopName("EnumUT1")
    .EnumField("Apple")
    .EnumField("Orange",2)
    .EnumField("Banana")
    .Builder().Script;



/* result: 
 
public enum EnumUT1{
   Apple,
   Orange=2,
   Banana}
   
*/
   
```  
因为调用了HiddenNameSpace方法，所以结果没有Namespace


也可以直接用NEnum来创建：

```C#

var script = NEnum
    .Namespace("aaa")
    .OopAccess(AccessTypes.Public).OopName("EnumUT1")
    .EnumField("Apple")
    .EnumField("Orange",2)
    .EnumField("Banana")
    .GetType();

```

或  

```C#

var script = NewEnum(builder=>builder
    .Namespace("aaa")
    .OopAccess(AccessTypes.Public).OopName("EnumUT1")
    .EnumField("Apple")
    .EnumField("Orange",2)
    .EnumField("Banana")
    );
   
```
