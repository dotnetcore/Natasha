using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 方法脚本构造器
    /// </summary>
    public class MethodBuilder :IComplier
    {
        //private readonly static Regex _get_class;
        static MethodBuilder()
        {
            //_get_class = new Regex(@"\sclass.*?(?<result>[a-zA-Z0-9]*?)[\s]*{", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public ClassBuilder ClassTemplate;
        public MethodTemplate MethodTemplate;
        internal (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) _info;

        public MethodBuilder()
        {
            ClassTemplate = new ClassBuilder();
            MethodTemplate = new MethodTemplate();
        }
        



        /// <summary>
        /// 获取方法脚本
        /// </summary>
        public string MethodScript
        {
            get { return _info.Script; }
        }




        /// <summary>
        /// 重置类型模板
        /// </summary>
        /// <param name="classTemplate">新的类型模板</param>
        /// <returns></returns>
        public virtual MethodBuilder ResetClassTemplate(ClassBuilder classTemplate)
        {
            ClassTemplate = classTemplate;
            return this;
        }




        /// <summary>
        /// 根据委托来定制类型模板
        /// </summary>
        /// <param name="classAction">类模板委托</param>
        /// <returns></returns>
        public virtual MethodBuilder ClassAction(Action<ClassBuilder> classAction)
        {
            classAction(ClassTemplate);
            return this;
        }




        /// <summary>
        /// 重置方法模板
        /// </summary>
        /// <param name="methodTemplate">方法模板</param>
        /// <returns></returns>
        public virtual MethodBuilder ResetBodyTemplate(MethodTemplate methodTemplate)
        {
            MethodTemplate = methodTemplate;
            return this;
        }




        /// <summary>
        /// 根据委托来定制方法模板
        /// </summary>
        /// <param name="methodAction">方法模板委托</param>
        /// <returns></returns>
        public virtual MethodBuilder UseBodyTemplate(Action<MethodTemplate> methodAction)
        {
            methodAction(MethodTemplate);
            return this;
        }




        /// <summary>
        /// 构建完整脚本
        /// </summary>
        /// <returns></returns>
        public override string Builder()
        {
            //获取方法脚本以及其他信息
            _info = MethodTemplate.Package();

            
            //生成完整脚本
            return ClassTemplate
                .Using(_info.Types)
                .ClassBody(_info.Script)
                .Builder();
        }




        /// <summary>
        /// 编译并返回委托
        /// </summary>
        /// <returns></returns>
        public override Delegate Complie()
        {
            //获取程序集
            Assembly assembly = GetAssemblyByScript(ClassTemplate.NameScript);


            //判空
            if (assembly == null)
            {
                return null;
            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[ClassTemplate.NameScript]
                .GetMethod(_info.Flag)
                .CreateDelegate(_info.Delegate);
        }

    }
}
