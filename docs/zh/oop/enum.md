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
