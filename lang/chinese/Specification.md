# Natasha  封装规约

<br/>

## 一个完整的Operator

完整的Operator包括三部分：

-  Builer
-  Extension
-  Function

<br/>


## 脚本构建器（Builder）  

Builder分为两部分 模板与编译器
 
<br/>  

   - Template 构建模板  
        
        使用 Template 模板构建运行时脚本字符串，Natasha内置了诸多模板，可以方便使用者构建脚本。          
        
       - Oop(class/struct/interface) 集成了类、结构体、接口的脚本模板，可以对面向对象的结构进行动态构建。
         
       - OnceMethod 是自实现Oop模板，可以构建的最高级别为using，OnceMethod 作为单独的模板存在，是为了解决常用委托构建的问题。  
         
       - Method/Ctor/Field 是成员模板，生成的结果为单个的函数/字段体，可以结合Oop模板完整的构建面向对象的结构。
         
<br/>  
     
   - Complier 编译器
     
        为了对接编译引擎和搜集异常，Natasha使用抽象类IComplier进行基础的编译工作，
        包括：选择编译方式/获取程序集/获取类型/获取方法元数据/获取委托。  
        
        引擎之外便是编译层，其中实现了OopComplier/MethodComplier
        
      - OopComplier : 在抽象编译器的基础上，区分了获取类、结构体以及接口的方法。  
      
      - MethodComplier : 在抽象编译器的基础上，对获取委托方法进行了包装。
        
        
