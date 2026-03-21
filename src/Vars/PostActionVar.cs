using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Vars;

/// <summary>
/// Variable indicating whether to trigger the post action effect.
/// </summary>
public class PostActionVar() : BoolVar(DEFAULT)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT = "PostAction";

    /// <summary>
    /// Marks that the card has been played.
    /// </summary>
    public void SetPlayed()
    {
        BoolVal = true;
    }

    /// <summary>
    /// Returns true if the card has been played.
    /// </summary>
    public bool IsPlayed()
    {
        return BoolVal;
    }

    /// <summary>
    /// Returns true if the card has been played.
    /// </summary>
    public bool IsPlayed(CardPlay cardPlay)
    {
        return BoolVal || !cardPlay.IsFirstInSeries;
    }
}