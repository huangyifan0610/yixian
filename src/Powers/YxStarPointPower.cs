using System;
using System.Collections;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Star Point Power.</summary>
/// <remarks>A power indicating star point slots in hand.</remarks>
public sealed class YxStarPointPower : PowerModel
{
    /// <summary>A default value</summary>
    public static readonly YxStarPointPower DEFAULT = ModelDb.Power<YxStarPointPower>();

    /// <summary>Neither buff or debuff.</summary>
    public override PowerType Type => PowerType.None;

    /// <summary>Unique instance.</summary>
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// Returns true if the <paramref name="index"/>-th slot in hand is star point.
    /// Or sets whether the <paramref name="index"/>-th slot in hand is star point.
    /// </summary>
    public bool this[int index]
    {
        get => GetInternalData<BitArray>().Get(index);
        set => GetInternalData<BitArray>().Set(index, value);
    }

    /// <summary>The third and sixth slots in hand are the default star points.</summary>
    protected override object? InitInternalData()
    {
        var data = new BitArray(CardPile.maxCardsInHand, false);
        data[2] = true;
        data[5] = true;
        return data;
    }

    /// <summary>Checkes that the <paramref name="target"/> creature is player.</summary>
    public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!target.IsPlayer)
        {
            throw new ArgumentException("Cannot apply 'Star Point' power on non-player creatures.", nameof(target));
        }

        return base.BeforeApplied(target, amount, applier, cardSource);
    }
}
