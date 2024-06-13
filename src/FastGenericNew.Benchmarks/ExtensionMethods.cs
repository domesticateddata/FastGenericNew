namespace FastGenericNew.Benchmarks;

public static unsafe class ExtensionMethods
{
    public static delegate* managed<void> GetStaticCtor(this Type type) => (delegate* managed<void>)type.TypeInitializer!.MethodHandle.GetFunctionPointer();
}