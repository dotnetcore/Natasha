using Microsoft.CodeAnalysis.Emit;
using System;

namespace Natasha.CSharp.Compiler.Utils
{
    internal static class PdbHelpers
    {
        public static DebugInformationFormat GetPlatformSpecificDebugInformationFormat()
        {
            // for CoreCLR & Mono, use PortablePdb
            if (NatashaCoreClrShim.ALC.Type != null || Type.GetType("Mono.Runtime") != null)
            {
                return DebugInformationFormat.PortablePdb;
            }

            // otherwise standard PDB
            return DebugInformationFormat.Pdb;
        }
    }
}
