using RaythaZero.Application.Common.Interfaces;

namespace RaythaZero.Application.Common.Models.RenderModels;

public record EmptyTarget_RenderModel : IInsertTemplateVariable
{
    public virtual IEnumerable<string> GetDeveloperNames()
    {
        return new string[0];
    }

    public virtual IEnumerable<KeyValuePair<string, string>> GetTemplateVariables()
    {
        return new KeyValuePair<string, string>[] { };
    }

    public virtual string GetTemplateVariablesAsForEachLiquidSyntax()
    {
        return string.Empty;
    }
}