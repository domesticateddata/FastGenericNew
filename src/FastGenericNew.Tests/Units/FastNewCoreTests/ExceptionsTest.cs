using FluentAssertions;

namespace FastGenericNew.Tests.Units.FastNewCoreTests;

public class ExceptionsTest
{
    [Test]
    public void ExceptionInterface()
    {
        // Act & Assert
        Action action = () => FastNew<IEnumerable>.CompiledDelegate();
        action.Should().Throw<MissingMethodException>("because creating an instance of an interface is not allowed")
            .WithMessage("Cannot create an instance of an interface*");
    }

    [Test]
    public void ExceptionAbstract()
    {
        // Act & Assert
        Action action = () => FastNew<Stream>.CompiledDelegate();
        action.Should().Throw<MissingMethodException>("because creating an instance of an abstract class is not allowed")
            .WithMessage("Cannot create an abstract class*");
    }

    [Test]
    public void ExceptionPlString()
    {
        // Act & Assert
        Action action = () => FastNew<string>.CompiledDelegate();
        action.Should().Throw<MissingMethodException>("because no matching constructor was found")
            .WithMessage("No match constructor found in type*");
    }

    [Test]
    public void ExceptionNotFoundNoParameter()
    {
        // Act & Assert
        Action action = () => FastNew<DemoClassNoParamlessCtor>.CompiledDelegate();
        action.Should().Throw<MissingMethodException>("because no parameterless constructor was found")
            .WithMessage("No match constructor found in type*");
    }

    [Test]
    public void ExceptionNotFoundWithParameter()
    {
        // Act & Assert
        Action action = () => FastNew<DemoClass, DBNull>.CompiledDelegate(DBNull.Value);
        action.Should().Throw<MissingMethodException>("because no matching constructor with parameters was found")
            .WithMessage("No match constructor found in type*");
    }
}
