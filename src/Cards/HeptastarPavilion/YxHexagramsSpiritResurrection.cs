using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Hexagrams Spirit Resurrection.</summary>
public sealed class YxHexagramsSpiritResurrection() : YxCardModel(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>X energy.</summary>
    protected override bool HasEnergyCostX => true;

    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain temporary HP. Have chance to double the effect.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(5),
        new ChanceVar(30),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gain more temporary HP.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxTemporaryHpPower)].UpgradeValueBy(2);

    /// <summary>Gain temporary HP. Have chance to double the effect.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();
        int repeat = ResolveEnergyXValue();
        var success = hexagram.Test(RunState, DynamicVars[ChanceVar.KEY].BaseValue, repeat, out int _);

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (int i = 0; i < success; ++i)
        {
            await PowerCmd.Apply<YxTemporaryHpPower>(
                Owner.Creature,
                DynamicVars[nameof(YxTemporaryHpPower)].BaseValue * 2,
                Owner.Creature,
                this
            );
        }
        for (int i = success; i < repeat; ++i)
        {
            await PowerCmd.Apply<YxTemporaryHpPower>(
                Owner.Creature,
                DynamicVars[nameof(YxTemporaryHpPower)].BaseValue,
                Owner.Creature,
                this
            );
        }
    }
}
