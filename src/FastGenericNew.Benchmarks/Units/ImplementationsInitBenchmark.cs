using System.Runtime.Serialization;

namespace FastGenericNew.Benchmarks.Units;

#if NET6_0_OR_GREATER
public unsafe class ImplementationsInitBenchmark
{
    public static readonly delegate* managed<void> ClrNew = typeof(ClrAllocator<DemoClass>).GetStaticCtor();

    public static readonly delegate* managed<void> IlNew = typeof(FastNew<DemoClass>).GetStaticCtor();

    public static readonly delegate* managed<object, Type, void> CtorActivatorCache =
        (delegate* managed<object, Type, void>)
        Type.GetType("System.RuntimeType")!
        .GetNestedType("ActivatorCache", BindingFlags.NonPublic)!
        .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { Type.GetType("System.RuntimeType")! }, null)!
        .MethodHandle
        .GetFunctionPointer();

    private static readonly object InstActivatorCache = FormatterServices.GetUninitializedObject(Type.GetType("System.RuntimeType")!.GetNestedType("ActivatorCache", BindingFlags.NonPublic | BindingFlags.Instance)!);

    [Benchmark]
    public void InitClrNew()
    {
        ClrNew();
    }

    [Benchmark(Baseline = true)]
    public void InitActivator()
    {
        CtorActivatorCache(InstActivatorCache, typeof(DemoClass));
    }

    [Benchmark]
    public void InitFastNewCore()
    {
        IlNew();
    }
}
#endif
