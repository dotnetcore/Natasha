# API Details  

<br/>  

## API Quick look-up table  

<br/>  

| ClassName | Description | NameSpace | OperatorType |
|:---:|:---:|:---:|:---:|
| NAction | Implementing Action delegation | Natasha | static | 
| NFunc | Implementing Func delegation | Natasha | static |
| OopComplier | Compile the entire string | Natasha | instantiation |
| NewMethod | Quickly implement a delegate | Natasha | static |
| NewClass | Create a class| Natasha | static |
| NewStruct | Create a struct| Natasha | static |
| NewInterface | Create an interface | Natasha | static |
| FakeMethodOperator | Forge MethodInfo and create a method | Natasha.Operator | static/instantiation |
| FastMethodOperator | Quickly create a method | Natasha.Operator | static/instantiation |
| DelegateOperator | Implement a delegate | Natasha.Operator | static |
| ProxyOperator | Dynamic implementation of interfaces / abstract classes / virtual methods | Natasha.Operator | instantiation |
| OopOperator | Dynamically build classes / interfaces / structures | Natasha.Operator | instantiation |
| SnapshotOperator | Snapshot operation | Natasha.Operator | static |
| CloneOperator | Clone operation | Natasha.Operator | static |
| CtorOperator | Dynamic initialization | Natasha.Operator | static |


<br/>  

## API Usage display

<br/>  

- **NFunc/NAction**  

```C#

//NFunc and NAction：

// general method：      Delegate
// async method：      AsyncDelegate
// unsafe method：    UnsafeDelegate
// unsafe async method： UnsafeAsyncDelegate

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

- **Operator**  : [See Operator page](https://github.com/dotnetcore/Natasha/blob/master/article/Operator.md)  

<br/>  

