## 问题汇总    

 - #### 问：学习这个需要哪些必备知识？
 
   - 答：
    
     - 要了解域、程序集、类型、方法/属性/字段它们之间的关系；
     - 域是大量动态构建的隔离容器，你需要在所有操作之前，最先考虑它的存在，尤其特别是你在做插件开发时，具体操作可参见文档。
     - Natasha 注重 C# 语法你需要遵从 C# 的语法来构建动态的需求；
     - 可以用模板 NClass / NInterface / NEnum / NStruct 来构建动态需求。
     - 也可以直接传字符串到 AassemblyCSharpBuilder 来构建你的需求，详细可参见文档。
     - using 是你需要特别注意的地方， Natasha 默认启用添加所有 Using, 也可以使用 CustomUsing 方法来切换到自定义的 Using.
     - Natasha 初衷是为了替代 Emit，所以如果你有 Expression 或 Emit 的编程基础，想必可以迅速上手；
     - 当你使用该库进行高级且复杂的封装时，你要清楚，这项技术带来的配置维度要比目前市面所有的类库配置维度要多。
     - Natasha 周边有 R2D / DynamicCache 可以为你的类库及代码提供性能加速。
     - 及时反馈你的问题，我可以尽快回复。
     
 <br/>  
 

 - #### 问：脚本如果以前编译过，Natasha 会跳过自动不编译？还是用户自己处理？
 
    - 答：Natasha 只提供编译功能，像这种需求开发者多写两行代码就得了。                               

 <br/>  
 



 - #### 问：写进去的C#代码是自动编译成dll了是吗？
 
    - 答：Natasha 只有一个编译器 AssemblyCSharpBuilder 类，它会存在每个操作类中，找到它，  
     Natasha 提供了扩展方法 UseFileCompile / UseStreamCompile 两个操作编译行为的方法，  
     其中 UseStreamCompile 只会编译到内存流，而 UseFileCompile 则会编译入 dll 文件中。

 <br/>  
 
  - #### 问：编译成的 dll 需要我重新加载吗？
 
    - 答：不需要，编译完就已经在运行时里面了，不用再加载一次。

 <br/>  
 
  - #### 问：这个库性能如何？  
  
    - 答：
    
      - 无限接近原生性能
      - 在不断优化
      - 性能对比： [DynamicCache 新一轮性能提升](https://github.com/night-moon-studio/DynamicCache/blob/master/image/%E6%80%A7%E8%83%BD%E6%94%B9%E8%BF%9B.png)

<br/>
 
- #### 问：Using 怎么处理？

  - 答：  
  
    - 用编译器： AssemblyCSharpBuilder 直接编译字符串，就在你的字符串里写好。  
    - 用 NClass / NInterface / NStruct / NEnum 模板：默认自动添加所有 using。  
    - 用 NClass / NInterface / NStruct / NEnum 模板：.CustomeUsing() 方法让操作类在整个操作周期搜集 using。  
    - 用 NClass / NInterface / NStruct / NEnum 模板：.Using() 方法手动强制添加 using。
    - 用NDelegate:  在Action(code,) 第二个参数以后添加 using。  
    
 <br/>
 
 - #### 问：为什么很多英文单词拼写错误？  
 
   - 答：  
   
      - 不会英语，日语、韩语、阿拉伯语也不会
      - 拼音也不建议用
      - 年老体弱，老眼昏花
        
 <br/>  
 
  - #### 问：这个库靠谱吗？  
  
    - 答：  
    
      - 不靠谱，靠手，得手敲
      - 不靠谱，Natasha + 周边项目的单元测试只有百十来个
