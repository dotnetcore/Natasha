# Natasha
去除IL操作，像正常人一样创造你的动态代码。

重启项目，使用roslyn方案。

欢迎参与讨论：[点击加入Gitter讨论组](https://gitter.im/dotnetcore/Natasha)

目前源码版本 0.6.5.0 

Nuget版本 0.6.5.0 ，案例可依照UT测试。  



- ### 项目架构
   <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/Natasha架构图.png" height="500" width="900" alt="Natasha架构"/>

<br/>
<br/>  

- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, origin）**  
     - [ ]  **动态调用性能测试（对照组： 动态直接调用，动态代理调用，emit, origin）**  
     - [ ]  **动态克隆性能测试（对照组： origin）**
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, origin）**

- **优化计划**：

     - [x]  **动态编译模块**  
        - [x]  评估“流加载模式”以及“文件加载”模式的资源占用情况  
        
            内存流： <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/memory.png" height="300" width="250" alt="程序集内存流编译"/>
            文件流： <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/file.png" height="300" width="250" alt="程序集文件编译"/>

        
            
      
     
