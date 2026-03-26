using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
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

/// <summary>Extension to <see cref="YxHeptastarPavilionCharacter"/>.</summary>
public static class YxHeptastarPavilionCharacterExtension
{
    /// <summary>Returns the character ID in uppercase.</summary>
    public static string Uppercase(this YxHeptastarPavilionCharacter character) => character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => "YX_TAN_SHUYAN",
        YxHeptastarPavilionCharacter.YanChen => "YX_YAN_CHEN",
        YxHeptastarPavilionCharacter.YaoLing => "YX_YAO_LING",
        YxHeptastarPavilionCharacter.JiangXiming => "YX_JIANG_XIMING",
        YxHeptastarPavilionCharacter.WuCe => "YX_WU_CE",
        YxHeptastarPavilionCharacter.FengXu => "YX_FENG_XU",
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the character ID in lowercase.</summary>
    public static string Lowercase(this YxHeptastarPavilionCharacter character) => character.Uppercase().ToLowerInvariant();

    /// <summary>Returns the character gender.</summary>
    public static CharacterGender Gender(this YxHeptastarPavilionCharacter character) => character switch
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
    public static int StartingHp(this YxHeptastarPavilionCharacter character) => character switch
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
    public static int StartingGold(this YxHeptastarPavilionCharacter character) => character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => 99,
        YxHeptastarPavilionCharacter.YanChen => 99,
        YxHeptastarPavilionCharacter.YaoLing => 199,
        YxHeptastarPavilionCharacter.JiangXiming => 99,
        YxHeptastarPavilionCharacter.WuCe => 99,
        YxHeptastarPavilionCharacter.FengXu => 99,
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the maximum energy.</summary>
    public static int MaxEnergy(this YxHeptastarPavilionCharacter character) => character switch
    {
        _ => 3,
    };

    /// <summary>Returns the color of character name.</summary>
    public static Color NameColor(this YxHeptastarPavilionCharacter character) => character switch
    {
        _ => YxHeptastarPavilion.LAB_OUTLINE_COLOR,
    };

    /// <summary>Returns the inital deck.</summary>
    public static IEnumerable<CardModel> StartingDeck(this YxHeptastarPavilionCharacter character) => character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        YxHeptastarPavilionCharacter.YanChen => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        YxHeptastarPavilionCharacter.YaoLing => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        YxHeptastarPavilionCharacter.JiangXiming => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        YxHeptastarPavilionCharacter.WuCe => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        YxHeptastarPavilionCharacter.FengXu => [
            ModelDb.Card<YxStrikeHeptastarPavilion>(),
        ],
        _ => throw new UnreachableException("unknown character"),
    };

    /// <summary>Returns the initial relics.</summary>
    public static IReadOnlyList<RelicModel> StartingRelics(this YxHeptastarPavilionCharacter character) => character switch
    {
        YxHeptastarPavilionCharacter.TanShuyan => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.YanChen => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.YaoLing => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.JiangXiming => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.WuCe => [ModelDb.Relic<BurningBlood>()],
        YxHeptastarPavilionCharacter.FengXu => [ModelDb.Relic<BurningBlood>()],
        _ => throw new UnreachableException("unknown character"),
    };
}
