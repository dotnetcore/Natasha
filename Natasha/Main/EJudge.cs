using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;

using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha
{
    //IF ELSE
    public class EJudge : ILGeneratorBase
    {
        private Func<Action, EJudge> TrueFunc;
        private Func<Action, EJudge> OneFunc;
        private Label CurrentLabel;
        private Label EndLabel;
        private int LabelIndex;
        static EJudge() { }
        private void Jump(Label label)
        {
            il.REmit(OpCodes.Br_S, label);
            
        }
        private EJudge() : base()
        {
            TrueFunc = (action) =>
            {
                if (action != null)
                {
                    action();
                }
                Jump(EndLabel);
                il.MarkLabel(CurrentLabel);
                DebugHelper.WriteLine("Normal-MarkLabel");
                return this;
            };
            OneFunc = (action) =>
            {
                if (action != null)
                {
                    action();
                }
                il.MarkLabel(EndLabel);
                DebugHelper.WriteLine("End-MarkLabel");
                return this;
            };
        }

        //正确则跳转
        public static void TrueJump(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Brtrue_S, label);
        }
        //错误则跳转
        public static void FalseJump(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Brfalse_S, label);
        }
        //直接跳转
        public static void JumpTo(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Br_S, label);
        }
        //其他跳转方式 已经由重载运算符完成
        public static Func<Action, EJudge> If(object temp)
        {
            EJudge newEJudge = new EJudge();
            newEJudge.il.NoErrorLoad(temp);
            newEJudge.EndLabel = newEJudge.il.DefineLabel();
            newEJudge.CurrentLabel = newEJudge.il.DefineLabel();
            newEJudge.LabelIndex += 1;
            newEJudge.il.REmit(ThreadCache.GetJudgeCode(), newEJudge.CurrentLabel);
            return newEJudge.TrueFunc;
        }
        public static Func<Action, EJudge> If(Action action)
        {
            EJudge newEJudge = new EJudge();
            if (action!=null)
            {
                action();
            }
            newEJudge.EndLabel = newEJudge.il.DefineLabel();
            newEJudge.CurrentLabel = newEJudge.il.DefineLabel();
            newEJudge.LabelIndex += 1;
            newEJudge.il.REmit(ThreadCache.GetJudgeCode(), newEJudge.CurrentLabel);
            return newEJudge.TrueFunc;
        }
        public static Func<Action, EJudge> IfFalse(Action action)
        {
            EJudge newEJudge = new EJudge();
            if (action != null)
            {
                action();
            }
            newEJudge.EndLabel = newEJudge.il.DefineLabel();
            newEJudge.LabelIndex += 1;
            newEJudge.il.REmit(OpCodes.Brfalse_S, newEJudge.EndLabel);

            return newEJudge.OneFunc;
        }
        public static Func<Action, EJudge> IfTrue(Action action)
        {
            EJudge newEJudge = new EJudge();
            if (action != null)
            {
                action();
            }
            newEJudge.EndLabel = newEJudge.il.DefineLabel();
            newEJudge.LabelIndex += 1;
            newEJudge.il.REmit(OpCodes.Brtrue_S, newEJudge.EndLabel);
            return newEJudge.OneFunc;
        }
        public Func<Action, EJudge> ElseIf(object temp)
        {
            il.NoErrorLoad(temp);
            CurrentLabel = il.DefineLabel();
            LabelIndex += 1;
            il.REmit(ThreadCache.GetJudgeCode(), CurrentLabel);
            return TrueFunc;
        }
        public EJudge Else(Action action)
        {
            if (action != null)
            {
                action();
            }
            il.MarkLabel(EndLabel);
            DebugHelper.WriteLine("End-MarkLabel");
            return this;
        }
    }

    //空判断
    public partial class ENull
    {
        #region 字符串是否为空
        public static Action StringIsNull(object value)
        {
            return StringIsNull(() => { ThreadCache.GetIL().NoErrorLoad(value); });
        }
        public static Action StringIsNull(Action action)
        {
            return (()=> {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.REmit(OpCodes.Ldnull);
                il.REmit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);

            });
        }
        #endregion
        #region 对象是否为空
        public static Action IsNull(object value)
        {
            return IsNull(() => { ThreadCache.GetIL().NoErrorLoad(value); });
        }
        public static Action IsNull(Action action)
        {
            return ()=> {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.REmit(OpCodes.Ldnull);
                il.REmit(OpCodes.Call, ClassCache.ClassCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            };
        }
        #endregion

    }
    //默认值
    public partial class EDefault
    {

        public static Action IsDefault(Type type,object value)
        {
            return IsDefault(type, () => {
                ThreadCache.GetIL().NoErrorLoad(value);
            }); ;
        }
        public static Action IsDefault(Type type,Action action)
        {
            //加载对应类型的默认值
            return ()=> {
               LoadDefault(type);
               action();
               ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
            };
        }
        
    }
    public partial class EDBNull
    {
        public static Action IsDBNull(object value)
        {
            return IsDBNull(() => {
                ThreadCache.GetIL().NoErrorLoad(value);
            });
        }

        public static Action IsDBNull(Action action)
        {
            return () => {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.REmit(OpCodes.Isinst, typeof(DBNull));
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }; ;
        }
    }
}

