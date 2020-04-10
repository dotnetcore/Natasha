<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/api/api-samples.html"> English </a>
</p>  

## API用法展示



- **NDomain**  

```C#


//NDomain 支持：

// 普通方法：      Func/Action
// 异步方法：      Async Func/Action
// 非安全方法：    Unsafe Func/Action
// 非安全异步方法： UnsafeAsync Func/Action



//------创建一个域（方便卸载）----//-----创建Func方法--------//
var func = NDomain.Create("NDomain2").Func<string,string>("return arg;");
Assert.Equal("1", func("1"));
//可卸载
NDomain.Delete("NDomain2");


NormalTestModel model = new NormalTestModel();
var func = NDomain.Create("NDomain6").Action<NormalTestModel, int, int>("arg1.Age=arg2+arg3;");
func(model,1,2);
Assert.Equal(3, model.Age);



案例2：
var action = NDomain.Default().UnsafeAsyncFunc<string, string, Task<string>>(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;");

string result = await action("Hello", "World1!");
//result = "Hello World1!"
  
```  


<br/>  



#### OopOperator : [参见UT测试](https://github.com/dotnetcore/Natasha/blob/master/test/NatashaUT/BuilderUT)  

<br/>  


#### OopComplier : [参见UT测试](https://github.com/dotnetcore/Natasha/blob/master/test/NatashaUT/OopComplierTest.cs)    


<br/>  

<br/>  

#### FastMethodOperator  

  <br/>  

- 普通定制  

> 快速定制一个方法
  
```C#
var action = FastMethodOperator.Default()
             .Param<string>("str1")
             .Param(typeof(string),"str2")
             .MethodBody("return str1+str2;")
             .Return<Task<string>>()
             .Complie<Func<string,string,string>>();
                    
var result = action("Hello ","World!");    //result:   "Hello World!"
```

<br/>  

- 增强实现与异步支持
> Complie<T>方法会检测参数以及返回类型，如果其中有任何一处没有指定，那么Complie方法会使用自己默认的参数或者返回值进行填充,
如果是Action<int> 这种带有1个参数的，请使用"arg", 另外如果想使用异步方法，请使用UseAsync方法,或者AsyncFrom<Class>(methodName)这两种方法。
返回的参数需要您指定Task<>,以便运行时异步调用，记得外面那层方法要有async关键字哦。

```C#
var delegateAction = FastMethodOperator.Random()

       .UseAsync()
       .MethodBody(@"
               await Task.Delay(100);
               string result = arg1 +"" ""+ arg2; 
               Console.WriteLine(result);
               return result;")
               
       .Complie<Func<string, string, Task<string>>>();
      
string result = await delegateAction?.Invoke("Hello", "World2!");   //result:   "Hello World2!"
```
<br/>
<br/>  

#### DelegateOperator  
  
> 快速快速实现委托
  
```C# 

//定义一个委托
public delegate string GetterDelegate(int value);
     
     
     
//方法一     
var action = DelegateOperator<GetterDelegate>.Delegate("value += 101; return value.ToString();");
string result = action(1);              
//result: "102"
 

```  

<br/>
<br/>  

#### FakeMethodOperator  

> 快速复制方法并实现

```C#
public class Test
{ 
   public string Handler(string str)
   { 
        retrurn null; 
   }
}

```
```C#
var action = FakeMethodOperator.Default()
             .UseMethod(typeof(Test).GetMethod("Handler"))
             .StaticMethodContent(" str += "" is xxx;"",return str; ")
             .Complie<Func<string,string>>();
                  
string result = action("xiao");              //result: "xiao is xxx;"          
```  

> [参见UT测试](https://github.com/dotnetcore/Natasha/blob/master/test/NatashaUT/DynamicMethodTest.cs#L96-L196)  

<br/>
<br/>  
