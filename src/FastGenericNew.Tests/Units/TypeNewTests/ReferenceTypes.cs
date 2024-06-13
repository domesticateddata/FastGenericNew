using System.Threading.Tasks;
using FluentAssertions;

namespace FastGenericNew.Tests.Units.TypeNewTests;


public class ReferenceTypes
{
    [Test]
    public void Object()
    {
        var expected = Activator.CreateInstance<object>();
        var actual = FastNew.GetCreateInstance<object>(typeof(object)).Invoke();

        actual.GetType().Should().Be(expected.GetType(), "the actual object's type should match the expected object's type");
    }

    [Test]
    public void WithParameters1()
    {
        const int val = 99999;
        var expected = new DemoClass(val);
        var actual = FastNew.GetCreateInstance<object, int>(typeof(DemoClass), typeof(int)).Invoke(val);

        actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected object with the given parameter");
    }

    [Test]
    public void WithParameters2()
    {
        const int val = 99999;
        const int val2 = 99999;
        var expected = new DemoClass(val, val2);
        var actual = FastNew.GetCreateInstance<DemoClass, int, int>(typeof(DemoClass), typeof(int), typeof(int)).Invoke(val, val2);

        actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected object with the given parameters");
    }

    [Test]
    public void WithParametersMany()
    {
            const int val = 11111;
            var expected = new DemoClass(
                val, val, val, val, val,
                val, val, val, val, val,
                val, val, val, val, val,
                val, val, val
                );
            var actual = FastNew.GetCreateInstance<DemoClass, 
                int, int, int, int, int,
                int, int, int, int, int,
                int, int, int, int, int,
                int, int, int>(
                typeof(DemoClass),
                typeof(int), typeof(int), typeof(int), typeof(int), typeof(int),
                typeof(int), typeof(int), typeof(int), typeof(int), typeof(int),
                typeof(int), typeof(int), typeof(int), typeof(int), typeof(int),
                typeof(int), typeof(int), typeof(int)
            ).Invoke(
                val, val, val, val, val,
                val, val, val, val, val, 
                val, val, val, val, val, 
                val, val, val);

            actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected object with the given many parameters");
        }

        [Test]
    public void PrivateCtor()
    {
        var expected = DemoClassPrivateCtor.Create();
        var actual = FastNew.GetCreateInstance<object>(typeof(DemoClassPrivateCtor)).Invoke();

        actual.Should().BeEquivalentTo(expected, "the actual object created by private constructor should be equivalent to the expected object");
    }

    [Test]
    public void PrivateCtorWithParameter()
    {
        const int val = 99999;
        var expected = DemoClassPrivateCtor.Create(val);
        var actual = FastNew.GetCreateInstance<object, int>(typeof(DemoClassPrivateCtor), typeof(int)).Invoke(val);

        actual.Should().BeEquivalentTo(expected, "the actual object created by private constructor with parameters should be equivalent to the expected object");
    }

    [TestCaseSource(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void CommonTypes(Type type)
    {
        var expected = Activator.CreateInstance(type);
        var actual = FastNew.GetCreateInstance<object>(type).Invoke();

        actual.Should().BeEquivalentTo(expected, "the actual object created for the given type should be equivalent to the expected object");
    }

    [TestCaseSource(typeof(TestData), nameof(TestData.CommonReferenceTypesPl))]
    [Parallelizable(ParallelScope.All)]
    public void ParallelNew(Type type)
    {
        const int count = 512;
        object[] array = new object[count];
        Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = count }, i =>
        {
            array[i] = FastNew.GetCreateInstance<object>(type).Invoke();
        });

        var expected = Activator.CreateInstance(type);
        foreach (var item in array)
        {
            item.Should().BeEquivalentTo(expected, "each item in the array should be equivalent to the expected object for the given type");
        }
    }
}
