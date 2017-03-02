using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha.Cache
{
    //线程缓存，保证EHandle可以并发生成函数
    public static class ThreadCache
    {
        public static Dictionary<int, ILGenerator> ILDict;
        public static Dictionary<int, string> TKeyDict;
        public static Dictionary<int, ILGenerator> TILDict;
        public static Dictionary<int, OpCode> CodeDict;
        public static Dictionary<int, OpCode> TCodeDict;
        public static Dictionary<int, Action> CompareAction1;
        public static Dictionary<int, Action> TCompareAction1;


        public static Dictionary<int, Action> CompareAction2;
        public static Dictionary<int, Action> TCompareAction2;

        static ThreadCache()
        {
            ILDict = new Dictionary<int, ILGenerator>();
            CodeDict = new Dictionary<int, OpCode>();
            TILDict = new Dictionary<int, ILGenerator>();
            TKeyDict = new Dictionary<int, string>();
            TCodeDict = new Dictionary<int, OpCode>();

            CompareAction1 = new Dictionary<int, Action>();
            TCompareAction1 = new Dictionary<int, Action>();

            CompareAction2 = new Dictionary<int, Action>();
            TCompareAction2 = new Dictionary<int, Action>();
        }
        public static void SetCompareAction1(Action action)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                TCompareAction1[ThreadId] = action;
            }
            else
            {
                CompareAction1[ThreadId] = action;
            }
        }
        public static void SetCompareAction2(Action action)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                TCompareAction2[ThreadId] = action;
            }
            else
            {
                CompareAction2[ThreadId] = action;
            }
        }
        public static Action GetCompareAction1()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                if (TCompareAction1.ContainsKey(ThreadId))
	            {
		            return TCompareAction1[ThreadId];
	            }
                else
	            {
                    return null;
	            }
            }
            else
            {
                if (CompareAction1.ContainsKey(ThreadId))
	            {
		            return CompareAction1[ThreadId];
	            }
                else
	            {
                    return null;
	            }
            }
        }
        public static Action GetCompareAction2()
        {

            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                if (TCompareAction2.ContainsKey(ThreadId))
                {
                    return TCompareAction2[ThreadId];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (CompareAction2.ContainsKey(ThreadId))
                {
                    return CompareAction2[ThreadId];
                }
                else
                {
                    return null;
                }
            }
        }
        public static void SetJudgeCode(OpCode code)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                TCodeDict[ThreadId] = code;
            }
            else
            {
                CodeDict[ThreadId] = code;
            }
        }
        public static OpCode GetJudgeCode()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;

            if (TKeyDict.ContainsKey(ThreadId))
            {
                return TCodeDict[ThreadId];
            }
            else
            {
                return CodeDict[ThreadId];
            }
        }
        public static ILGenerator GetIL()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (TKeyDict.ContainsKey(ThreadId))
            {
                return TILDict[ThreadId];
            }
            if (ILDict.ContainsKey(ThreadId))
            {
                return ILDict[ThreadId];
            }
            return null;
        }
    }
}
