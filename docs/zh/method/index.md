<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/log/index.html"> English </a>
</p> 

## 动态方法

动态方法是最常用的动态编程手段之一，其生成的委托会被缓存起来，以便后续使用。
Natasha为此提供了极其方便的方法构建操作类，使用原生C#代码构建运行时动态方法，极大地提高了开发者的动态编程效率。
方便了后人的维护工作。  

<br/>

## 操作类封装

Natasha 从三种封装深度对方法操作类进行了封装。  

- Operator 原始操作类，最为灵活。
- NewMethod Operator的包装方法，以静态和委托的方式来构建一个动态方法。
- NFunc/NAction Operator的包装方法，以静态的方式模拟Func/Action类构建一个动态方法。

