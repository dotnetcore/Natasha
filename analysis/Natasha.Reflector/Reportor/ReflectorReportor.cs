using Console.Formator;
using Natasha.Reflection;
using Natasha.Reflection.Reportor;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{
    public class ReflectionReportor
    {
        public Dictionary<MemberInfo, FeasibilityHandler> Handlers;

        private Type _type;

        private FieldInfo[] Fields;
        private PropertyInfo[] Properties;
        private MethodInfo[] Methods;

        private List<StringBuilder> InfosCache;

        private StringBuilder TableHeader;
        private StringBuilder DeclaringInfo;

        private string Splite;
        private string Replace;
        public ReflectionReportor()
        {
            Splite = "@|@";
            Handlers = new Dictionary<MemberInfo, FeasibilityHandler>();
            InfosCache = new List<StringBuilder>();
            TableHeader = new StringBuilder();
            DeclaringInfo = new StringBuilder();
            _start = new StringBuilder();
            _split = new StringBuilder();
            _end = new StringBuilder();
            InfosCache.Add(TableHeader);
            InfosCache.Add(DeclaringInfo);
            InfosCache.Add(new StringBuilder());
        }
        public ReflectionReportor(Type type) : this()
        {
            _type = type;

            Fields = _type.GetFields(Reflector.Flag);
            Properties = _type.GetProperties(Reflector.Flag);

            InitAnalysisCache(Fields);
            InitAnalysisCache(Properties);

        }
        public ReflectionReportor(FieldInfo info) : this()
        {
            _type = info.DeclaringType;
            Fields = new FieldInfo[] { info };
            InitAnalysisCache(info);
        }
        public ReflectionReportor(MethodInfo info) : this()
        {
            _type = info.DeclaringType;
            Methods = new MethodInfo[] { info };
            InitAnalysisCache(info);
        }
        public ReflectionReportor(PropertyInfo info) : this()
        {
            _type = info.DeclaringType;
            Properties = new PropertyInfo[] { info };
            InitAnalysisCache(info);
        }

        /// <summary>
        /// 分析类型
        /// </summary>
        public void Analysis()
        {
            SetMemberType();
            SetMemberName();
            ProtectionAnalysis();
            ParticularityAnalysis();
            SecurityAnalysis();
            SetAnalysis();
            GetAnalysis();
            EmitAnalysis();
        }
        /// <summary>
        /// 设置成员类型
        /// </summary>
        private void SetMemberType()
        {
            InfosCache.ForEach(item => item.Append(Splite));
            Replace = null;
            TableHeader.Append("成员类型");

            DeclaringInfo.Append("父类信息");

            FieldAnalysis((handler, info) =>
            {
                handler.SetMemberType(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.SetMemberType(info);
            });
            MethodAnalysis((handler, info) =>
            {
                handler.SetMemberType(info);
            });
            Replace = "│";
        }
        /// <summary>
        /// 设置成员名称
        /// </summary>
        private void SetMemberName()
        {
            TableHeader.Append("成员名称");

            var declaringHandler = new FeasibilityHandler();
            declaringHandler.DeclaringAnalysis(_type);
            DeclaringInfo.Append(declaringHandler.ResultRecoder);

            FieldAnalysis((handler, info) =>
            {
                handler.SetMemberName(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.SetMemberName(info);
            });
            MethodAnalysis((handler, info) =>
            {
                handler.SetMemberName(info);
            });
        }
        /// <summary>
        /// 保护级别分析
        /// </summary>
        private void ProtectionAnalysis()
        {
            TableHeader.Append("访问级别");

            FieldAnalysis((handler, info) =>
            {
                handler.ProtectionAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.ProtectionAnalysis(info);
            });
            MethodAnalysis((handler, info) =>
            {
                handler.ProtectionAnalysis(info);
            });
        }
        /// <summary>
        /// 特性分析
        /// </summary>
        private void ParticularityAnalysis()
        {

            TableHeader.Append("特性");

            FieldAnalysis((handler, info) =>
            {
                handler.ParticularityAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.ParticularityAnalysis(info);
            });
            MethodAnalysis((handler, info) =>
            {
                handler.ParticularityAnalysis(info);
            });

        }
        /// <summary>
        /// Set操作分析
        /// </summary>
        private void SetAnalysis()
        {

            TableHeader.Append("Set/赋值操作");

            FieldAnalysis((handler, info) =>
            {
                handler.SetOperatorAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.SetOperatorAnalysis(info);
            });
            MethodAnalysis();
        }
        /// <summary>
        /// Get操作分析
        /// </summary>
        private void GetAnalysis()
        {
            TableHeader.Append("Get/赋值操作");

            FieldAnalysis((handler, info) =>
            {
                handler.GetOperatorAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.GetOperatorAnalysis(info);
            });
            MethodAnalysis();
        }
        /// <summary>
        /// 安全性分析
        /// </summary>
        private void SecurityAnalysis()
        {
            TableHeader.Append("安全性");

            FieldAnalysis((handler, info) =>
            {
                handler.SecurityAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.SecurityAnalysis(info);
            });
            MethodAnalysis((handler, info) =>
            {
                handler.SecurityAnalysis(info);
            });
        }
        /// <summary>
        /// Emit可行性分析
        /// </summary>
        private void EmitAnalysis()
        {
            TableHeader.Append("Emit读写");

            FieldAnalysis((handler, info) =>
            {
                handler.EmitAnalysis(info);
            });
            PropertyAnalysis((handler, info) =>
            {
                handler.EmitAnalysis(info);
            });
            MethodAnalysis(null);
        }

        #region 分析方法封装
        private void FieldAnalysis(Action<FeasibilityHandler, FieldInfo> analysisAction)
        {
            foreach (var item in Fields)
            {
                var handler = Handlers[item];
                analysisAction(handler, item);
            }
        }
        private void PropertyAnalysis(Action<FeasibilityHandler, PropertyInfo> analysisAction)
        {
            foreach (var item in Properties)
            {
                var handler = Handlers[item];
                analysisAction(handler, item);
            }
        }

        private void PropertySetterAnalysis(Action<FeasibilityHandler, PropertyInfo, bool> analysisAction)
        {
            foreach (var item in Properties)
            {
                var handler = Handlers[item];
                analysisAction(handler, item, true);
            }
        }

        private void PropertyGetterAnalysis(Action<FeasibilityHandler, PropertyInfo> analysisAction)
        {
            foreach (var item in Properties)
            {
                var handler = Handlers[item];
                analysisAction(handler, item);
            }
        }
        private void MethodAnalysis(Action<FeasibilityHandler, MethodInfo> analysisAction = null)
        {
            if (Methods != null)
            {
                foreach (var item in Methods)
                {
                    var handler = Handlers[item];
                    analysisAction?.Invoke(handler, item);
                }
            }

            Alignmentor.Alignment(InfosCache, Splite, Replace, AlignmentType.Center);
        }
        #endregion

        public void InitAnalysisCache(params MemberInfo[] members)
        {
            if (members != null)
            {
                foreach (var item in members)
                {
                    FeasibilityHandler handler = new FeasibilityHandler();
                    Handlers[item] = handler;
                    InfosCache.Add(handler.ResultRecoder);
                    InfosCache.Add(new StringBuilder());
                }
            }
        }

        public void AddSpliteNode()
        {
            _start.Append('┬');
            _split.Append('┼');
            _end.Append('┴');
        }
        private StringBuilder _start;
        private StringBuilder _split;
        private StringBuilder _end;

        public void Show()
        {

            InfosCache.ForEach(item => item.RemoveLastest(Splite.Length));

            int RealLength = Alignmentor.Packet(InfosCache, "││", AlignmentType.Even);

            _start.Append('┌');
            _split.Append('├');
            _end.Append('└');

            for (int i = 0; i < RealLength - 11; i +=1)
            {
                _split.Append('─');
                _start.Append('─');
                _end.Append('─');
            }
            _start.Append('┐');
            _split.Append('┤');
            _end.Append('┘');

            StringBuilder result = new StringBuilder();
            result.AppendLine(_start.ToString());
            result.AppendLine(TableHeader.ToString());
            result.AppendLine(_split.ToString());
            InfosCache.RemoveAt(0);
            InfosCache.ForEach(item => result.AppendLine(item.ToString()));
            result.AppendLine(_end.ToString());

            int fatherClassColor = 0;
            for (int i = 0; i < result.Length; i += 1)
            {
                if (fatherClassColor != 0)
                {
                    if (fatherClassColor == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (fatherClassColor<i && result[i] == '│')
                    {
                        Console.ResetColor();
                        fatherClassColor = 0;
                    }
                    Console.Write(result[i]);
                }
                else
                {
                    if (result[i] == '父')
                    {
                        fatherClassColor = i + 5;
                    }

                    if (result[i] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (result[i] == '√')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write(result[i]);
                    Console.ResetColor();
                }
            }
        }
    }

    public class ReflectionReport<T> : ReflectionReportor
    {
        public ReflectionReport() : base(typeof(T))
        {

        }
    }



}
