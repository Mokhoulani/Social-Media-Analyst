using System.Reflection;
using Mapster;

namespace Application.Common.Mappings;


public sealed class ModelCodeGenRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        var as1 = Assembly.Load("Domain");
        config.AdaptTo("[name]Dto")
            .ForAllTypesInNamespace(as1, "Domain.Entities")
            .PreserveReference(true)
            .MaxDepth(1)
            .MapToConstructor(false);
    }
}
