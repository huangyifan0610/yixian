using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - White Snake.</summary>
public sealed class YxWhiteSnake() : YxCardModel(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal damage; Apply random vulnerable.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        new PowerVar<VulnerablePower>("MinVulnerablePower", 0),
        new PowerVar<VulnerablePower>("MaxVulnerablePower", 3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);

    /// <summary>Deal damage; Apply random vulnerable.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();
        var vulnerable = hexagram.Range(
            RunState,
            DynamicVars["MinVulnerablePower"].IntValue,
            DynamicVars["MaxVulnerablePower"].IntValue,
            out bool _
        );
        if (vulnerable > 0)
        {
            await PowerCmd.Apply<VulnerablePower>(
                cardPlay.Target,
                vulnerable,
                Owner.Creature,
                this
            );
        }
    }
}
