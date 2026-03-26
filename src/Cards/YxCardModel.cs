using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Yixian.Powers;

namespace Yixian.Cards;

/// <summary>Abstract class for cards in Yixian Mod.</summary>
public abstract class YxCardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true)
    : CardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    /// <summary>Canonical card tags.</summary>
    public virtual IEnumerable<YxCardTag> CanonicalYxTags => [];

    /// <summary>Canonical card Keywords.</summary>
    public virtual IEnumerable<YxCardKeyword> CanonicalYxKeywords => [];

    /// <summary>Card tags.</summary>
    public IReadOnlySet<YxCardTag> YxTags => _yxTags ??= [.. CanonicalYxTags];
    private HashSet<YxCardTag>? _yxTags = null;

    /// <summary>Card Keywords.</summary>
    public IReadOnlySet<YxCardKeyword> YxKeywords => _yxKeywords ??= [.. CanonicalYxKeywords];
    private HashSet<YxCardKeyword>? _yxKeywords = null;

    /// <summary>Returns true if the card is 'Astral Move'.</summary>
    public bool IsAstralMove => YxTags.Contains(YxCardTag.AstralMove);

    /// <summary>Returns true if the card is 'Post Action'.</summary>
    public bool IsPostAction => YxKeywords.Contains(YxCardKeyword.PostAction);

    /// <summary>Returns true if the card is on 'Star Point'.</summary>
    public bool IsOnStarPoint => YxStarPointPower.Test(this);

    /// <summary>Represents whether the post action is effective.</summary>
    public bool PostAction = false;

    /// <summary>Adds new <paramref name="keyword"/> to the mutable card.</summary>
    public void AddYxKeyword(YxCardKeyword keyword)
    {
        AssertMutable();
        _yxKeywords ??= [.. CanonicalYxKeywords];
        _yxKeywords.Add(keyword);
    }

    /// <summary>Removes <paramref name="keyword"/> from the mutable card.</summary>
    public void RemoveYxKeyword(YxCardKeyword keyword)
    {
        AssertMutable();
        _yxKeywords ??= [.. CanonicalYxKeywords];
        _yxKeywords.Remove(keyword);
    }

    /// <summary>Returns true if the post action is effective.</summary>
    public async Task<bool> TryPostAction(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Short Path.
        if (PostAction) { return true; }

        // Activates the post action.
        PostAction = true;

        return false;
    }
}
