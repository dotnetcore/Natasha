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
        private Label CurrentLabel;
        private Label EndLabel;
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
                ilHandler.MarkLabel(CurrentLabel);
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
            EData.NoErrorLoad(temp, newEJudge.ilHandler);
            newEJudge.EndLabel = newEJudge.ilHandler.DefineLabel();
            newEJudge.CurrentLabel = newEJudge.ilHandler.DefineLabel();
            newEJudge.ilHandler.Emit(ThreadCache.GetJudgeCode(), newEJudge.CurrentLabel);
            return newEJudge.TrueFunc;
        }
        public Func<Action, EJudge> ElseIf(object temp)
        {
            EData.NoErrorLoad(temp,ilHandler);
            CurrentLabel = ilHandler.DefineLabel();
            ilHandler.Emit(ThreadCache.GetJudgeCode(), CurrentLabel);
            return TrueFunc;
        }
        public EJudge Else(Action action)
        {
            if (action != null)
            {
                action();
            }
            ilHandler.MarkLabel(EndLabel);
            return this;
        }
    }

    //空判断
    public partial class ENull
    {
        #region 字符串是否为空
        public static object StringIsNull(object value)
        {
            return StringIsNull(() => { EData.NoErrorLoad(value, ThreadCache.GetIL()); });
        }
        public static object StringIsNull(Action action)
        {
            ILGenerator il = ThreadCache.GetIL();
            action();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Call, ClassCache.StringCompare);
            ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            return null;
        }
        #endregion
        #region 对象是否为空
        public static object IsNull(object value)
        {
            return IsNull(() => { EData.NoErrorLoad(value, ThreadCache.GetIL()); });
        }
        public static object IsNull(Action action)
        {
            ILGenerator il = ThreadCache.GetIL();
            action();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Call, ClassCache.ClassCompare);
            ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            return null;
        }
        #endregion

    }
    //默认值
    public partial class EDefault
    {

        public static object IsDefault(Type type,object value)
        {
            IsDefault(type,() => {
                EData.NoErrorLoad(value,ThreadCache.GetIL());
            });
            return null;
        }
        public static object IsDefault(Type type,Action action)
        {
            //加载对应类型的默认值
            LoadDefault(type);
            action();
            ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
            return null;
        }
        
    }
    public partial class EDBNull
    {
        public static object IsDBNull(object value)
        {
            IsDBNull(() => {
                EData.NoErrorLoad(value, ThreadCache.GetIL());
            });
            return null;
        }

        public static object IsDBNull(Action action)
        {
            ILGenerator il = ThreadCache.GetIL();
            action();
            il.Emit(OpCodes.Isinst, typeof(DBNull));
            ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            return null;
        }
    }
}

