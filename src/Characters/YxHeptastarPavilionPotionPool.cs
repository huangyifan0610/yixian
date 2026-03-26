using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace Yixian.Characters;

/// <summary>Yixian Heptastar Pavilion Potion Pool Model.</summary>
public sealed class YxHeptastarPavilionPotionPool : PotionPoolModel
{
    public override string EnergyColorName => YxHeptastarPavilion.ENERGY_COLOR_NAME;
    public override Color LabOutlineColor => YxHeptastarPavilion.LAB_OUTLINE_COLOR;
    protected override IEnumerable<PotionModel> GenerateAllPotions() => [];
    public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState) => AllPotions;
}
