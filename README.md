# Natasha
去除IL操作，像正常人一样创造你的动态代码。

重启项目，使用roslyn方案。

欢迎参与讨论：[点击加入Gitter讨论组](https://gitter.im/dotnetcore/Natasha)

目前源码版本 0.6.6.0 

Nuget版本 0.6.5.0 ，案例可依照UT测试。  


### 使用 FastMethodOperator 快速构建函数：
```C#
     var action = FastMethodOperator.New
                    .Param<string>("str1")
                    .Param(typeof(string),"str2")
                    .MethodBody("return str1+str2;")
                    .Return<string>()
                    .Complie<Func<string,string,string>>();
                    
     string result = action("Hello ","World!");    //result:   "Hello World!"
```

### 使用 DelegateOperator 快速实现委托：
```C# 
     //定义一个委托
     public delegate string GetterDelegate(int value);
     
     
     var action = DelegateOperator<GetterDelegate>.Create("value += 101; return value.ToString();");
     
     string result = action(1);              //result: "102"
     
```
<br/>
<br/>  

- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, origin）**  
     - [ ]  **动态调用性能测试（对照组： 动态直接调用，动态代理调用，emit, origin）**  
     - [ ]  **动态克隆性能测试（对照组： origin）**
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, origin）**


        
            
      
     
