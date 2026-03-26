using System;
using System.Collections;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Yixian.Patches;

namespace Yixian.Powers;

/// <summary>Star Point Power.</summary>
/// <remarks>A power indicating star point slots in hand.</remarks>
public sealed class YxStarPointPower : PowerModel
{
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
    public static BitArray DEFAULT_BIT_ARRAY => _defaultBitArray ??= DefaultBitArray();
    private static BitArray? _defaultBitArray = null;
    private static BitArray DefaultBitArray()
    {
        var data = new BitArray(CardPile.maxCardsInHand, false);
        data[2] = true;
        data[5] = true;
        return data;
    }

    /// <summary>The third and sixth slots in hand are the default star points.</summary>
    protected override object? InitInternalData() => new BitArray(DEFAULT_BIT_ARRAY);

    /// <summary>Checkes that the <paramref name="target"/> creature is player.</summary>
    public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!target.IsPlayer)
        {
            throw new ArgumentException("Cannot apply 'Star Point' power on non-player creatures.", nameof(target));
        }

        return base.BeforeApplied(target, amount, applier, cardSource);
    }

    /// <summary>Retruns true if the <paramref name="cardModel"/> is on star point.</summary>
    public static bool Test(CardModel cardModel)
    {
        int index = cardModel.IndexInHand();
        if (0 <= index && index < CardPile.maxCardsInHand)
        {
            var power = cardModel.Owner?.Creature?.GetPower<YxStarPointPower>();
            return power?[index] ?? DEFAULT_BIT_ARRAY[index];
        }
        return false;
    }
}
