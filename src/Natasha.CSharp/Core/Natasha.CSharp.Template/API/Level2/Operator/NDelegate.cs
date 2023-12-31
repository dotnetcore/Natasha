using Natasha.CSharp;
using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using System;
using System.Reflection;


/// <summary>
/// 委托的快速实现类
/// 如果使用 Func:  Func<T,R> 第一个参数名为:arg , Func<T,T,R> 起,为 arg1,arg2....
/// 如果使用 Action:  Action<T> 第一个参数名为:obj , Action<T,T> 起,为 arg1,arg2....
/// </summary>
public sealed class NDelegate : CompilerTemplate<NDelegate>
{

    public readonly FakeMethodOperator MethodHandler;

    public NDelegate()
    {
        MethodHandler = new();
        MethodHandler.AssemblyBuilder = this.AssemblyBuilder;
        Usings = Array.Empty<NamespaceConverter>();
        Link = this;
    }


    public T Delegate<T>(string content, MethodInfo? methodInfo = null) where T : Delegate
    {

        if (methodInfo == null)
        {
            methodInfo = typeof(T).GetMethod("Invoke")!;
        }
        MethodHandler
            .UseMethod(methodInfo)
            .StaticMethodBody(content);
        return MethodHandler.Compile<T>();

    }




    public T AsyncDelegate<T>(string content,MethodInfo? methodInfo = null) where T : Delegate
    {

        if (methodInfo == null)
        {
            methodInfo = typeof(T).GetMethod("Invoke")!;
        }
        MethodHandler
            .UseMethod(methodInfo)
            .Async()
            .StaticMethodBody(content);
        return MethodHandler.Compile<T>();

    }




    public T UnsafeDelegate<T>(string content,MethodInfo? methodInfo = null) where T : Delegate
    {
       
        if (methodInfo == null)
        {
            methodInfo = typeof(T).GetMethod("Invoke")!;
        }
        MethodHandler
            .UseMethod(methodInfo)
            .Unsafe()
            .StaticMethodBody(content);
        return MethodHandler.Compile<T>();

    }




    public T UnsafeAsyncDelegate<T>(string content, MethodInfo? methodInfo = null) where T : Delegate
    {
        
        if (methodInfo == null)
        {
            methodInfo = typeof(T).GetMethod("Invoke")!;
        }
        MethodHandler
            .UseMethod(methodInfo)
            .Unsafe()
            .Async()
            .StaticMethodBody(content);
        return MethodHandler.Compile<T>();

    }


    /// <summary>
    /// 配置委托模板的类模板
    /// </summary>
    /// <param name="classAction"></param>
    /// <returns></returns>
    public NDelegate ConfigClass(Func<OopBuilder, OopBuilder> classAction)
    {

        classAction(MethodHandler.OopHandler);
        return this;

    }
    /// <summary>
    /// 配置委托模板的方法模板
    /// </summary>
    /// <param name="methodAction"></param>
    /// <returns></returns>
    public NDelegate ConfigMethod(Func<FakeMethodOperator, FakeMethodOperator> methodAction)
    {

        methodAction(MethodHandler);
        return this;

    }

    public NamespaceConverter[] Usings;
    public NDelegate ConfigUsing(params NamespaceConverter[] usings)
    {
        MethodHandler.Using(usings);
        return this;
    }


    public Func<T> Func<T>(string content)
    {

        return Delegate<Func<T>>(content, DelegateImplementationHelper<T>.FuncInfo);
    }


    public Func<T> AsyncFunc<T>(string content)
    {

        return AsyncDelegate<Func<T>>(content, DelegateImplementationHelper<T>.FuncInfo);
    }


    public Func<T> UnsafeFunc<T>(string content)
    {

        return UnsafeDelegate<Func<T>>(content, DelegateImplementationHelper<T>.FuncInfo);
    }


    public Func<T> UnsafeAsyncFunc<T>(string content)
    {

        return UnsafeAsyncDelegate<Func<T>>(content, DelegateImplementationHelper<T>.FuncInfo);
    }


    /// <summary>
    /// 第一个参数名为: arg
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Func<T1, T2> Func<T1, T2>(string content)
    {

        return Delegate<Func<T1, T2>>(content, DelegateImplementationHelper<T1,T2>.FuncInfo);
    }



    /// <summary>
    /// 第一个参数名为: arg
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Func<T1, T2> AsyncFunc<T1, T2>(string content)
    {

        return AsyncDelegate<Func<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.FuncInfo);
    }



    /// <summary>
    /// 第一个参数名为: arg
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Func<T1, T2> UnsafeFunc<T1, T2>(string content)
    {

        return UnsafeDelegate<Func<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.FuncInfo);
    }



    /// <summary>
    /// 第一个参数名为: arg
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Func<T1, T2> UnsafeAsyncFunc<T1, T2>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.FuncInfo);
    }




    public Func<T1, T2, T3> Func<T1, T2, T3>(string content)
    {

        return Delegate<Func<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2,T3>.FuncInfo);
    }




    public Func<T1, T2, T3> AsyncFunc<T1, T2, T3>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.FuncInfo);
    }




    public Func<T1, T2, T3> UnsafeFunc<T1, T2, T3>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.FuncInfo);
    }




    public Func<T1, T2, T3> UnsafeAsyncFunc<T1, T2, T3>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.FuncInfo);
    }



    public Func<T1, T2, T3, T4> Func<T1, T2, T3, T4>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo);
    }




    public Func<T1, T2, T3, T4> AsyncFunc<T1, T2, T3, T4>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo);
    }




    public Func<T1, T2, T3, T4> UnsafeFunc<T1, T2, T3, T4>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo);
    }




    public Func<T1, T2, T3, T4> UnsafeAsyncFunc<T1, T2, T3, T4>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5> Func<T1, T2, T3, T4, T5>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5> AsyncFunc<T1, T2, T3, T4, T5>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5> UnsafeFunc<T1, T2, T3, T4, T5>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5> UnsafeAsyncFunc<T1, T2, T3, T4, T5>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6> Func<T1, T2, T3, T4, T5, T6>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6> AsyncFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6> UnsafeFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7> Func<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> AsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8> Func<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.FuncInfo);
    }



    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return Delegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return AsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return UnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo);
    }




    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return UnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.FuncInfo);
    }




    public Action Action(string content)
    {

        return Delegate<Action>(content, null);
    }



    public Action AsyncAction(string content)
    {

        return AsyncDelegate<Action>(content, null);
    }




    public Action UnsafeAction(string content)
    {

        return UnsafeDelegate<Action>(content, null);
    }




    public Action UnsafeAsyncAction(string content)
    {

        return UnsafeAsyncDelegate<Action>(content, null);
    }



    /// <summary>
    /// 参数名为 obj
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Action<T> Action<T>(string content)
    {

        return Delegate<Action<T>>(content, DelegateImplementationHelper<T>.ActionInfo);
    }



    /// <summary>
    /// 参数名为 obj
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Action<T> AsyncAction<T>(string content)
    {

        return AsyncDelegate<Action<T>>(content, DelegateImplementationHelper<T>.ActionInfo);
    }



    /// <summary>
    /// 参数名为 obj
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Action<T> UnsafeAction<T>(string content)
    {

        return UnsafeDelegate<Action<T>>(content, DelegateImplementationHelper<T>.ActionInfo);
    }



    /// <summary>
    /// 参数名为 obj
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Action<T> UnsafeAsyncAction<T>(string content)
    {

        return UnsafeAsyncDelegate<Action<T>>(content, DelegateImplementationHelper<T>.ActionInfo);
    }


    public Action<T1, T2> Action<T1, T2>(string content)
    {

        return Delegate<Action<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.ActionInfo);
    }




    public Action<T1, T2> AsyncAction<T1, T2>(string content)
    {

        return AsyncDelegate<Action<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.ActionInfo);
    }




    public Action<T1, T2> UnsafeAction<T1, T2>(string content)
    {

        return UnsafeDelegate<Action<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.ActionInfo);
    }




    public Action<T1, T2> UnsafeAsyncAction<T1, T2>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2>>(content, DelegateImplementationHelper<T1, T2>.ActionInfo);
    }




    public Action<T1, T2, T3> Action<T1, T2, T3>(string content)
    {

        return Delegate<Action<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.ActionInfo);
    }




    public Action<T1, T2, T3> AsyncAction<T1, T2, T3>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.ActionInfo);
    }




    public Action<T1, T2, T3> UnsafeAction<T1, T2, T3>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.ActionInfo);
    }




    public Action<T1, T2, T3> UnsafeAsyncAction<T1, T2, T3>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3>>(content, DelegateImplementationHelper<T1, T2, T3>.ActionInfo);
    }



    public Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo);
    }




    public Action<T1, T2, T3, T4> AsyncAction<T1, T2, T3, T4>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo);
    }




    public Action<T1, T2, T3, T4> UnsafeAction<T1, T2, T3, T4>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo);
    }




    public Action<T1, T2, T3, T4> UnsafeAsyncAction<T1, T2, T3, T4>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4>>(content, DelegateImplementationHelper<T1, T2, T3, T4>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5> AsyncAction<T1, T2, T3, T4, T5>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5> UnsafeAction<T1, T2, T3, T4, T5>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5> UnsafeAsyncAction<T1, T2, T3, T4, T5>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6> AsyncAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6> UnsafeAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> AsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ActionInfo);
    }



    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return Delegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return AsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return UnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo);
    }




    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content)
    {

        return UnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(content, DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ActionInfo);
    }
}

