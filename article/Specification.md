# Natasha  封装规约

<br/>

## 一个完整的Operator

完整的Operator包括三部分：

-  Builer
-  Extension (为了方便可以写些扩展)
-  Function (根据自己的需求定制)

<br/>


## 脚本构建器（Builder）  

作为Operator最重要的核心部分，Builder分为两部分 模板与编译器：
 
<br/>  

   - Template 构建模板  
        
        使用 Template 模板构建运行时脚本字符串，Natasha内置了诸多模板，可以方便使用者构建脚本。          
        
       - Oop(class/struct/interface) 集成了类、结构体、接口的脚本模板，可以对面向对象的结构进行动态构建。
         
       - OnceMethod 为快速构建一个方法的模板，继承了Oop模板，可以解决常用一次性委托构建的问题。  
       
       - Member 为了给成员提供一个公用的保护级别、修饰符等必备构建信息。
         
       - Method/Ctor/Field 是继承了Member模板，生成的结果为单个的函数/字段体，可以结合Oop模板完整的构建面向对象的结构。
         
<br/>  
     
   - Complier 编译器
     
        为了对接编译引擎和搜集异常，Natasha使用抽象类IComplier进行基础的编译工作，
        包括：选择编译方式/获取程序集/获取类型/获取方法元数据/获取委托。  
        
        引擎之外便是编译层，其中实现了OopComplier/MethodComplier
        
      - OopComplier : 在抽象编译器的基础上，区分了获取类、结构体以及接口的方法。  
      
      - MethodComplier : 在抽象编译器的基础上，对获取委托方法进行了包装。
        
        

<br/>


## Operator

Operator 在 Builder的基础上进行封装，Builder提供了脚本构建以及编译的大部分功能，因此，Operator的封装需要更专注功能及扩展的开发。    

对于扩展而言，Operator或者Builder写好之后，可以根据需要，封装一个扩展方法，给用户使用。  

Operator的功能是根据自己的需求进行定制的。

#### 案例  

例如 FastMethodOperator 在 OnceMethodBuilder 的基础上进行了包装和简化，FastMethodBuilder 的初始化函数中定制了一个专属自己的脚本构建流程，如下图：

```C#
HiddenNameSpace()
 .OopAccess(AccessTypes.Public)
 .OopModifier(Modifiers.Static)
 .MethodAccess(AccessTypes.Public)
 .MethodModifier(Modifiers.Static);

```  

隐藏命名空间，类使用Public保护级别和静态修饰符，方法使用Public保护级别和静态修饰符，那么这个固定的构造流程将生成如下代码：

```C#

using XXXX;
public static class XXXXX{
 public static X Invoke(){
   xxxxxx
 }
}

```
在编译之后，我们可以拿到Invoke委托函数，就可以直接用了。
