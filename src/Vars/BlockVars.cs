using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Vars;

/// <summary>
/// The lower bound of the random damage range. 
/// </summary>
public class MinBlockVar(decimal amount, ValueProp props): BlockVar(DEFAULT_NAME, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT_NAME = "MinBlock";
}

/// <summary>
/// The upper bound of the random damage range. 
/// </summary>
public class MaxBlockVar(decimal amount, ValueProp props): BlockVar(DEFAULT_NAME, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT_NAME = "MaxBlock";
}
