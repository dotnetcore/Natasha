using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha.Cache
{
    //线程缓存，保证EHandle可以并发生成函数
    public static class ThreadCache
    {
        public static ConcurrentDictionary<int, ILGenerator> ILDict;
        public static ConcurrentDictionary<int, string> TKeyDict;
        public static ConcurrentDictionary<int, ILGenerator> TILDict;
        public static ConcurrentDictionary<int, OpCode> CodeDict;
        public static ConcurrentDictionary<int, OpCode> TCodeDict;
        public static ConcurrentDictionary<int, Action> CompareAction1;
        public static ConcurrentDictionary<int, Action> TCompareAction1;


        public static ConcurrentDictionary<int, Action> CompareAction2;
        public static ConcurrentDictionary<int, Action> TCompareAction2;

        static ThreadCache()
        {
            ILDict = new ConcurrentDictionary<int, ILGenerator>();
            CodeDict = new ConcurrentDictionary<int, OpCode>();
            TILDict = new ConcurrentDictionary<int, ILGenerator>();
            TKeyDict = new ConcurrentDictionary<int, string>();
            TCodeDict = new ConcurrentDictionary<int, OpCode>();

            CompareAction1 = new ConcurrentDictionary<int, Action>();
            TCompareAction1 = new ConcurrentDictionary<int, Action>();

            CompareAction2 = new ConcurrentDictionary<int, Action>();
            TCompareAction2 = new ConcurrentDictionary<int, Action>();
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

        public static void Initialize()
        {

        }
    }
}
