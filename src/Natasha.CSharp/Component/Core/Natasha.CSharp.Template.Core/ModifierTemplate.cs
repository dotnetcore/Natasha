using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Natasha.CSharp.Template.Reverser;

namespace Natasha.CSharp.Template
{


    public class ModifierTemplate<T> : AccessTemplate<T> where T : ModifierTemplate<T>, new()
    {

        public string ModifierScript { get { return GetModifyBuilder(); } }
        private string _modifierScript;
        private readonly HashSet<ModifierFlags> _status;



        public ModifierTemplate()
        {
            _modifierScript = string.Empty;
            _status = new HashSet<ModifierFlags>();
        }



        public T Static()
        {

            _status.Add(ModifierFlags.Static);
            return Link;

        }
        public T Fixed()
        {

            _status.Add(ModifierFlags.Abstract);
            return Link;

        }
        public T Abstract()
        {

            _status.Add(ModifierFlags.Abstract);
            return Link;

        }
        public T Readonly()
        {
            return ModifierAppend("readonly");
        }

        public T New()
        {

            _status.Add(ModifierFlags.New);
            return Link;

        }
        public T Virtrual()
        {

            _status.Add(ModifierFlags.Virtual);
            return Link;

        }
        public T Override()
        {

            _status.Add(ModifierFlags.Override);
            return Link;

        }
        public T Async()
        {

            _status.Add(ModifierFlags.Async);
            return Link;

        }
        public T Unsafe()
        {

            _status.Add(ModifierFlags.Unsafe);
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
        /// 通过事件元数据反射出特殊修饰符
        /// </summary>
        /// <param name="modifierInfo">事件元数据</param>
        /// <returns></returns>
        public T Modifier(EventInfo modifierInfo)
        {

            _modifierScript = ModifierReverser.GetModifier(modifierInfo);
            return Link;

        }

        /// 通过字段元数据反射出特殊修饰符
        /// </summary>
        /// <param name="modifierInfo">字段元数据</param>
        /// <returns></returns>
        public T Modifier(FieldInfo modifierInfo)
        {

            _modifierScript = ModifierReverser.GetModifier(modifierInfo);
            return Link;

        }
        /// 通过属性元数据反射出特殊修饰符
        /// </summary>
        /// <param name="modifierInfo">属性元数据</param>
        /// <returns></returns>
        public T Modifier(PropertyInfo modifierInfo)
        {

            _modifierScript = ModifierReverser.GetModifier(modifierInfo);
            return Link;

        }

        /// <summary>
        /// 根据枚举来指定特殊修饰符
        /// </summary>
        /// <param name="modifierEnum">特殊修饰符枚举</param>
        /// <returns></returns>
        public T Modifier(ModifierFlags modifierEnum)
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
            if (_status.Contains(ModifierFlags.Static))
            {

                builder.Append("static ");

            }


            if (_status.Contains(ModifierFlags.Unsafe))
            {

                builder.Append("unsafe ");

            }


            foreach (var item in _status)
            {

                if (item != ModifierFlags.Static && item != ModifierFlags.Unsafe)
                {

                    builder.Append(ModifierReverser.GetModifier(item));

                }

            }
            builder.Append(_modifierScript);
            return builder.ToString();

        }



        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [{this}]
            base.BuilderScript();
            _script.Append(GetModifyBuilder());
            return Link;

        }

    }
}
