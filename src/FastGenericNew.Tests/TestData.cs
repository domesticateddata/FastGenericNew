namespace FastGenericNew.Tests;

public static class TestData
{
    public static readonly Type[] CommonReferenceTypesPl =
    [
        typeof(DemoClass)
    ];

    public static readonly Type[] CommonValueTypes = {
        #region Primary Value Types

        typeof(char),

        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),

        typeof(float),
        typeof(double),
        typeof(decimal),

        typeof(nint),
        typeof(nuint),

        #endregion

        typeof(DateTime),
        typeof(Guid),
        typeof(TypeCode), // Enum

        typeof(DemoStruct),
    };
}