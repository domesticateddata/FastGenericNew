
using FluentAssertions;

namespace FastGenericNew.Tests;

public class TypeNewTest
{
    #region Exceptions should happen

    [Test]
    public void ExceptionTypeNewVoid()
    {
        // Use FluentAssertions for exception testing
        Action action = () => FastNew.GetCreateInstance<object>(typeof(void));
        action.Should().Throw<ArgumentException>("because creating an instance of 'void' type is invalid.");
    }

    #endregion

    #region ValueTypes

    [Test(Description = "Check if FastNew works on primary types")]
    [TestCaseSource(nameof(GetValueTypes))]
    [Parallelizable(ParallelScope.All)]
    public void ValueTypeCreateInstance<T>(T defaultVal) where T : new()
    {
        // Ensure T is a value type
        typeof(T).IsValueType.Should().BeTrue("T must be a value type");

        // Compare expected and actual instances
        var expected = Activator.CreateInstance<T>();
        var fastNewInst = FastNew.GetCreateInstance<T>(typeof(T)).Invoke();

        fastNewInst.Should().Be(expected, "the instance created by FastNew should be equivalent to the one created by Activator.");
    }

    private static IEnumerable GetValueTypes()
    {
        #region Primary Value Types

        yield return default(char);

        yield return default(byte);
        yield return default(sbyte);
        yield return default(short);
        yield return default(ushort);
        yield return default(int);
        yield return default(uint);
        yield return default(long);
        yield return default(ulong);

        yield return default(float);
        yield return default(double);
        yield return default(decimal);

        yield return default(nint);
        yield return default(nuint);

        #endregion

        yield return default(DateTime);
        yield return default(Guid);
        yield return default(TypeCode);

        yield return default(DemoStruct);
#if !NETFRAMEWORK
        yield return default(DemoStructParameterless);
#endif
    }

    #endregion
}
