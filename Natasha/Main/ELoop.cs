using System;
using System.Reflection.Emit;
using Natasha.Core.Base;
using Natasha.Core;
using Natasha.Cache;
using Natasha.Debug;

namespace Natasha
{
    //循环啊循环
    public class ELoop:ILGeneratorBase
    {
        private Func<Action, ELoop> BodyFunc;

        private Label StartLabel;
        private Label EndLabel;
        private ELoop():base()
        {
            BodyFunc = (action) =>
            {
                if (action != null)
                {
                    action();
                }
                ilHandler.Emit(OpCodes.Br_S, StartLabel);               //无条件跳转到起始标签
                ilHandler.MarkLabel(EndLabel);                          //末尾标签
                DebugHelper.WriteLine("JumpTo Start");
                DebugHelper.WriteLine("SetLabel END");
                return this;
            };
        }

        public LocalBuilder CurrentFlag;

        
        /// <summary>
        /// 提供while操作
        /// </summary>
        /// <param name="condition">判断语句</param>
        /// <returns></returns>
        public static Func<Action, ELoop> While(Action condition)
        {
            ELoop loopHandler = new ELoop();
            loopHandler.StartLabel = loopHandler.ilHandler.DefineLabel();
            loopHandler.EndLabel = loopHandler.ilHandler.DefineLabel();
            loopHandler.ilHandler.MarkLabel(loopHandler.StartLabel);                        //设置开始标签
            condition();                                                                    //条件判断
            loopHandler.ilHandler.Emit(ThreadCache.GetJudgeCode(), loopHandler.EndLabel);   //不成立就跳转到末尾
            DebugHelper.WriteLine(ThreadCache.GetJudgeCode().Name + " END");
            return loopHandler.BodyFunc;
        }
        //对实现了迭代接口的操作类进行遍历
        public static ELoop For(IIterator instance, Action<Action> action)
        {
            return For(0,instance.Length,1,instance,action);
        }
        //对实现了迭代接口的操作类进行遍历
        public static ELoop For(int start, int end, int increment,IIterator instance, Action<Action> action = null)
        {
            ELoop loopHandler = new ELoop();
            ILGenerator CurrentIL = loopHandler.ilHandler;
            Label lb_while = CurrentIL.DefineLabel();
            loopHandler.CurrentFlag = CurrentIL.DeclareLocal(typeof(int));
                                                                    
            start -= 1;                                             //为了减少标签使用，全部减少1
            end -= 1;
            CurrentIL.Emit(OpCodes.Ldc_I4, start);                  //此处可优化 255一下用Ldc_I4_S 短格式
            CurrentIL.Emit(OpCodes.Stloc, loopHandler.CurrentFlag); //起始值入临时变量
            CurrentIL.MarkLabel(lb_while);                          //设置循环标签
            CurrentIL.Emit(OpCodes.Ldloc, loopHandler.CurrentFlag); //其实值加increment
            CurrentIL.Emit(OpCodes.Ldc_I4, increment);
            CurrentIL.Emit(OpCodes.Add);
            CurrentIL.Emit(OpCodes.Stloc, loopHandler.CurrentFlag); //填充到临时变量
                                                                    //生成委托，委托参数为当前起始值
                                                                    
            Action loadCurrentElemenet = () => {                    //loadCurrentElemenet用来加载当前元素
                instance.LoadCurrentElement(loopHandler.CurrentFlag);//迭代接口的实现
            };
            if (action != null)
            {
                action(loadCurrentElemenet);                        //主要逻辑
            }
            CurrentIL.Emit(OpCodes.Ldloc, loopHandler.CurrentFlag); //比较并循环
            CurrentIL.Emit(OpCodes.Ldc_I4, end);
            CurrentIL.Emit(OpCodes.Blt_S, lb_while);
            return loopHandler;
        }
        //对实现了迭代接口的操作类进行遍历
        public static ELoop Foreach(IIterator instance, Action<Action> action)
        {
            //获取迭代需要的属性等
            instance.Initialize();

            ELoop loopHandler = new ELoop();
            ILGenerator CurrentIL = loopHandler.ilHandler;

            Label lb_while_end = CurrentIL.DefineLabel();
            Label lb_while_start = CurrentIL.DefineLabel();

            CurrentIL.MarkLabel(lb_while_start);                     //设置标签位置-循环开始
            CurrentIL.Emit(OpCodes.Ldloca, instance.TempEnumerator); //加载由结构体构成的迭代对象
            CurrentIL.Emit(OpCodes.Call, instance.MoveNext);         //调用movenext方法
            CurrentIL.Emit(OpCodes.Brfalse_S, lb_while_end);         //如果为false跳转到结束标签

            Action loadCurrentElemenet = () => {                     //loadCurrentElemenet用来加载当前元素
                instance.LoadCurrentElement(null);                   //执行实现了迭代接口的方法，通常这个方法是为了加载当前元素
            }; 
            if (action != null)
            {
                action(loadCurrentElemenet);                         //执行主要逻辑
            }

            CurrentIL.Emit(OpCodes.Br_S, lb_while_start);            //无条件跳转开始标签
            CurrentIL.MarkLabel(lb_while_end);                       //设置标签位置-循环结束

            CurrentIL.Emit(OpCodes.Ldloca, instance.TempEnumerator); //调用dispose方法
            CurrentIL.Emit(OpCodes.Call, instance.Dispose);
            return loopHandler;
        }


    }
}
