using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using System;
using System.Linq;

namespace Natasha.CSharp
{

    public class NDelegate : CompilerTemplate<NDelegate>
    {

        private Func<FakeMethodOperator, FakeMethodOperator> _methodAction;
        private Func<OopBuilder, OopBuilder> _oopAction;
        public NDelegate() { Link = this; }




        public Type GetType(string code, string typeName = default)
        {

            OopOperator oopOperator = OopOperator.UseCompiler(AssemblyBuilder, OptionAction);
            string result = oopOperator
                .GetUsingBuilder()
                .Append(code).ToString();

            oopOperator.AssemblyBuilder.Add(result, oopOperator.Usings);

            var text = result;
            if (typeName == default)
            {
                typeName = ScriptHelper.GetClassName(text);
                if (typeName == default)
                {
                    typeName = ScriptHelper.GetInterfaceName(text);
                    if (typeName == default)
                    {
                        typeName = ScriptHelper.GetStructName(text);
                        if (typeName == default)
                        {
                            typeName = ScriptHelper.GetEnumName(text);
                        }
                    }
                }
            }

            return oopOperator.AssemblyBuilder.GetTypeFromShortName(typeName);
        }


        public NDelegate SetClass(Func<OopBuilder, OopBuilder> classAction)
        {

            _oopAction += classAction;
            return this;

        }
        public NDelegate SetMethod(Func<FakeMethodOperator, FakeMethodOperator> methodAction)
        {

            _methodAction += methodAction;
            return this;

        }




        public T Delegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
           
            return content == default ? null : DelegateOperator<T>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public T AsyncDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
           
            return content == default ? null : DelegateOperator<T>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public T UnsafeDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
           
            return content == default ? null : DelegateOperator<T>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public T UnsafeAsyncDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
           
            return content == default ? null : DelegateOperator<T>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T> Func<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T> AsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T> UnsafeFunc<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T> UnsafeAsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2> Func<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2> AsyncFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2> UnsafeFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2> UnsafeAsyncFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3> Func<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3> AsyncFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3> UnsafeFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3> UnsafeAsyncFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4> Func<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4> AsyncFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4> UnsafeFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4> UnsafeAsyncFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5> Func<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> AsyncFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> UnsafeFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> UnsafeAsyncFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6> Func<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> AsyncFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> UnsafeFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7> Func<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> AsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8> Func<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action Action(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action AsyncAction(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action UnsafeAction(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action UnsafeAsyncAction(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T> Action<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T> AsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T> UnsafeAction<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T> UnsafeAsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2> Action<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2> AsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2> UnsafeAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2> UnsafeAsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3> Action<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3> AsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3> UnsafeAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3> UnsafeAsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4> AsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4> UnsafeAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4> UnsafeAsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> AsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> AsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> AsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
           
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, AssemblyBuilder.Compiler.Domain, OptionAction, _methodAction, _oopAction, usings.ToArray());
        }
    }
}
