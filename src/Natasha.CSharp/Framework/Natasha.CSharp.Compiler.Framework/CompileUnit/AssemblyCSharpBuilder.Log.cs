using System;

public sealed partial class AssemblyCSharpBuilder
{

    public event Action<NatashaCompilationLog>? LogCompilationEvent;
    public AssemblyCSharpBuilder SetLogEvent(Action<NatashaCompilationLog> logAction)
    {
        LogCompilationEvent = logAction;
        return this;
    }

}

