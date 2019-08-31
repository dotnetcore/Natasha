<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/method/layer1.html"> English </a>
</p> 


## 轻度封装

该层操作类是在Builder上进行封装，最为灵活，用户同样可以包装Builder类，Builder类包括模板和抽象编译器，模板生成字符串，编译器进行编译并返回。
仅此而已，不是很难理解。  

此封装分为2类：

- 模板定制方法， 使用Natasha提供得LINQ操作类定制模板。
- 元数据定制方法，先反射出MethodInfo,然后仿制一个模板。

在元数据定制方法的基础上是委托的操作类实现，因为委托可以直接利用元数据生成方法。
委托实现之后是字符串的扩展实现，字符串可以单独和委托进行组合并生成方法。

