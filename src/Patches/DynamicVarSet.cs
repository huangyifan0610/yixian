using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Patches;

/// <summary>
/// An extensition for <see cref="DynamicVarSet"/> that provides additional variables
/// shared by the Mod.
/// </summary>
public static class DynamicVarSetExtension
{
    /// <summary>
    /// Returns the default Min Damage variable.
    /// </summary>
    public static MinDamageVar MinDamage(this DynamicVarSet vars) => (MinDamageVar)vars[MinDamageVar.DEFAULT];

    /// <summary>
    /// Returns the default Max Damage variable.
    /// </summary>
    public static MaxDamageVar MaxDamage(this DynamicVarSet vars) => (MaxDamageVar)vars[MaxDamageVar.DEFAULT];

    /// <summary>
    /// Returns the default Min Block variable.
    /// </summary>
    public static MinBlockVar MinBlock(this DynamicVarSet vars) => (MinBlockVar)vars[MinBlockVar.DEFAULT];

    /// <summary>
    /// Returns the default Max Block variable.
    /// </summary>
    public static MaxBlockVar MaxBlock(this DynamicVarSet vars) => (MaxBlockVar)vars[MaxBlockVar.DEFAULT];

    /// <summary>
    /// Returns the default Star Power variable.
    /// </summary>
    public static PowerVar<T> Power<T>(this DynamicVarSet vars) where T : PowerModel => (PowerVar<T>)vars[nameof(T)];

    /// <summary>
    /// Returns the default Star Power Bonus variable.
    /// </summary>
    public static StarPowerBonusVar StarPowerBonus(this DynamicVarSet vars) => (StarPowerBonusVar)vars[StarPowerBonusVar.DEFAULT];

    /// <summary>
    /// Attempts to get the default Star Power Bonus variable.
    /// </summary>
    public static bool TryStarPowerBonus(this DynamicVarSet vars, [MaybeNullWhen(false)] out StarPowerBonusVar starPowerBonus)
    {
        if (vars.TryGetValue(StarPowerBonusVar.DEFAULT, out var variable) && variable is StarPowerBonusVar bonus)
        {
            starPowerBonus = bonus;
            return true;
        }
        else
        {
            starPowerBonus = null;
            return false;
        }
    }
}

/// <summary>
/// The lower bound of the random damage range. 
/// </summary>
public class MinBlockVar(decimal amount, ValueProp props) : BlockVar(DEFAULT, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "MinBlock";
}

/// <summary>
/// The upper bound of the random damage range. 
/// </summary>
public class MaxBlockVar(decimal amount, ValueProp props) : BlockVar(DEFAULT, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "MaxBlock";
}

/// <summary>
/// The lower bound of the random damage range. 
/// </summary>
public class MinDamageVar(decimal amount, ValueProp props) : DamageVar(DEFAULT, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "MinDamage";
}

/// <summary>
/// The upper bound of the random damage range. 
/// </summary>
public class MaxDamageVar(decimal amount, ValueProp props) : DamageVar(DEFAULT, amount, props)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "MaxDamage";
}

/// <summary>
/// Bonus from star power.
/// </summary>
public class StarPowerBonusVar(decimal amount) : DynamicVar(DEFAULT, amount)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "StarPowerBonus";
}
