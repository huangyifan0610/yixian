using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Vars;

/// <summary>
/// An extensition for <see cref="DynamicVarSet"/> that provides additional variables
/// shared by the Mod.
/// </summary>
public static class DynamicVarSetExtension
{
    /// <summary>
    /// Returns the default Min Damage variable.
    /// </summary>
    public static MinDamageVar MinDamage(this DynamicVarSet vars)
    {
        return (MinDamageVar)vars[MinDamageVar.DEFAULT_NAME];
    }

    /// <summary>
    /// Returns the default Max Damage variable.
    /// </summary>
    public static MaxDamageVar MaxDamage(this DynamicVarSet vars)
    {
        return (MaxDamageVar)vars[MaxDamageVar.DEFAULT_NAME];
    }

    /// <summary>
    /// Returns the default Min Block variable.
    /// </summary>
    public static MinBlockVar MinBlock(this DynamicVarSet vars)
    {
        return (MinBlockVar)vars[MinBlockVar.DEFAULT_NAME];
    }

    /// <summary>
    /// Returns the default Max Block variable.
    /// </summary>
    public static MaxBlockVar MaxBlock(this DynamicVarSet vars)
    {
        return (MaxBlockVar)vars[MaxBlockVar.DEFAULT_NAME];
    }

    /// <summary>
    /// Returns the default Hexagram variable.
    /// </summary>
    public static HexagramVar Hexagram(this DynamicVarSet vars)
    {
        return (HexagramVar )vars[HexagramVar.DEFAULT_NAME];
    }

    /// <summary>
    /// Returns the default Star Power variable.
    /// </summary>
    public static StarPowerVar StarPower(this DynamicVarSet vars)
    {
        return (StarPowerVar)vars[StarPowerVar.DEFAULT_NAME];
    }
}