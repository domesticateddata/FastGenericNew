﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by FastGenericNew.SourceGenerator
//     Please do not modify this file directly
// <auto-generated/>
//------------------------------------------------------------------------------
#define FastNewPX_AllowUnsafeImplementation
#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;

#if NET6_0_OR_GREATER && FastNewPX_AllowUnsafeImplementation
namespace @FastGenericNew
{
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    static unsafe class ClrAllocator
    {
        public static readonly delegate*<void*, ref delegate*<void*, object>, ref void*, ref delegate*<object, void>, int*, void> GetActivationInfo;

        public static readonly bool IsSupported;

        static ClrAllocator()
        {
            foreach (var met in typeof(global::System.RuntimeTypeHandle).GetMethods(global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.NonPublic))
            {
                if (met.Name == "GetActivationInfo" && (met.Attributes & global::System.Reflection.MethodAttributes.PinvokeImpl) != 0)
                {
                    var parameters = met.GetParameters();
                    // TODO Consider to use list pattern when available
                    // Double-check the method
                    if (
                        parameters.Length == 5
                        && parameters[0].ParameterType == Type.GetType("System.Runtime.CompilerServices.ObjectHandleOnStack", false)
                        && parameters[1].ParameterType == typeof(delegate*<void*, object>*)
                        && parameters[2].ParameterType == typeof(void**)
                        && parameters[3].ParameterType == typeof(delegate*<object, void>*)
                        // && parameters[4].ParameterType == Type.GetType("Interop.BOOL", false)
                        )
                    {
                        GetActivationInfo = (delegate*<void*, ref delegate*<void*, object>, ref void*, ref delegate*<object, void>, int*, void>)
                            met.MethodHandle.GetFunctionPointer();
                        IsSupported = true;
                    }
                }
            }
        }

		public static void CtorNoopStub(object _) { }
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining | global::System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
		public static object ThrowNotSupported(void* _) => throw new global::System.NotSupportedException();
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining | global::System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        public static object SmartThrow<T>(void* _) => (object)global::@FastGenericNew.ThrowHelper.SmartThrowImpl<T>()!;
    }

    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    static unsafe class ClrAllocator<T>
    {
        private static readonly delegate*<void*, object> _pfnAllocator;

        private static readonly void* _allocatorFirstArg;

        private static readonly delegate*<object, void> _pfnCtor;

        /// <summary>
        /// TRUE means ClrAllocator&lt;T&gt; can fully replace to FastNewCore<br>
        /// So this can be TRUE even the CreateInstance will throw an exception.
        /// </summary>
        public static readonly bool IsSupported;

        static ClrAllocator()
        {
            if (!global::@FastGenericNew.ClrAllocator.IsSupported) goto MarkUnsupported;
            var type = typeof(T);

            int _ctorIsPublic = default;
            try
            {
                ((delegate*<void*, ref delegate*<void*, object>, ref void*, ref delegate*<object, void>, int*, void>)global::@FastGenericNew.ClrAllocator.GetActivationInfo)
                    (Unsafe.AsPointer(ref type), ref _pfnAllocator, ref _allocatorFirstArg, ref _pfnCtor, &_ctorIsPublic);
            }
            catch
            {
                // Exceptions SmartThrow can handle.
                //if (type.IsAbstract) goto GoSmartThrow;

                // GetActivationInfo has many extra limits
                // https://github.com/dotnet/runtime/blob/a5ec8aa173e4bc76b173a70aa7fa3be1867011eb/src/coreclr/vm/reflectioninvocation.cpp#L1942:25
  
                // Exceptions SmartThrow CAN NOT handle.
                // Mark unsupported so FastGenericNew will use FastNewCore instead if hit any
                // 
                if (
                    type.IsArray // typeHandle.IsArray()
                    || type.IsByRefLike // pMT->IsByRefLike()
                    || type == typeof(string) // pMT->HasComponentSize()
                    || typeof(Delegate).IsAssignableFrom(type) // pMT->IsDelegate()
                    ) 
                    goto MarkUnsupported;
            }

            if (_pfnAllocator is null)
                goto GoSmartThrow;

            if (_pfnCtor is null)
                _pfnCtor = &global::@FastGenericNew.ClrAllocator.CtorNoopStub;

            IsSupported = true;
            return;

GoSmartThrow:
            _pfnAllocator = &global::@FastGenericNew.ClrAllocator.SmartThrow<T>;
            IsSupported = true; // read the comment of IsSupported
            return;

MarkUnsupported:
            _pfnAllocator = &global::@FastGenericNew.ClrAllocator.ThrowNotSupported;
            IsSupported = false;
            return;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CreateInstance()
        {
            var result = _pfnAllocator(_allocatorFirstArg);
            _pfnCtor(result);
            return (T)result;
        }
    }
}
#endif
