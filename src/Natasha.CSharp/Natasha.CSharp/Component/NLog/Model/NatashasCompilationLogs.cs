using System;
using System.Collections.Generic;
using System.Text;

public class NatashaCompilationLogs
{
    public NatashaCompilationLogs(string info)
    {
        CompilationsSerializableInfomation = info;
        Messages = new();
    }

    public bool HasError;

    public readonly string CompilationsSerializableInfomation;

    public readonly List<NatashaCompileErrorLog> Messages;


    public void AddMessage(int count,string code,string message)
    {
        Messages.Add(new(count, code, message));
    }

}

public class NatashaCompileErrorLog 
{
    private readonly int _count;
    private readonly string _code;
    private readonly string _message;
    public NatashaCompileErrorLog(int count, string code,string message)
    {
        _count = count;
        _code = code;
        _message = message;
    }
    public int ErrorCount { get { return _count; } }
    public string ErrorCode { get { return _code; } }
    public string ErrorMessage { get { return _message; } }

}


