using System;
using System.Collections.Generic;
using System.Text;

namespace RefAssembly.Compile.Oop
{
    internal class OopCheckHelper
    {
        internal static void SimpleCheckWrapper(string oopKind, bool hasSpace,bool isGeneric = false)
        {
            string oopName = NameGenerator.GetRandomName();
            string? nameSpace = null;
            string script;
            if (hasSpace)
            {
                nameSpace = NameGenerator.GetRandomName();
                script = $"namespace {nameSpace} {{  public {oopKind} {oopName}{ (isGeneric == true ? "<T>" : string.Empty) }{{ }} }}";
            }
            else
            {
                script = $"public {oopKind} {oopName}{(isGeneric == true ? "<T>" : string.Empty)}{{ }}";
            }

            try
            {
                var asm = script.GetAssemblyForUT(builder=>builder.ConfigLoadContext(opt=>opt.AddReferenceAndUsingCode<object>()));
                if (isGeneric)
                {
                    oopName += "`1";
                }
                var realType = asm.GetTypeFromShortName(oopName);
                Assert.Equal(nameSpace, realType.Namespace);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
