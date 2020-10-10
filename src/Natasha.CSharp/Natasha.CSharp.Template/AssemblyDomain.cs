using System;


public class AssemblyDomain
{
    public static void Init()
    {
        throw new Exception(@"该方法已过时，从4.0起，组件需要分别注册，详见 https://natasha.dotnetcore.xyz/zh/helloworld.html");
    }
}

