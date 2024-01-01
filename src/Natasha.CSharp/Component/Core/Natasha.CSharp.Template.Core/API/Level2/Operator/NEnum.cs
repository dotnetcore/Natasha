using Natasha.CSharp;
using Natasha.CSharp.Template;
using System;

public class NEnum : NHandler<NEnum>
{

    public NEnum()
    {

        Link = this;
        this.Enum();

    }



    private void AddSummary(string content)
    {
        if (content!="")
        {
            BodyAppendLine("/// <summary>");
            BodyAppendLine($"/// {content.Replace(Environment.NewLine, "")}");
            BodyAppendLine("/// </summary>");
        }
    }
    /// <summary>
    /// 设置枚举字段
    /// </summary>
    /// <param name="name">枚举字段名</param>
    /// <param name="value">枚举值</param>
    /// <param name="name">枚举字段注释</param>
    /// <returns></returns>
    public NEnum EnumField(string name, int value, string summary = "")
    {
        
        if (BodyScript.Length > 0)
        {
            BodyAppendLine(",");
        }
        AddSummary(summary);
        BodyAppend($"{name}={value}");
        return Link;

    }




    /// <summary>
    /// 设置枚举字段
    /// </summary>
    /// <param name="name">枚举字段名</param>
    /// <param name="name">枚举字段注释</param>
    /// <returns></returns>
    public NEnum EnumField(string name, string summary = "")
    {

        if (BodyScript.Length > 0)
        {
            BodyAppendLine(",");
        }
        AddSummary(summary);
        BodyAppend(name);
        return Link;

    }

}

