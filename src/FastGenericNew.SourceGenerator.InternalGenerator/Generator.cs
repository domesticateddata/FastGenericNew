using FastGenericNew.SourceGenerator.InternalGenerator.Gen;

namespace FastGenericNew.SourceGenerator.InternalGenerator;

[Generator(LanguageNames.CSharp)]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        GeneratorOptionAttributeGenerator.Register(in context);
        GeneratorOptionsRelatedGenerator.Register(in context);
        RoslynPropsFileGenerator.Register(in context);
    }
}
