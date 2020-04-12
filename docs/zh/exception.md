## 异常捕获

#### 异常结构：
```C#
public class CompilationException
 {

        public CompilationException()
        {

            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ComplieError.None;

        }


        //编译日志
        public string Log;

        //错误信息
        public string Message;

        //格式化后的脚本字符串
        public string Formatter;

        //错误类型
        public ComplieError ErrorFlag;

        //roslyn诊断集合
        public List<Diagnostic> Diagnostics;
       
}
```


```C# 
//整个编译流程中涉及到异常的过程包括：
//添加语法树会返回异常
//AssemblyBuilde builder;;
var exception = builder.Syntax.Add();


//编译过后 Exceptions 字段中会存有异常
//AssemblyBuilde builder;
var exception = builder.Exceptions;


//可以通过设置异常行为来控制异常的发生动作
builder.Syntax.ErrorBehavior = ExceptionBehavior.Log | ExceptionBehavior.Throw;
builder.Compiler.ErrorBehavior = ExceptionBehavior.Throw;
```

例如：
```C#
NClass @class = NClass.Random();
@class.Syntax.ErrorBehavior = xxx;
```
