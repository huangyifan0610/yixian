using System.Collections.Generic;
using System.Diagnostics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Yixian.Cards;

/// <summary>Card keywords.</summary>
public enum YxCardKeyword
{
    /// <summary>Astral move.</summary>
    AstralMove,
    /// <summary>Character exclusive - Tan Shuyan.</summary>
    CharacterExclusiveTanShuyan,
    /// <summary>Character exclusive - Yan Chen.</summary>
    CharacterExclusiveYanChen,
    /// <summary>Character exclusive - Yao Ling.</summary>
    CharacterExclusiveYaoLing,
    /// <summary>Character exclusive - Jiang Ximing.</summary>
    CharacterExclusiveJiangXiming,
    /// <summary>Character exclusive - Wu Ce.</summary>
    CharacterExclusiveWuCe,
    /// <summary>Character exclusive - Feng Xu.</summary>
    CharacterExclusiveFengXu,
    /// <summary>Hexagram.</summary>
    Hexagram,
    /// <summary>Post action.</summary>
    PostAction,
    /// <summary>Thunder.</summary>
    Thunder,
}

/// <summary>Extension to <see cref="YxCardKeyword"/>.</summary>
public static class YxCardKeywordExtension
{
    /// <summary>The locale table.</summary>
    private const string LOC_TABLE = "card_keywords";

    /// <summary>The cached hover tips.</summary>
    private static readonly Dictionary<YxCardKeyword, HoverTip> _hoverTips = [];

    /// <summary>Returns the prefix of the locale key.</summary>
    public static string GetLocKeyPrefix(this YxCardKeyword keyword) => keyword switch
    {
        YxCardKeyword.AstralMove => "YX_ASTRAL_MOVE",
        YxCardKeyword.CharacterExclusiveTanShuyan => "YX_CHARACTER_EXCLUSIVE_TAN_SHUYAN",
        YxCardKeyword.CharacterExclusiveYanChen => "YX_CHARACTER_EXCLUSIVE_YAN_CHEN",
        YxCardKeyword.CharacterExclusiveYaoLing => "YX_CHARACTER_EXCLUSIVE_YAO_LING",
        YxCardKeyword.CharacterExclusiveJiangXiming => "YX_CHARACTER_EXCLUSIVE_JIANG_XIMING",
        YxCardKeyword.CharacterExclusiveWuCe => "YX_CHARACTER_EXCLUSIVE_WU_CE",
        YxCardKeyword.CharacterExclusiveFengXu => "YX_CHARACTER_EXCLUSIVE_FENG_XU",
        YxCardKeyword.Hexagram => "YX_HEXAGRAM",
        YxCardKeyword.PostAction => "YX_POST_ACTION",
        YxCardKeyword.Thunder => "YX_THUNDER",
        _ => throw new UnreachableException("unknown keyword"),
    };

    /// <summary>Returns the title locale key of the keyword.</summary>
    public static LocString GetTitle(this YxCardKeyword keyword) => new(LOC_TABLE, keyword.GetLocKeyPrefix() + ".title");

    /// <summary>Returns the description locale key of the keyword.</summary>
    public static LocString GetDescription(this YxCardKeyword keyword) => new(LOC_TABLE, keyword.GetLocKeyPrefix() + ".description");

    /// <summary>Returns hover tip for the keyword.</summary>
    public static HoverTip GetHoverTip(this YxCardKeyword keyword)
    {
        if (!_hoverTips.TryGetValue(keyword, out var hoverTip))
        {
            hoverTip = new HoverTip(keyword.GetTitle(), keyword.GetDescription());
            _hoverTips.Add(keyword, hoverTip);
        }
        return hoverTip;
    }

    /// <summary>Returns true if the keyword describes character exclusive cards.</summary>
    public static bool IsCharacterExclusive(this YxCardKeyword keyword) => keyword switch
    {
        YxCardKeyword.CharacterExclusiveTanShuyan => true,
        YxCardKeyword.CharacterExclusiveYanChen => true,
        YxCardKeyword.CharacterExclusiveYaoLing => true,
        YxCardKeyword.CharacterExclusiveJiangXiming => true,
        YxCardKeyword.CharacterExclusiveWuCe => true,
        YxCardKeyword.CharacterExclusiveFengXu => true,
        _ => false,
    };
}
