using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Dance Of The Dragonfly.</summary>
public sealed class YxDanceOfTheDragonfly() : YxCardModel(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal damage; Have chance to draw cards.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move),
        new ChanceVar(15),
        new CardsVar(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(4);

    /// <summary>Deal damage; Have chance to draw cards.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (Owner.Creature.GetPower<YxHexagramPower>().Test(RunState, DynamicVars["Chance"].BaseValue, out bool _))
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}
