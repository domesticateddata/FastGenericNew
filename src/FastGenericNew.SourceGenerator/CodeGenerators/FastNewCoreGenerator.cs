namespace FastGenericNew.SourceGenerator.CodeGenerators;

public class FastNewCoreGenerator : CodeGenerator<FastNewCoreGenerator>
{
    public override string Filename => "FastNew{T}.g.cs";

    internal const string ClassName = "FastNew";
    internal const string IsValidName = "IsValid";
    internal const string CompiledDelegateName = "CompiledDelegate";
    internal const string ConstructorName = "CachedConstructor";

    public override CodeGenerationResult Generate(in GeneratorOptions options)
    {
        CodeBuilder builder = new(65536, in options);
        builder.WriteFileHeader();
        builder.StartNamespace();
        builder.Indent(1);
        builder.AppendAccessibility(options.PublicFastNewCore);

        #region Get CompiledDelegateName Type
        string compiledDelegateTypeNoParam;
        {
            CodeBuilder internalBuilder = new(32, in options);
            internalBuilder.UseGenericDelegate(0);
            compiledDelegateTypeNoParam = internalBuilder.ToString();
        }
        #endregion

        #region IL Generation
        builder.AppendLine(@$"static partial class {ClassName}<T>
{{
    /// <summary>
    /// The constructor of <typeparamref name=""T"" /> with given arguments. <br/>
    /// Could be <see langword=""null"" /> if the constructor couldn't be found.
    /// </summary>
    public static readonly ConstructorInfo? {ConstructorName} = typeof(T).GetConstructor(
        {GetBindingFlags(options)}, null, Type.EmptyTypes, null);

    public static readonly {compiledDelegateTypeNoParam} {CompiledDelegateName};

    public static readonly bool {IsValidName} = typeof(T).IsValueType || 
        ({ConstructorName} != null && !typeof(T).IsAbstract);

    static {ClassName}()
    {{
        var dm = new DynamicMethod("""", typeof(T), new Type[] {{ typeof(object) }}, restrictedSkipVisibility: true);
        var il = dm.GetILGenerator();

        if ({IsValidName})
        {{
            if ({ConstructorName} != null)
                il.Emit(OpCodes.Newobj, {ConstructorName}!);
            else
            {{
                il.DeclareLocal(typeof(T));
                il.Emit(OpCodes.Ldloc_0);
            }}
        }}
        else
        {{
            il.Emit(OpCodes.Call, typeof(ThrowHelper).GetMethod(nameof(ThrowHelper.GetSmartThrow))!.MakeGenericMethod(typeof(T)));
        }}
        il.Emit(OpCodes.Ret);
        {CompiledDelegateName} = ({compiledDelegateTypeNoParam})dm.CreateDelegate(typeof({compiledDelegateTypeNoParam}));
    }}
}}");
        #endregion

        for (int parameterIndex = 1; parameterIndex <= options.MaxParameterCount; parameterIndex++)
        {
            builder.Indent(1);
            builder.AppendAccessibility(options.PublicFastNewCore);
            builder.AppendKeyword("static partial class");

            builder.Append(ClassName);
            builder.DeclareGenericMember(parameterIndex);
            builder.AppendLine();

            builder.StartBlock(1);

            #region CachedConstructor
            builder.XmlDoc(2, @"
/// <summary>
/// The constructor of <typeparamref name=""T"" /> with given arguments. <br/>
/// Could be <see langword=""null"" /> if the constructor couldn't be found.
/// </summary>");

            builder.Indent(2);
            builder.Append($"public static readonly ConstructorInfo? {ConstructorName} = typeof(T).GetConstructor(");
            builder.Append(GetBindingFlags(options));
            builder.Append(", null, ");
            if (parameterIndex == 0)
            {
                builder.Append("Type.EmptyTypes");
            }
            else
            {
                builder.Append("new Type[]");
                builder.AppendLine();
                builder.StartBlock(2);

                for (int i = 0; i < parameterIndex; i++)
                {
                    builder.Indent(3);
                    builder.Append("typeof(");
                    builder.AppendGenericArgumentName(i);
                    builder.Append(')', ',');
                    builder.AppendLine();
                }

                builder.EndBlock(2, false);
            }
            builder.AppendLine(", null);");
            #endregion

            builder.Indent(2);
            builder.Append("public static readonly ");
            builder.UseGenericDelegate(parameterIndex);
            builder.AppendLine($" {CompiledDelegateName};");
            builder.PrettyNewLine();

            builder.Indent(2);
            builder.AppendLine($"public static readonly bool {IsValidName};");
            builder.PrettyNewLine();

            #region Constructor
            builder.Indent(2);
            builder.AppendLine($"static {ClassName}()");
            builder.StartBlock(2);

            #region IsValid
            builder.AppendLine(3, $"IsValid = {ConstructorName} != null && !typeof(T).IsAbstract;");
            #endregion

            #region IL

            #region Parameters
            for (int i = 0; i < parameterIndex; i++)
            {
                builder.Indent(3);
                builder.Append("var ");
                builder.AppendGenericMethodArgumentName(i);
                builder.Append(" = System.Linq.Expressions.Expression.Parameter(typeof(");
                builder.AppendGenericArgumentName(i);
                builder.Append("));\n");
            }
            #endregion

            builder.Indent(3);
            builder.Append($"{CompiledDelegateName} = System.Linq.Expressions.Expression.Lambda<");
            builder.UseGenericDelegate(parameterIndex);
            builder.AppendLine($">({IsValidName}");

            builder.Indent(4);
            builder.Append($"? (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.New({ConstructorName}!");
            for (int i = 0; i < parameterIndex; i++)
            {
                builder.Append(',', ' ');
                builder.AppendGenericMethodArgumentName(i);
            }
            builder.AppendLine(')');

            builder.Indent(4);
            builder.Append(": (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.Call(");
            builder.GlobalNamespaceDot();
            builder.AppendLine($"{ThrowHelperGenerator.ClassName}.GetSmartThrow<T>())");

            builder.Indent(3);
            builder.Append(", new System.Linq.Expressions.ParameterExpression[] {{ ");
            for (int i = 0; i < parameterIndex; i++)
            {
                if (i != 0)
                {
                    builder.Append(',', ' ');
                }
                builder.AppendGenericMethodArgumentName(i);
            }
            builder.AppendLine(" }}).Compile();");
            #endregion

            builder.EndBlock(2);
            #endregion

            builder.EndBlock(1);
        }

        builder.EndNamespace();

        return builder.BuildAndDispose(this);
    }

    private static string GetBindingFlags(in GeneratorOptions options)
    {
        return options.NonPublicConstructorSupport
            ? "BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic"
            : "BindingFlags.Instance | BindingFlags.Public";
    }

    public override bool ShouldUpdate(in GeneratorOptions oldValue, in GeneratorOptions newValue) =>
        base.ShouldUpdate(oldValue, newValue)
        || oldValue.ForceFastNewDelegate != newValue.ForceFastNewDelegate;
}
