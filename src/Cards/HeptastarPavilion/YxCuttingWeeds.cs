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

/// <summary>Heptastar Pavilion - Cutting Weeds.</summary>
public sealed class YxCuttingWeeds() : YxCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal damage; Have chance to exhaust status cards in draw pile.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9, ValueProp.Move),
        new ChanceVar(30),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Upgrade the chance to exhaust status cards.</summary>
    protected override void OnUpgrade() => DynamicVars[ChanceVar.KEY].UpgradeValueBy(20);

    /// <summary>Deal damage; Exhaust status cards in draw pile.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        ArgumentNullException.ThrowIfNull(Owner.PlayerCombatState, nameof(Owner.PlayerCombatState));
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();
        if (hexagram.Test(RunState, DynamicVars[ChanceVar.KEY].BaseValue, out bool _))
        {
            foreach (var card in Owner.PlayerCombatState.DrawPile.Cards)
            {
                if (card.Type == CardType.Status)
                {
                    await CardCmd.Exhaust(choiceContext, card);
                }
            }
        }
    }
}
