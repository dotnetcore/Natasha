# Natasha

## CI Build Status



[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![GitHub license](https://img.shields.io/github/license/dotnetcore/Natasha.svg)](https://github.com/dotnetcore/EasyCaching/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fdotnetcore%2FEasyCaching.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fdotnetcore%2FNatasha?ref=badge_shield)  

| Platform | Build Server | Master Status  | Nuget Version |
|--------- |------------- |---------|---------|
| Travis   | Linux | [![Build Status](https://travis-ci.com/dotnetcore/Natasha.svg?branch=master)](https://travis-ci.org/dotnetcore/Natasha) |   [![NuGet Badge](https://buildstats.info/nuget/Natasha?includePreReleases=true)](https://www.nuget.org/packages/Natasha)|

[![Build history](https://buildstats.info/travisci/chart/dotnetcore/Natasha)](https://travis-ci.com/dotnetcore/Natasha/builds)    


重启项目，使用roslyn方案，去除IL操作，像正常人一样创造你的动态代码。

欢迎参与讨论：[点击加入Gitter讨论组](https://gitter.im/dotnetcore/Natasha)

目前源码版本 0.6.7.0 ，案例可依照UT测试。 

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


### 使用 FakeMethodOperator 快速构建函数：  

```C#
public class Test{ 

   public string Handler(string str){ 
        retrurn null; 
   }
   
}

```
```C#
var action = FakeMethodOperator.New
             .UseMethod(typeof(Test).GetMethod("Handler"))
             .StaticMethodContent(" str += "" is xxx;"",return str; ")
             .Complie<Func<string,string>>();
                  
string result = action("xiao");              //result: "xiao is xxx;"          
```
  
  
### 使用Natasha的类扩展  

```C#

Example:  
        Type : Dictionary<string,List<int>>[] 
        
        typeof(Dictionary<string,List<int>>).GetDevelopName();     //result:  "Dictionary<String,List<Int32>>[]"
        typeof(Dictionary<string,List<int>>).GetAvailableName();   //result:  "Dictionary_String_List_Int32____"
        typeof(Dictionary<string,List<int>>).GetAllGenericTypes(); //result:  [string,list<>,int]
        typeof(Dictionary<string,List<int>>).IsImplementFrom<IDictionary>(); //result: true

```

### 动态调用普通类  

```C#
public class A{
   public int Age;
   public DateTime Time;
   public B Outter = new B();
}
public class B{
   public string Name;
   public B(){
      Name = "小明"
   }
}
//如果是运行时动态生成类，也同样
```

```C#
var handler = DynamicOperator.GetOperator(typeof(A));

handler["Age"].IntValue = 100;                                    // Set Operator

Console.WriteLine(handler["Time"].DateTime);                      // Get Operator

handler["Outter"].OperatorValue["Name"].StringValue = "NewName"   // Link Operator
```


### 动态调用静态类
```C#
public static class A{
   public static int Age;
   public static DateTime Time;
   public static B Outter = new B();
}
public class B{
   public string Name;
   public B(){
      Name = "小明"
   }
}
//如果是运行时动态生成类，也同样
```

```C#
DynamicStaticOperator handler = typeof(A);

handler["Age"].IntValue = 100;                                    // Set Operator

Console.WriteLine(handler["Time"].DateTime);                      // Get Operator

handler["Outter"].OperatorValue["Name"].StringValue = "NewName"   // Link Operator
```
<br/>
<br/>  

- **测试计划（等待下一版本bechmark）**：
      
     - [ ]  **动态函数性能测试（对照组： emit, origin）**  
     - [ ]  **动态调用性能测试（对照组： 动态直接调用，动态代理调用，emit, origin）**  
     - [ ]  **动态克隆性能测试（对照组： origin）**
     - [ ]  **远程动态封装函数性能测试（对照组： 动态函数，emit, origin）**


        
            
      
     
