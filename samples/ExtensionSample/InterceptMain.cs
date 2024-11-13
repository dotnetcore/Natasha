using System.Runtime.CompilerServices;

namespace System;

public static class InterceptMain
{

    [InterceptsLocation(
        filePath: @"G:\Project\OpenSource\Natasha\samples\ExtensionSample\Program.cs",
        line: 24,
        character: 28)]
    public static void InterceptMethodMain()
    {

        Console.WriteLine(11);

    }

}