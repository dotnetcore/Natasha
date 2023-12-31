using System;
using System.Collections.Generic;
using System.Text;

public sealed class NatashaCompilationLog
{
    public NatashaCompilationLog()
    {
        _csInfo = string.Empty;
        Messages = new();
        CompilationInfomations = new();
    }

    public bool HasError;

    private string _csInfo;

    public string CompilationsSerializableInfomation
    {
        get
        {
            if (_csInfo == string.Empty)
            {
                _csInfo = $"AssemblyName:{CompilationInfomations["AssemblyName"]};Time:{DateTime.Now:yyyy-MM-dd HH:mm:ss};Language:{CompilationInfomations["Language"]};LanguageVersion:{CompilationInfomations["LanguageVersion"]};ReferencesCount:{CompilationInfomations["ReferencesCount"]}";
            }
            return _csInfo;
        } 
    }

    public readonly List<NatashaCompilationMessage> Messages;

    public readonly Dictionary<string, string> CompilationInfomations;

    internal void AddCompilationInfo(string key, string value)
    {
        CompilationInfomations[key] = value;
    }

    internal void AddMessage(int count,string code,string message)
    {
        Messages.Add(new(count, code, message));
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"{Environment.NewLine}============================== {(HasError?"ERROR":"SUCCEED")} : {CompilationInfomations["AssemblyName"]} ==============================");
        if (HasError)
        {
            foreach (var item in Messages)
            {
                result.AppendLine($"{Environment.NewLine}------------------------------------------------------------------------------------------------------{Environment.NewLine}");
                result.AppendLine(item.Code);
                result.AppendLine($"{Environment.NewLine}{item.Message}");
            }
            
        }
        else
        {
            foreach (var item in Messages)
            {
                result.AppendLine($"{Environment.NewLine}------------------------------------------------- {item.Message} -------------------------------------------{Environment.NewLine}");
                result.AppendLine(item.Code);
            }
        }

        result.AppendLine($"{Environment.NewLine}------------------------------------------------------------------------------------------------------");
        result.AppendLine($"{Environment.NewLine}    Time     :\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        result.AppendLine($"{Environment.NewLine}    Language :\t{CompilationInfomations["Language"]} & {CompilationInfomations["LanguageVersion"]}");
        result.AppendLine($"{Environment.NewLine}    TreeCount:\t共 {CompilationInfomations["SyntaxTreeCount"]} 个");
        result.AppendLine($"{Environment.NewLine}    RefCount :\t共 {CompilationInfomations["ReferencesCount"]} 个");
        result.AppendLine($"{Environment.NewLine}------------------------------------------------------------------------------------------------------");
        result.AppendLine($"{Environment.NewLine}======================================================================================================");
        return result.ToString();
    }


}

public class NatashaCompilationMessage 
{
    private readonly int _count;
    private readonly string _code;
    private readonly string _message;
    public NatashaCompilationMessage(int count, string code,string message)
    {
        _count = count;
        _code = code;
        _message = message;
    }
    public int ErrorCount { get { return _count; } }
    public string Code { get { return _code; } }
    public string Message { get { return _message; } }

}


