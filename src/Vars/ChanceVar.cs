using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Vars;

/// <summary>Dynamic variable representing chance in percentage.</summary>
public sealed class ChanceVar(decimal between0and100) : DynamicVar(KEY, between0and100)
{
    public const string KEY = "Chance";
}
