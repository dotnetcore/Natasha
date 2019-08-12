# API详情  

<br/>  

## API速查表  

<br/>  

| 类名 | 作用 | 命名空间 | 操作类型 |
|:---:|:---:|:---:|:---:|
| NAction | 快速创建动态的Action委托 | Natasha | 静态 | 
| NFunc | 快速创建动态的Func委托 | Natasha | 静态 |
| OopComplier | 编译整个字符串 | Natasha | 实例化 |
| NewMethod | 创建委托 | Natasha | 静态 |
| NewClass | 创建类| Natasha | 静态 |
| NewStruct | 创建结构体| Natasha | 静态 |
| NewInterface | 创建接口 | Natasha | 静态 |
| FakeMethodOperator | 仿造MethodInfo创建方法 | Natasha.Operator | 静态/实例化 |
| FastMethodOperator | 快速创建方法 | Natasha.Operator | 静态/实例化 |
| DelegateOperator | 快速实现委托 | Natasha.Operator | 静态 |
| ProxyOperator | 动态实现接口/抽象类/虚方法 | Natasha.Operator | 实例化 |
| OopOperator | 动态构建类/接口/结构体 | Natasha.Operator | 实例化 |
| SnapshotOperator | 快照操作 | Natasha.Operator | 静态 |
| CloneOperator | 克隆操作 | Natasha.Operator | 静态 |
| CtorOperator | 动态初始化 | Natasha.Operator | 静态 |


<br/>  

## API用法展示

<br/>  

- **NFunc/NAction**  

```C#

//NFunc 和 NAction 支持：

// 普通方法：      Delegate
// 异步方法：      AsyncDelegate
// 非安全方法：    UnsafeDelegate
// 非安全异步方法： UnsafeAsyncDelegate

var action = NFunc<string, string, Task<string>>.UnsafeAsyncDelegate(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;");

string result = await action("Hello", "World1!");
//result = "Hello World1!"
  
```  


<br/>  

- **NewClass/NewInterface/NewStruct**  

```C#

  var result = NewStruct.Create(builder => builder
  
                .Namespace("TestNamespace")
                .OopAccess(AccessTypes.Private)
                .OopName("TestUt2")
                
                .Ctor(item=>item
                    .MemberAccess("public")
                    .Param<string>("name")
                    .Body("this.Name=name;"))
                    
                .OopBody(@"public static void Test(){}")
                
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                
  );
  
  
  Type type = result.Type;
  var error = result.Exception; 
  
```  

<br/>  

- **NewMethod**  

```C#

 var result = NewMethod.Create<Func<string, string, Task<string>>>(builder => builder
 
                    .UseAsync()
                    .MethodBody(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;")
                    );

  var method = result.Method;
  var error = result.Exception; 
  

```  

<br/>  

- **Operator**  : [参见Operator页](https://github.com/dotnetcore/Natasha/blob/master/article/Operator.md)  

<br/>  

