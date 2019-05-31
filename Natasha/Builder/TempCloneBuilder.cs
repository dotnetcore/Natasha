using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class TempCloneBuilder : TypeIterator
    {
        public StringBuilder Script;
        public Type CloneType;
        public const string NewInstance = "NewInstance";
        public const string OldInstance = "OldInstance";
        public static ConcurrentDictionary<Type, Delegate> CloneCache;

        static TempCloneBuilder() => CloneCache = new ConcurrentDictionary<Type, Delegate>();

        public TempCloneBuilder(Type type) {
            CloneType = type;
            Script = new StringBuilder();
        }
        public override void StartHandler(BuilderInfo buildInfo)
        {
            Script.Append($"if({OldInstance}==null){{return null;}}");
            Script.Append($"{buildInfo.ClassName} {NewInstance} = new {buildInfo.ClassName}();");
        }
        public override void MemberHandler(BuilderInfo buildInfo)
        {
            Script.Append($"{NewInstance}.{buildInfo.Name} = {OldInstance}.{buildInfo.Name};");
        }
        public override void MemberArrayHandler(BuilderInfo buildInfo)
        {
            Script.Append(
                $@"
                if({OldInstance}.{buildInfo.Name}!=null)
                {{
                    {NewInstance}.{buildInfo.Name}=new {buildInfo.ClassName}[{OldInstance}.{buildInfo.Name}.Length];
                    for(int i=0;i<{OldInstance}.{buildInfo.Name}.Length;i+=1)
                    {{
                        {NewInstance}.{buildInfo.Name}[i]={OldInstance}.{buildInfo.Name}[i];
                    }}
                }}");
        }
        public override void MemberArrayEntitiesHandler(BuilderInfo buildInfo)
        {
            Script.Append(
                $@"
                if({OldInstance}.{buildInfo.Name}!=null)
                {{
                    {NewInstance}.{buildInfo.Name}=new {buildInfo.ClassName}[{OldInstance}.{buildInfo.Name}.Length];
                    for(int i=0;i<{OldInstance}.{buildInfo.Name}.Length;i+=1)
                    {{
                        {NewInstance}.{buildInfo.Name}[i]=NatashaClone{buildInfo.AvailableName}.Clone({OldInstance}.{buildInfo.Name}[i]);
                    }}
                }}");
        }
        public override void ReturnHandler(BuilderInfo buildInfo)
        {
            Script.Append($"return {NewInstance};");
        }
    }
}
