using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Builder
{
    public static class ComplierBuilder
    {
        public static void Create(AssemblyComplier complier, ComplierResultTarget target, ComplierResultError error)
        {
            complier.EnumCRTarget = target;

        }

        public static void Create(AssemblyComplier complier, ComplierResultError error, ComplierResultTarget target)
        {

        }
    }
}
