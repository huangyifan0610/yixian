using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Unlocks;
using Yixian.Cards.HeptastarPavilion;

namespace Yixian.Characters;

/// <summary>Yixian Heptastar Pavilion Character ID.</summary>
public enum YxHeptastarPavilionCharacter
{
    /// <summary>Tan shuyan.</summary>
    TanShuyan,
    /// <summary>Yan Chen.</summary>
    YanChen,
    /// <summary>Yao Ling.</summary>
    YaoLing,
    /// <summary>Jiang Ximing.</summary>
    JiangXiming,
    /// <summary>Wu Ce.</summary>
    WuCe,
    /// <summary>Feng Xu.</summary>
    FengXu,
}

/// <summary>Yixian Heptastar Pavilion Character Model.</summary>
public sealed class YxHeptastarPavilion : CharacterModel
{
    // Constants of Heptastar Pavilion.
    public const string ENERGY_COLOR_NAME = "yx_qi";
    public static readonly Color ENERGY_OUTLINE_COLOR = new("3070C6");
    public static readonly Color DECK_ENTRY_CARD_COLOR = new("8030D6");
    public static readonly Color LAB_OUTLINE_COLOR = new("8030D6");

    /// <summary>Unlocked by default.</summary>
    protected override CharacterModel? UnlocksAfterRunAs => null;

    /// <summary>Returns the card pool.</summary>
    public override CardPoolModel CardPool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Returns the relic pool.</summary>
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<YxHeptastarPavilionRelicPool>();

    /// <summary>Returns the potion pool.</summary>
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<YxHeptastarPavilionPotionPool>();

    /// <summary>The real character ID.</summary>
    public YxHeptastarPavilionCharacter Character = default;

    /// <summary>Returns the character ID in uppercase.</summary>
    public string CharacterUppercase => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => "TAN_SHUYAN",
        YxHeptastarPavilionCharacter.YanChen => "YAN_CHEN",
        YxHeptastarPavilionCharacter.YaoLing => "YAO_LING",
        YxHeptastarPavilionCharacter.JiangXiming => "JIANG_XIMING",
        YxHeptastarPavilionCharacter.WuCe => "WU_CE",
        YxHeptastarPavilionCharacter.FengXu => "FENG_XU",
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the character ID in lowercase.</summary>
    public string CharacterLowercase => CharacterUppercase.ToLowerInvariant();

    /// <summary>Returns the character gender.</summary>
    public override CharacterGender Gender => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => CharacterGender.Feminine,
        YxHeptastarPavilionCharacter.YanChen => CharacterGender.Masculine,
        YxHeptastarPavilionCharacter.YaoLing => CharacterGender.Feminine,
        YxHeptastarPavilionCharacter.JiangXiming => CharacterGender.Masculine,
        YxHeptastarPavilionCharacter.WuCe => CharacterGender.Feminine,
        YxHeptastarPavilionCharacter.FengXu => CharacterGender.Feminine,
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the initial max HP.</summary>
    public override int StartingHp => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => 75,
        YxHeptastarPavilionCharacter.YanChen => 75,
        YxHeptastarPavilionCharacter.YaoLing => 70,
        YxHeptastarPavilionCharacter.JiangXiming => 75,
        YxHeptastarPavilionCharacter.WuCe => 75,
        YxHeptastarPavilionCharacter.FengXu => 75,
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the initial Gold.</summary>
    public override int StartingGold => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => 99,
        YxHeptastarPavilionCharacter.YanChen => 99,
        YxHeptastarPavilionCharacter.YaoLing => 199,
        YxHeptastarPavilionCharacter.JiangXiming => 99,
        YxHeptastarPavilionCharacter.WuCe => 99,
        YxHeptastarPavilionCharacter.FengXu => 99,
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the color of character name.</summary>
    public override Color NameColor => LAB_OUTLINE_COLOR;

    /// <summary>Returns the inital deck.</summary>
    public override IEnumerable<CardModel> StartingDeck => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        YxHeptastarPavilionCharacter.YanChen => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        YxHeptastarPavilionCharacter.YaoLing => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        YxHeptastarPavilionCharacter.JiangXiming => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        YxHeptastarPavilionCharacter.WuCe => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        YxHeptastarPavilionCharacter.FengXu => [ModelDb.Card<YxStrikeHeptastarPavilion>()],
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the initial relics.</summary>
    public override IReadOnlyList<RelicModel> StartingRelics => Character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.YanChen => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.YaoLing => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.JiangXiming => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.WuCe => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.FengXu => [ModelDb.Relic<BurningBlood>()],
        _ => throw new UnreachableException("unknown character"),
    };

    // TODO: Overrides abstract methods.
    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 0.25f;
    public override Color EnergyLabelOutlineColor => new("801212FF");
    public override Color DialogueColor => new("590700");
    public override Color MapDrawingColor => new("CB282B");
    public override Color RemoteTargetingLineColor => new("E15847FF");
    public override Color RemoteTargetingLineOutline => new("801212FF");
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter",
    ];
}

/// <summary>Yixian Heptastar Pavilion Card Pool Model.</summary>
public sealed class YxHeptastarPavilionCardPool : CardPoolModel
{
    public override string Title => StringHelper.SnakeCase(nameof(YxHeptastarPavilion));
    public override string CardFrameMaterialPath => "card_frame_" + Title;
    public override string EnergyColorName => YxHeptastarPavilion.ENERGY_COLOR_NAME;
    public override Color EnergyOutlineColor => YxHeptastarPavilion.ENERGY_OUTLINE_COLOR;
    public override Color DeckEntryCardColor => YxHeptastarPavilion.DECK_ENTRY_CARD_COLOR;
    public override bool IsColorless => false;
    protected override CardModel[] GenerateAllCards() => [
        ModelDb.Card<YxStrikeHeptastarPavilion>(),
    ];
}

/// <summary>Yixian Heptastar Pavilion Relic Pool Model.</summary>
public sealed class YxHeptastarPavilionRelicPool : RelicPoolModel
{
    public override string EnergyColorName => YxHeptastarPavilion.ENERGY_COLOR_NAME;
    public override Color LabOutlineColor => YxHeptastarPavilion.LAB_OUTLINE_COLOR;
    protected override IEnumerable<RelicModel> GenerateAllRelics() => [];
    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState) => AllRelics;
}

/// <summary>Yixian Heptastar Pavilion Potion Pool Model.</summary>
public sealed class YxHeptastarPavilionPotionPool : PotionPoolModel
{
    public override string EnergyColorName => YxHeptastarPavilion.ENERGY_COLOR_NAME;
    public override Color LabOutlineColor => YxHeptastarPavilion.LAB_OUTLINE_COLOR;
    protected override IEnumerable<PotionModel> GenerateAllPotions() => [];
    public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState) => AllPotions;
}
