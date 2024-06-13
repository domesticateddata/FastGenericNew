#if AllowUnsafeImplementation && NET8_0_OR_GREATER
using System.Threading.Tasks;
using FluentAssertions;

namespace FastGenericNew.Tests.Units.ClrAllocatorTests;

public class ReferenceTypes
{
    [Test]
    public void Object()
    {
        var expected = Activator.CreateInstance<object>();
        var actual = ClrAllocator<object>.CreateInstance();

        actual.GetType().Should().Be(expected.GetType(), "the actual object should have the same type as the expected object.");
    }

    [TestCaseSourceGeneric(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void CommonTypes<T>()
    {
        var expected = Activator.CreateInstance<T>();
        var actual = ClrAllocator<T>.CreateInstance();

        actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected object of type {0}.", typeof(T).Name);
    }

    [TestCaseSourceGeneric(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void ParallelNew<T>()
    {
        const int count = 512;
        T[] array = new T[count];

        Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = count }, i =>
        {
            array[i] = ClrAllocator<T>.CreateInstance();
        });

        var expected = Activator.CreateInstance<T>();
        foreach (var item in array)
        {
            item.Should().BeEquivalentTo(expected, "each item in the array should be equivalent to the expected object of type {0}.", typeof(T).Name);
        }
    }
}
#endif