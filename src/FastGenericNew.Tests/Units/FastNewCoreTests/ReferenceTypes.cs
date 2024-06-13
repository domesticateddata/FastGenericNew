using System.Threading.Tasks;
using FluentAssertions;

namespace FastGenericNew.Tests.Units.FastNewCoreTests;

public class ReferenceTypes
{
    [Test]
    public void Object()
    {
        var expected = Activator.CreateInstance<object>();
        var actual = FastNew<object>.CompiledDelegate();

        actual.Should().BeOfType(expected.GetType());
    }

    [Test]
    public void WithParameters1()
    {
        const int val = 99999;
        var expected = new DemoClass(val);
        var actual = FastNew<DemoClass, int>.CompiledDelegate(val);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void WithParameters2()
    {
        const int val = 99999;
        const int val2 = 99999;
        var expected = new DemoClass(val, val2);
        var actual = FastNew<DemoClass, int, int>.CompiledDelegate(val, val2);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void WithParametersMany()
    {
        const int val = 11111;
        var expected = new DemoClass(val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val);
        var actual = FastNew<DemoClass, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>.CompiledDelegate(val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val, val);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PrivateCtor()
    {
        var expected = DemoClassPrivateCtor.Create();
        var actual = FastNew.CreateInstance<DemoClassPrivateCtor>();

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PrivateCtorWithParameter()
    {
        const int val = 99999;
        var expected = DemoClassPrivateCtor.Create(val);
        var actual = FastNew.CreateInstance<DemoClassPrivateCtor, int>(val);

        actual.Should().BeEquivalentTo(expected);
    }

    [TestCaseSourceGeneric(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void CommonTypes<T>()
    {
        var expected = Activator.CreateInstance<T>();
        var actual = FastNew<T>.CompiledDelegate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestCaseSourceGeneric(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void ParallelNew<T>()
    {
        const int count = 512;
        T[] array = new T[count];
        Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = count }, i =>
        {
            array[i] = FastNew<T>.CompiledDelegate();
        });

        var expected = Activator.CreateInstance<T>();
        array.Should().AllBeEquivalentTo(expected);
    }
}