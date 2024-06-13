using System.Linq;

namespace FastGenericNew.Tests;

public record class DemoClassNoParamlessCtor
{
    public const int DefaultValue = 2;

    public int Value = DefaultValue;

    public DemoClassNoParamlessCtor(int value) => Value = value;
}

public record class DemoClassPrivateCtor
{
    public const int DefaultValue = 2;

    public int Value = DefaultValue;

    private DemoClassPrivateCtor() { }

    private DemoClassPrivateCtor(int val) => Value = val;

    public static DemoClassPrivateCtor Create() => new();

    public static DemoClassPrivateCtor Create(int val) => new(val);
}

public record class DemoClass
{
    public const int DefaultValue = 2;

    public int Value = DefaultValue;

    public string? AllValues;

    public int Mode;

    public DemoClass() { }

    public DemoClass(int val)
    {
        Mode = 1;
        Value = val;
    }

    public DemoClass(int val, int val2)
    {
        Mode = 2;
        AllValues = string.Join(",", new[] { val, val2 }.Select(x => x.ToString()).ToArray());
    }

    public DemoClass(int val, int val2, int val3, int val4, int val5, int val6, int val7, int val8, int val9, int val10, int val11, int val12, int val13, int val14, int val15, int val16, int val17, int val18)
    {
        Mode = 3;
        AllValues = string.Join(",", new[] { val, val2, val3, val4, val5, val6, val7, val8, val9, val10, val11, val12, val13, val14, val15, val16, val17, val18 }.Select(x => x.ToString()).ToArray());
    }
}

public record struct DemoStruct
{
    public int Value;
}

public record struct DemoStructParameterless
{
    public const int ParameterlessValue = 999999;

    public int Value;

    public DemoStructParameterless()
    {
        Value = ParameterlessValue;
    }
}