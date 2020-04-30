### 整串编译

传入整个字符串进行编译， Natasha 的最小编译单元为程序集。
请使用 AssemblyBuilder .

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
oop.Syntax.Add(text);
Assembly assembly = oop.GetAssembly();


//或者使用二级API NAssembly
var asm = new NAssembly("MyAssembly");
asm.AddScript(text);
var type = asm.GetTypeFromShortName("Test");
var type = asm.GetTypeFromFullName("HelloWorld.Test");
```  

