<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/oop/index.html"> English </a>
</p> 

## 构建数据

我们常用存储结构包括各种类、结构体、接口、枚举，Natasha支持开发者动态的构建这些类型。
由于以上几种类型在命名空间的包裹中，有些共性，而且都可以独立存在于命名空间中。
经过抽象之后，Natasha统一采用OopBuilder操作进行4种结构的构建。  

OopBuilder是由OopContentTemplate以及OopComplier组成的。  

 - Template负责脚本字符串的构建工作。
 - Complier负责字符串的编译及异常搜集工作。  
 
按照Natasha的封装规约，Builder是不宜直接交给用户使用的，对外交付的应该是Operator操作类，因此Natasha使用了OopOperator操作类包裹了OopBuilder,
尽管OopOperator没有进行任何操作和调用，但它仍然是有意义的，意义就是对接用户。  

```C#
public class OopOperator : OopBuilder<OopOperator>
{
        
   public OopOperator()
   {
       Link = this;
   }

}

```  

## 结构分解

如图，一个完整的结构，将被拆分成以下几部分：  

![Struction](https://github.com/dotnetcore/Natasha/blob/master/Image/OopStruct.png)

此图右边批注对应的是Natasha中模板的方法，您只需要`.`然后根据提示找到方法即可。  

> 同一行的批注，代码从左到右，方法从上到下。
