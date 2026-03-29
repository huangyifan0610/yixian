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

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - White Crane Bright Wings.</summary>
public sealed class YxWhiteCraneBrightWings() : YxCardModel(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal random damage; gain energy if hexagram is consumed.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar("MinDamage", 1m, ValueProp.Move),
        new DamageVar("MaxDamage", 6m, ValueProp.Move),
        new EnergyVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have enough hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars["MaxDamage"].UpgradeValueBy(3);

    /// <summary>Deal random damage; gain energy if hexagram is consumed.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));

        Main.LOGGER.Info("=========================== " + Owner.Creature.Player?.Character);
        Main.LOGGER.Info("=========================== " + Owner.Creature.Player?.Character?.Id.Entry);
        Main.LOGGER.Info("=========================== " + ModelDb.Character<YxHeptastarPavilion>());
        Main.LOGGER.Info("=========================== " + ModelDb.Character<YxHeptastarPavilion>().Id.Entry);
        Main.LOGGER.Info("=========================== " + (Owner.Creature.Player?.Character == ModelDb.Character<YxHeptastarPavilion>()));

        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();
        await DamageCmd
            .Attack(hexagram.Range(RunState, DynamicVars["MinDamage"].IntValue, DynamicVars["MaxDamage"].IntValue, out bool used))
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (used)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}
