### 异常捕获

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

//程序集单元编译器
AssemblyComplier.ComplieException/SyntaxExceptions

//语法检测异常
Operator.Complier.SyntaxExceptions[0]
//语法检测之后会搜集编译异常
Operator.Complier.ComplieException

```

#### 开启日志：
```C#
NSucceedLog.Enabled = true;
NErrorLog.Enabled = true;
NWarningLog.Enabled = true;

```  

#### 关闭日志：
```C#
NSucceedLog.Enabled = false;
NErrorLog.Enabled = false;
NWarningLog.Enabled = false
```
