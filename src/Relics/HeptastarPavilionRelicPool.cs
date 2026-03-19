using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace Yixian.Relics;

/// <summary>
/// Relics of Heptastar PavilionRelic.
/// </summary>
public sealed class HeptastarPavilionRelicPool: RelicPoolModel
{
    /// <summary>
    /// Asset "res://images/atlases/ui_atlas.sprites/card/energy_qi.tres".
    /// </summary>
    public override string EnergyColorName => "qi";

    public override Color LabOutlineColor => StsColors.halfTransparentBlack;

    /// <summary>
    /// Returns relics of Heptastar Pavilion.
    /// </summary>
    protected override IEnumerable<RelicModel> GenerateAllRelics()
    {
        return [
            ModelDb.Relic<StarPoint>(),
        ]; 
    }

    /// <summary>
    /// Returns unlocked relics.
    /// </summary>
    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
    {
        return AllRelics;
    }
}