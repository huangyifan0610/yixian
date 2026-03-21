
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Vars;

/// <summary>
/// Variable indicating the amount of Star Power.
/// </summary>
public class StarPowerVar(string name, decimal amount) : DynamicVar(name, amount)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "StarPower";

    /// <summary>
    /// Construct with default variable name.
    /// </summary>
    public StarPowerVar(decimal amount) : this(DEFAULT, amount) { }

    /// <summary>
    /// Construct with default variable name.
    /// </summary>
    public StarPowerVar(int amount) : this(DEFAULT, amount) { }
}

/// <summary>
/// Variable indicating the bounds from Star Power.
/// </summary>
public class StarPowerBonusVar(decimal amount) : DynamicVar(DEFAULT, amount)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "StarPowerBonus";
}
