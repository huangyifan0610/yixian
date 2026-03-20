using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Vars;

/// <summary>
/// The lower bound of the random damage range. 
/// </summary>
public class MinDamageVar(decimal amount, ValueProp props): DamageVar(DEFAULT_NAME, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT_NAME = "MinDamage";
}


/// <summary>
/// The upper bound of the random damage range. 
/// </summary>
public class MaxDamageVar(decimal amount, ValueProp props): DamageVar(DEFAULT_NAME, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT_NAME = "MaxDamage";
}
