### 整串编译

传入整个字符串进行编译，Natasha的最小编译单元为程序集。
请使用AssemblyComplier

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
AssemblyComplier oop = new AssemblyComplier();
oop.Add(text);
Type type = oop.GetType("Test");


//或者使用三级API NAssembly

var asm = new NAssembly("MyAssembly");
asm.AddScript(text);

```  

四级运算 API

```C#

var handler = DomainOperator.Create("Tesst2Domain")
& @"public class  DomainTest1{
      public string Name;
      public DomainOperator Operator;
}" | "System" | typeof(DomainOperator);


var type = handler.GetType();
Assert.Equal("DomainTest1", type.Name);

```
使用 DomainOperator.Create 创建域， 使用 & 符合连接字符串代码， 使用 | 符号链接命名空间。
