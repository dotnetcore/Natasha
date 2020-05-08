## 问题汇总    

 - #### 问：脚本如果以前编译过，Natasha 会跳过自动不编译？还是用户自己处理？
 
    - 答：
     - Natasha 只提供编译功能，像这种需求，开发者自己多写两行代码就得了。  
     在此之前 Natasha 已经分离出 N 多项目，我很难一个人维护那么多。                                 

 <br/>  
 



 - #### 问：写进去的C#代码是自动编译成dll了是吗？
 
    - 答：
     - Natasha 只有一个编译器 AssemblyCSharpBuilder 类，它会存在每个操作类中，找到它，  
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
