### 整串编译

传入整个字符串进行编译， Natasha 的最小编译单元为程序集。
请使用 AssemblyCSharpBuilder .

```C#
string text = @"
  namespace HelloWorld
  {public class Test{public Test(){
            Name=""111"";
        }public string Name;
        public int Age{get;set;}
    }
  }";


//根据脚本创建动态类
AssemblyCSharpBuilder oop = new AssemblyCSharpBuilder();

//这里就算你添加100个类，最终编译的时候都会在一个程序集中
oop.Add(text);

//下面的程序集里会有你在 Syntax 中添加的类
Assembly assembly = oop.GetAssembly();


//或者使用二级API NAssembly
//该操作类有 CreateClass / CreateInterface 等 API 函数，但最终的构建编译都会在同一个 AssemblyCSharpBuilder 中
var asm = new NAssembly("MyAssembly");
asm.AddScript(text);
var type = asm.GetTypeFromShortName("Test");
var type = asm.GetTypeFromFullName("HelloWorld.Test");
```  

