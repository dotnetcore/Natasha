## 仿制方法操作类 FakeMethodOperator

该类继承自 MethodBuilder 拥有全部构建方法的API: 

- UseMethod : 将方法元数据中的信息反射出来，进行仿制，构建出相同的委托。

- Methodbody ： 添加方法内容

- StaticMethodBody : 将方法转为静态方法，并添加方法内容（常用）  

该类的初始化模板为：
```C#
using

   public static class [random_name]{
   
         [ 需要自己构建 ]
   }
```
