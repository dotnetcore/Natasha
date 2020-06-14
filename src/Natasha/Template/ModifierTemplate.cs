using Natasha.Reverser;
using Natasha.Reverser.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template
{
    

    public class ModifierTemplate<T> : AccessTemplate<T> where T : ModifierTemplate<T>, new()
    {

        public string ModifierScript { get { return GetModifyBuilder(); } }
        private string _modifierScript;
        private readonly HashSet<Modifiers> _status;



        public ModifierTemplate()
        {
            _status = new HashSet<Modifiers>();
        }



        public T Static()
        {

            _status.Add(Modifiers.Static);
            return Link;

        }
        public T Abstract()
        {

            _status.Add(Modifiers.Abstract);
            return Link;

        }
        public T Readonly()
        {
            return ModifierAppend("readonly");
        }

        public T New()
        {

            _status.Add(Modifiers.New);
            return Link;

        }
        public T Virtrual()
        {

            _status.Add(Modifiers.Virtual);
            return Link;

        }
        public T Override()
        {

            _status.Add(Modifiers.Override);
            return Link;

        }
        public T Async()
        {

            _status.Add(Modifiers.Async);
            return Link;

        }
        public T Unsafe()
        {

            _status.Add(Modifiers.Unsafe);
            return Link;

        }


        /// <summary>
        /// 根据类型反射得到保护级别
        /// </summary>
        /// <param name="type">外部类型</param>
        /// <returns></returns>
        public T Modifier(Type type)
        {

            _modifierScript = ModifierReverser.GetModifier(type);
            return Link;

        }




        /// <summary>
        /// 通过方法元数据反射出特殊修饰符
        /// </summary>
        /// <param name="modifierInfo">方法元数据</param>
        /// <returns></returns>
        public T Modifier(MethodInfo modifierInfo)
        {

            _modifierScript = ModifierReverser.GetModifier(modifierInfo);
            return Link;

        }




        /// <summary>
        /// 根据枚举来指定特殊修饰符
        /// </summary>
        /// <param name="modifierEnum">特殊修饰符枚举</param>
        /// <returns></returns>
        public T Modifier(Modifiers modifierEnum)
        {

            _status.Add(modifierEnum);
            return Link;

        }




        /// <summary>
        /// 根据字符串来指定特殊修饰符
        /// </summary>
        /// <param name="modifierString">特殊修饰符字符串</param>
        /// <returns></returns>
        public T Modifier(string modifierString)
        {
            _modifierScript = ScriptWrapper(modifierString);
            return Link;

        }
        public T ModifierAppend(string modifierString)
        {

            _modifierScript += ScriptWrapper(modifierString);
            return Link;

        }


        public string GetModifyBuilder()
        {

            StringBuilder builder = new StringBuilder();
            if (_status.Contains(Modifiers.Static))
            {

                builder.Append("static ");

            }


            if (_status.Contains(Modifiers.Unsafe))
            {

                builder.Append("unsafe ");

            }


            foreach (var item in _status)
            {

                if (item != Modifiers.Static && item != Modifiers.Unsafe)
                {

                    builder.Append(ModifierReverser.GetModifier(item));

                }

            }
            builder.Append(_modifierScript);
            return builder.ToString();

        }



        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [{this}]
            base.BuilderScript();
            _script.Append(GetModifyBuilder());
            return Link;

        }

    }
}
