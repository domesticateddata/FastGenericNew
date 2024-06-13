namespace FastGenericNew.SourceGenerator.CodeGenerators;

public class FastTryCreateInstanceGenerator : CodeGenerator<FastTryCreateInstanceGenerator>
{
    public override string Filename => "FastNew.TryCreateInstance.g.cs";

    internal const string ClassName = "FastNew";

    public override CodeGenerationResult Generate(in GeneratorOptions options)
    {
        if (!options.GenerateTryCreateInstance) return CodeGenerationResult.Empty;

        CodeBuilder builder = new(20480, in options);
        builder.WriteFileHeader();
        builder.StartNamespace();
        builder.Indent(1);
        builder.AppendKeyword("public static partial class");
        builder.Append(ClassName);

        builder.StartBlock(1);

        // Generate TryCreateInstance for default T
        builder.AppendLine($@"
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryCreateInstance<T>(out T result)
        {{
            if ({options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}<T>.{FastNewCoreGenerator.IsValidName})
            {{
                result = typeof(T).IsValueType
                    ? System.Activator.CreateInstance<T>()
                    : {options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}<T>.{FastNewCoreGenerator.CompiledDelegateName}();
                return true;
            }}
            result = default!;
            return false;
        }}

        // Generate TryNewOrDefault for default T
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryNewOrDefault<T>(out T result)
        {{
            if ({options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}<T>.{FastNewCoreGenerator.IsValidName})
            {{
                result = typeof(T).IsValueType
                    ? default(T)!
                    : {options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}<T>.{FastNewCoreGenerator.CompiledDelegateName}();
                return true;
            }}
            result = default!;
            return false;
        }}");

        // Generate TryCreateInstance with parameters
        for (int parameterIndex = 1; parameterIndex <= options.MaxParameterCount; parameterIndex++)
        {
            builder.Indent(2);
            builder.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
            builder.Indent(2);
            builder.Append("public static bool TryCreateInstance");
            builder.DeclareGenericMember(parameterIndex);
            builder.Append('(');

            for (int i = 0; i < parameterIndex; i++)
            {
                if (i > 0) builder.Append(", ");
                builder.AppendGenericArgumentName(i);
                builder.Append(' ');
                builder.AppendGenericMethodArgumentName(i);
            }
            builder.AppendLine(", out T result)");
            builder.StartBlock(2);

            builder.Indent(3);
            builder.Append($"if ({options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}");
            builder.UseGenericMember(parameterIndex);
            builder.AppendLine($".{FastNewCoreGenerator.IsValidName})");

            builder.StartBlock(3);
            builder.Indent(4);
            builder.Append($"result = {options.GlobalNsDot()}{FastNewCoreGenerator.ClassName}");
            builder.UseGenericMember(parameterIndex);
            builder.Append($".{FastNewCoreGenerator.CompiledDelegateName}(");

            for (int i = 0; i < parameterIndex; i++)
            {
                if (i > 0) builder.Append(", ");
                builder.AppendGenericMethodArgumentName(i);
            }
            builder.Append(");");
            builder.AppendLine();
            builder.Indent(4);
            builder.AppendLine("return true;");
            builder.EndBlock(3);

            builder.Indent(3);
            builder.AppendLine("result = default!;");
            builder.Indent(3);
            builder.AppendLine("return false;");
            builder.EndBlock(2);
        }

        builder.EndBlock(1);
        builder.EndNamespace();

        return builder.BuildAndDispose(this);
    }

    public override bool ShouldUpdate(in GeneratorOptions oldValue, in GeneratorOptions newValue)
    {
        return base.ShouldUpdate(oldValue, newValue) && newValue.GenerateTryCreateInstance;
    }
}
