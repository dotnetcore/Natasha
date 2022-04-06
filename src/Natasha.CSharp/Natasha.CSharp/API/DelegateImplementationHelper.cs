using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{

    public static class DelegateImplementationHelper<T>
    {
        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T> action = (obj) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T> func = () => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2> action = (arg1, arg2) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2> func = (arg) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3> action = (arg1, arg2, arg3) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3> func = (arg1, arg2) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

        public static MethodInfo GetAction<S,S1,S2>()
        {
            Action<S,S1,S2 > action = (arg1, arg2, arg3) => {  };
            return action.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4> action = (arg1, arg2, arg3, arg4) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4> func = (arg1, arg2, arg3) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5> action = (arg1, arg2, arg3, arg4, arg5) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5> func = (arg1, arg2, arg3, arg4) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6> action = (arg1, arg2, arg3, arg4, arg5, arg6) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6> func = (arg1, arg2, arg3, arg4, arg5) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7> func = (arg1, arg2, arg3, arg4, arg5, arg6) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }
    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }
    public static class DelegateImplementationHelper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {

        public static readonly MethodInfo ActionInfo;
        public static readonly MethodInfo FuncInfo;
        static DelegateImplementationHelper()
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) => { throw new NotImplementedException(""); };
            ActionInfo = action.Method;
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> func = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) => { throw new NotImplementedException(""); };
            FuncInfo = func.Method;
        }

    }

}
