# Natasha
去除IL操作，像正常人一样创造你的动态代码。

重启项目，使用roslyn方案。


- ### 项目计划

- **2019/05 功能计划**：  

   -------
   - [x]  **脚本引擎&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]**

     - [x]  动态编译&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
     - [x]  动态构造&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
          - [x] 扩展模板&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
            - [x] Method模板&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;[##########][100%]  
          - [x] 反解器&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
            - [x] 类型反解&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
            - [x] 参数反解&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
            - [x] 字段反解&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
          - [x] 构造器&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]
   -------
   - [x]  **动态调用&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]**  
   
      - [x] 本地调用(字段+属性)&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;&nbsp;&nbsp;&nbsp;[##########][100%]
      - [x] 远程调用(Json序列化)&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;[##########][100%]
    -------
    - [x]  **动态实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[##########][100%]**  
    
      - [x] 接口动态实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;[##########][100%]
      - [x] 方法动态实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;[##########][100%]
      - [x] 类型动态实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;[##########][100%]
      - [x] 动态初始化实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;[##########][100%]
      - [x] 动态深度克隆实现(迭代接口排除)&emsp;&ensp;[##########][100%]
   -------
   
 
- **2019/06 功能计划**：  

   -------
   - [ ]  **动态实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[----------][0%]**
      - [ ] 类型映射实现&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp;&ensp;[----------][0%]
   -------
<br/>
<br/>  

- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, orgin, delegate）**  
     - [ ]  **动态调用性能测试（对照组： emit, orgin）**  
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, orgin）**

- **优化计划**：

     - [x]  **动态编译模块**  
        - [x]  评估“流加载模式”以及“文件加载”模式的资源占用情况  
        
            内存流： <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/memory.png" height="300" width="250" alt="程序集内存流编译"/>
            文件流： <img src="https://github.com/dotnetcore/Natasha/blob/master/Image/file.png" height="300" width="250" alt="程序集文件编译"/>
        - [x]  优化引擎，区分编译方式，增加内存流编译加载。
     - [x]  **动态构造模块**  
        - [x]  抽象出最基方法
        - [x]  采用部分类规整目录结构
     - [x]  **分离类库**  
      
      
     
