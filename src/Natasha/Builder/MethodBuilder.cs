using Natasha.Template;
using System;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 方法脚本构造器
    /// </summary>
    public class MethodBuilder
    {


        public ClassBuilder ClassTemplate;
        public MethodTemplate MethodTemplate;
        public ClassComplier ComplierOption;

        public MethodBuilder()
        {

            ClassTemplate = new ClassBuilder();
            MethodTemplate = new MethodTemplate();
            ComplierOption = new ClassComplier(); 

        }




        /// <summary>
        /// 获取方法脚本
        /// </summary>
        public string MethodScript
        {

            get { return MethodTemplate.Builder().Script; }

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
        public virtual MethodBuilder MethodAction(Action<MethodTemplate> methodAction)
        {

            methodAction(MethodTemplate);
            return this;

        }


        /// <summary>
        /// 编译并返回委托
        /// </summary>
        /// <returns></returns>
        public Delegate Complie()
        {

            //获取程序集
            Assembly assembly = ComplierOption.GetAssemblyByScript(ClassTemplate
                .Using(MethodTemplate.UsingRecoder.Types)
                .ClassBody(MethodTemplate.Builder()._script)
                .Builder().Script);


            //判空
            if (assembly == null)
            {

                return null;

            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[ClassTemplate.ClassNameScript]
                .GetMethod(MethodTemplate.MethodNameScript)
                .CreateDelegate(MethodTemplate.DelegateType);

        }

    }

}
