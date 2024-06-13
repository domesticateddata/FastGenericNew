using FluentAssertions;

namespace FastGenericNew.Tests.Units.FastNewTests;

public class NewOrDefaultTests
{
    [TestCaseSourceGeneric(typeof(TestData), nameof(TestData.CommonValueTypes))]
    [Parallelizable(ParallelScope.All)]
    public void ValueTypeDefault<T>()
    {
        // Verify that T is a value type
        typeof(T).IsValueType.Should().BeTrue("because T must be a value type");

        // Existing comparison logic
        var expected = default(T);
        var actual = FastNew.NewOrDefault<T>();
        expected.Should().Be(actual);
    }
}
