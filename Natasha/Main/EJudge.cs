using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha
{
    //IF ELSE
    public class EJudge : ILGeneratorBase
    {
        private Func<Action, EJudge> TrueFunc;
        private Label CurrentLabel;
        private Label EndLabel;
        private int LabelIndex;
        static EJudge() { }
        private void Jump(Label label)
        {
            ilHandler.Emit(OpCodes.Br_S, label);
            
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
                DebugHelper.WriteLine("JumpTo","End");
                ilHandler.MarkLabel(CurrentLabel);
                DebugHelper.WriteLine("SetLabel", LabelIndex);
                return this;
            };
        }

        //正确则跳转
        public static void TrueJump(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Brtrue_S, label);
            DebugHelper.WriteLine("Brtrue_S", label.ToString());
        }
        //错误则跳转
        public static void FalseJump(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Brfalse_S, label);
            DebugHelper.WriteLine("Brfalse_S", label.ToString());
        }
        //直接跳转
        public static void JumpTo(Label label)
        {
            ThreadCache.GetIL().Emit(OpCodes.Br_S, label);
            DebugHelper.WriteLine("Br_S", label.ToString());
        }
        //其他跳转方式 已经由重载运算符完成
        public static Func<Action, EJudge> If(object temp)
        {
            EJudge newEJudge = new EJudge();
            DataHelper.NoErrorLoad(temp, newEJudge.ilHandler);
            newEJudge.EndLabel = newEJudge.ilHandler.DefineLabel();
            newEJudge.CurrentLabel = newEJudge.ilHandler.DefineLabel();
            newEJudge.LabelIndex += 1;
            newEJudge.ilHandler.Emit(ThreadCache.GetJudgeCode(), newEJudge.CurrentLabel);
            DebugHelper.WriteLine(ThreadCache.GetJudgeCode().Name, newEJudge.LabelIndex);
            return newEJudge.TrueFunc;
        }
        public Func<Action, EJudge> ElseIf(object temp)
        {
            DataHelper.NoErrorLoad(temp,ilHandler);
            CurrentLabel = ilHandler.DefineLabel();
            LabelIndex += 1;
            ilHandler.Emit(ThreadCache.GetJudgeCode(), CurrentLabel);
            DebugHelper.WriteLine(ThreadCache.GetJudgeCode().Name, LabelIndex);
            return TrueFunc;
        }
        public EJudge Else(Action action)
        {
            if (action != null)
            {
                action();
            }
            ilHandler.MarkLabel(EndLabel);
            DebugHelper.WriteLine("SetLabel","End");
            return this;
        }
    }

    //空判断
    public partial class ENull
    {
        #region 字符串是否为空
        public static Action StringIsNull(object value)
        {
            return StringIsNull(() => { DataHelper.NoErrorLoad(value, ThreadCache.GetIL()); });
        }
        public static Action StringIsNull(Action action)
        {
            return (()=> {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);

                DebugHelper.WriteLine("Ldnull");
                DebugHelper.WriteLine("Call", ClassCache.StringCompare.Name);
            });
        }
        #endregion
        #region 对象是否为空
        public static Action IsNull(object value)
        {
            return IsNull(() => { DataHelper.NoErrorLoad(value, ThreadCache.GetIL()); });
        }
        public static Action IsNull(Action action)
        {
            return ()=> {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Call, ClassCache.ClassCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);

                DebugHelper.WriteLine("Ldnull");
                DebugHelper.WriteLine("Call", ClassCache.ClassCompare.Name);
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
                DataHelper.NoErrorLoad(value, ThreadCache.GetIL());
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
                DataHelper.NoErrorLoad(value, ThreadCache.GetIL());
            });
        }

        public static Action IsDBNull(Action action)
        {
            return () => {
                ILGenerator il = ThreadCache.GetIL();
                action();
                il.Emit(OpCodes.Isinst, typeof(DBNull));
                DebugHelper.WriteLine("Isinst", typeof(DBNull).Name);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }; ;
        }
    }
}

