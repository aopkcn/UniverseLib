using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using InlineIL;
using Unity.Collections.LowLevel.Unsafe;
using static InlineIL.IL.Emit;

namespace ECSExtension.Util
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "EntityNameCapturedOnly.Global")]
    internal static class ModUnsafe
    {
        public static int AlignOf<U>() where U : unmanaged => SizeOf<UnsafeUtility.AlignOfHelper<U>>() - SizeOf<U>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Read<T>(void* source)
        {
            Ldarg_0();
            Ldobj<T>();
            return IL.Return<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy<T>(void* destination, ref T source)
        {
            Ldarg_0();
            Ldarg_1();
            Ldobj<T>();
            Stobj<T>();
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe U ReadArrayElement<U>(void* source, int index) where U : unmanaged
        {
            Ldarg_0();
            Ldarg_1();
            Conv_I8();
            Sizeof<U>();
            Conv_I8();
            Mul();
            Conv_I();
            Add();
            Ldobj<U>();
            return IL.Return<U>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteArrayElement<U>(void* destination, int index, U value) where U : unmanaged
        {
            Ldarg_0();
            Ldarg_1();
            Conv_I8();
            Sizeof<U>();
            Conv_I8();
            Mul();
            Conv_I();
            Add();
            Ldarg_2();
            Stobj<U>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref T ArrayElementAsRef<T>(void* ptr, int index) where T : unmanaged
        {
            IL.DeclareLocals(
                false,
                new LocalVar("local", typeof(int).MakeByRefType())
            );        
            
            Ldarg_0();
            Ldarg_1();
            Conv_I8();
            Sizeof<T>();
            Conv_I8();
            Mul();
            Conv_I();
            Add();
            Stloc("local");
            Ldloc("local");
            return ref IL.ReturnRef<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>() where T : unmanaged
        {
            Sizeof(typeof(T));
            return IL.Return<int>();
        }
    }
}