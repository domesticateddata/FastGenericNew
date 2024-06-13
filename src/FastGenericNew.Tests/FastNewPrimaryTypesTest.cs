using FluentAssertions;

namespace FastGenericNew.Tests;


public class FastNewTests
{
    [SetUp]
    public void Setup()
    {
        // Setup code if needed
    }

    #region CreateInstance Class

    [Test]
    public void CreateInstanceObject()
    {
        var expected = new object();
        var actual = FastNew.CreateInstance<object>();

        actual.GetType().Should().Be(expected.GetType(), "the actual object should have the same type as the expected object.");
    }

    [Test]
    public void CreateInstanceClass()
    {
        var expected = new DemoClass();
        var actual = FastNew.CreateInstance<DemoClass>();

        actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected DemoClass instance.");
    }

    [Test]
    public void CreateInstanceClassParameter()
    {
        const int val = 99999;
        var expected = new DemoClass(val);
        var actual = FastNew.CreateInstance<DemoClass, int>(val);

        actual.Should().BeEquivalentTo(expected, "the actual object should be equivalent to the expected DemoClass instance with the given parameter.");
    }

    [Test]
    public void CreateInstanceClassPrivateCtor()
    {
        var expected = DemoClassPrivateCtor.Create();
        var actual = FastNew.CreateInstance<DemoClassPrivateCtor>();

        actual.Should().BeEquivalentTo(expected, "the actual object created by private constructor should be equivalent to the expected DemoClassPrivateCtor instance.");
    }

    [Test]
    public void CreateInstanceClassPrivateParameterCtor()
    {
        const int val = 99999;
        var expected = DemoClassPrivateCtor.Create(val);
        var actual = FastNew.CreateInstance<DemoClassPrivateCtor, int>(val);

        actual.Should().BeEquivalentTo(expected, "the actual object created by private constructor with parameters should be equivalent to the expected DemoClassPrivateCtor instance.");
    }

    #endregion

    #region Exceptions should happen

    [Test]
    public void ExceptionFastNewInterface()
    {
        Action action = () => FastNew.CreateInstance<IEnumerable>();
        action.Should().Throw<MissingMethodException>()
              .WithMessage("Cannot create an instance of an interface*");
    }

    [Test]
    public void ExceptionFastNewAbstract()
    {
        Action action = () => FastNew.CreateInstance<Stream>();
        action.Should().Throw<MissingMethodException>()
              .WithMessage("Cannot create an abstract class*");
    }

    [Test]
    public void ExceptionFastNewParameterless()
    {
        Action action = () => FastNew.CreateInstance<string>();
        action.Should().Throw<MissingMethodException>()
              .WithMessage("No match constructor found in type*");
    }

    [Test]
    public void ExceptionFastNewWithParameter()
    {
        Action action = () => FastNew.CreateInstance<string, DBNull>(DBNull.Value);
        action.Should().Throw<MissingMethodException>()
              .WithMessage("No match constructor found in type*");
    }

    #endregion

    #region ValueTypes

    [Test(Description = "Check if FastNew works on primary types")]
    [TestCaseSource(nameof(GetValueTypes))]
    [Parallelizable(ParallelScope.All)]
    public void ValueTypeDefault<T>(T defaultVal)
    {
        typeof(T).IsValueType.Should().BeTrue("T must be a value type");

        var defaultValExpected = default(T);
        defaultValExpected.Should().Be(defaultVal);
        var fastNewInst = FastNew.NewOrDefault<T>();
        defaultVal.Should().Be(fastNewInst);
    }

    [Test(Description = "Check if FastNew works on primary types")]
    [TestCaseSource(nameof(GetValueTypes))]
    [Parallelizable(ParallelScope.All)]
    public void ValueTypeCreateInstance<T>(T defaultVal)
    {
        typeof(T).IsValueType.Should().BeTrue("T must be a value type");

        var expected = Activator.CreateInstance<T>();
        var fastNewInst = FastNew.CreateInstance<T>();
        fastNewInst.Should().Be(expected);
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
        yield return default(DemoStructParameterless);
    }

    #endregion

    #region ParameterlessStructCtor

    [Test(Description = "Check if FastNew supports Parameterless Struct Constructor feature added in C# 10")]
    public void ParameterlessStructCtor()
    {
        var expected = new DemoStructParameterless(); // This will call the parameterless constructor
        var shouldEqual = FastNew.CreateInstance<DemoStructParameterless>(); // This should call the parameterless constructor
        var shouldntEqual = FastNew.NewOrDefault<DemoStructParameterless>(); // This shouldn't call the constructor

        shouldEqual.Should().Be(expected);
        shouldntEqual.Should().NotBe(expected, "FastNew.NewOrDefault should not call the parameterless constructor.");
    }

    #endregion
}
