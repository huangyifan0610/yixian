using System.Diagnostics.CodeAnalysis;
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
        return (MinDamageVar)vars[MinDamageVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Max Damage variable.
    /// </summary>
    public static MaxDamageVar MaxDamage(this DynamicVarSet vars)
    {
        return (MaxDamageVar)vars[MaxDamageVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Min Block variable.
    /// </summary>
    public static MinBlockVar MinBlock(this DynamicVarSet vars)
    {
        return (MinBlockVar)vars[MinBlockVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Max Block variable.
    /// </summary>
    public static MaxBlockVar MaxBlock(this DynamicVarSet vars)
    {
        return (MaxBlockVar)vars[MaxBlockVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Hexagram variable.
    /// </summary>
    public static HexagramVar Hexagram(this DynamicVarSet vars)
    {
        return (HexagramVar)vars[HexagramVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Star Power variable.
    /// </summary>
    public static StarPowerVar StarPower(this DynamicVarSet vars)
    {
        return (StarPowerVar)vars[StarPowerVar.DEFAULT];
    }

    /// <summary>
    /// Returns the default Star Power Bonus variable.
    /// </summary>
    public static StarPowerBonusVar StarPowerBonus(this DynamicVarSet vars)
    {
        return (StarPowerBonusVar)vars[StarPowerBonusVar.DEFAULT];
    }

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

    /// <summary>
    /// Returns the default Post Action variable.
    /// </summary>
    public static PostActionVar PostAction(this DynamicVarSet vars)
    {
        return (PostActionVar)vars[PostActionVar.DEFAULT];
    }

    /// <summary>
    /// Attempts to get the default Post Action variable.
    /// </summary>
    public static bool TryPostAction(this DynamicVarSet vars, [MaybeNullWhen(false)] out PostActionVar postAction)
    {
        if (vars.TryGetValue(PostActionVar.DEFAULT, out var variable) && variable is PostActionVar bonus)
        {
            postAction = bonus;
            return true;
        }
        else
        {
            postAction = null;
            return false;
        }
    }
}