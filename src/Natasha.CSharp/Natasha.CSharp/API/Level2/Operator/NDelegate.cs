using Natasha.CSharp;
using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using System;



public class NDelegate : CompilerTemplate<NDelegate>
{

    private Func<FakeMethodOperator, FakeMethodOperator>? _methodAction;
    private Func<OopBuilder, OopBuilder>? _oopAction;
    public NDelegate() {
        Usings = Array.Empty<NamespaceConverter>();
        Link = this; 
    }



    public NDelegate ConfigClass(Func<OopBuilder, OopBuilder> classAction)
    {

        _oopAction += classAction;
        return this;

    }
    public NDelegate ConfigMethod(Func<FakeMethodOperator, FakeMethodOperator> methodAction)
    {

        _methodAction += methodAction;
        return this;

    }

    public NamespaceConverter[] Usings;
    public NDelegate AddUsing(params NamespaceConverter[] usings)
    {
        Usings = usings;
        return this;
    }

    public T Delegate<T>(string content) where T : Delegate
    {
        return DelegateOperator<T>.Delegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public T AsyncDelegate<T>(string content) where T : Delegate
    {

        return DelegateOperator<T>.AsyncDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public T UnsafeDelegate<T>(string content) where T : Delegate
    {

        return DelegateOperator<T>.UnsafeDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public T UnsafeAsyncDelegate<T>(string content) where T : Delegate
    {

        return DelegateOperator<T>.UnsafeAsyncDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T> Func<T>(string content)
    {

        return DelegateOperator<Func<T>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T> AsyncFunc<T>(string content)
    {

        return DelegateOperator<Func<T>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T> UnsafeFunc<T>(string content)
    {

        return DelegateOperator<Func<T>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T> UnsafeAsyncFunc<T>(string content)
    {

        return DelegateOperator<Func<T>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2> Func<T1, T2>(string content)
    {

        return DelegateOperator<Func<T1, T2>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1,T2>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2> AsyncFunc<T1, T2>(string content)
    {

        return DelegateOperator<Func<T1, T2>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2> UnsafeFunc<T1, T2>(string content)
    {

        return DelegateOperator<Func<T1, T2>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2> UnsafeAsyncFunc<T1, T2>(string content)
    {

        return DelegateOperator<Func<T1, T2>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3> Func<T1, T2, T3>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2,T3>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3> AsyncFunc<T1, T2, T3>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3> UnsafeFunc<T1, T2, T3>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3> UnsafeAsyncFunc<T1, T2, T3>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4> Func<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4> AsyncFunc<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4> UnsafeFunc<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4> UnsafeAsyncFunc<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5> Func<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5> AsyncFunc<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5> UnsafeFunc<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5> UnsafeAsyncFunc<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6> Func<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6> AsyncFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6> UnsafeFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7> Func<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> AsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8> Func<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action Action(string content)
    {

        return DelegateOperator<Action>.Delegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action AsyncAction(string content)
    {

        return DelegateOperator<Action>.AsyncDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action UnsafeAction(string content)
    {

        return DelegateOperator<Action>.UnsafeDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action UnsafeAsyncAction(string content)
    {

        return DelegateOperator<Action>.UnsafeAsyncDelegate(content, AssemblyBuilder, null, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T> Action<T>(string content)
    {

        return DelegateOperator<Action<T>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T> AsyncAction<T>(string content)
    {

        return DelegateOperator<Action<T>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T> UnsafeAction<T>(string content)
    {

        return DelegateOperator<Action<T>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T> UnsafeAsyncAction<T>(string content)
    {

        return DelegateOperator<Action<T>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2> Action<T1, T2>(string content)
    {

        return DelegateOperator<Action<T1, T2>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2> AsyncAction<T1, T2>(string content)
    {

        return DelegateOperator<Action<T1, T2>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2> UnsafeAction<T1, T2>(string content)
    {

        return DelegateOperator<Action<T1, T2>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2> UnsafeAsyncAction<T1, T2>(string content)
    {

        return DelegateOperator<Action<T1, T2>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3> Action<T1, T2, T3>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3> AsyncAction<T1, T2, T3>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3> UnsafeAction<T1, T2, T3>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3> UnsafeAsyncAction<T1, T2, T3>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4> AsyncAction<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4> UnsafeAction<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4> UnsafeAsyncAction<T1, T2, T3, T4>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5> AsyncAction<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5> UnsafeAction<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5> UnsafeAsyncAction<T1, T2, T3, T4, T5>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6> AsyncAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6> UnsafeAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> AsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, AssemblyBuilder, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo, OptionAction, _methodAction, _oopAction, Usings);
    }
}

