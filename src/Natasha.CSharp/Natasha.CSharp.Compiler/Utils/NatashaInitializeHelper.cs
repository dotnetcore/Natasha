using Natasha.DynamicLoad.Base;
using System;
using System.Reflection;

namespace Natasha.CSharp.Compiler.Utils
{
    public class NatashaInitializeHelper
    {
        private bool? _useRuntimeUsing = null;
        private bool? _useRuntimeReference = null;
        private bool _useFileCache;
        private Func<AssemblyName?, string?, bool>? _excludeReferencesFunc;

        public NatashaInitializeHelper WithMemoryReference()
        {
            _useRuntimeReference = true;
            return this;
        }

        public NatashaInitializeHelper WithRefReference()
        {
            _useRuntimeReference = false;
            return this;
        }

        public NatashaInitializeHelper WithMemoryUsing()
        {
            _useRuntimeUsing = true;
            return this;
        }

        public NatashaInitializeHelper WithRefUsing()
        {
            _useRuntimeUsing = false;
            return this;
        }

        public NatashaInitializeHelper WithFileUsingCache()
        {
            _useFileCache = true;
            return this;
        }


        /// <summary>
        /// 排除对应的程序集，委托返回 true 则排除， false 则不排除。
        /// </summary>
        /// <param name="excludeReferencesFunc"></param>
        /// <returns></returns>
        public NatashaInitializeHelper WithExcludeReferences(Func<AssemblyName?, string?, bool>? excludeReferencesFunc)
        {
            _excludeReferencesFunc = excludeReferencesFunc;
            return this;
        }
        public void Preheating()
        {
            NatashaManagement.Preheating(_excludeReferencesFunc, _useRuntimeUsing, _useRuntimeReference, _useFileCache);
        }
        public void Preheating<T>() where T : INatashaDynamicLoadContextCreator, new()
        {
            NatashaManagement.Preheating<T>(_excludeReferencesFunc, _useRuntimeUsing, _useRuntimeReference, _useFileCache);
        }
    }
}
