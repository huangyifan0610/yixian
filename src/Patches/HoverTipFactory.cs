using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Yixian.Patches;

/// <summary>
/// More static hover tips.
/// </summary>
public enum StaticHoverTipExtension
{
    /// <summary>
    /// Post action.
    /// </summary>
    PostAction,
}

/// <summary>
/// More hover tips.
/// </summary>
public static class HoverTipFactoryExtension
{
    private const string STATIC_LOC_TABLE = "static_hover_tips";

    /// <summary>
    /// Factory method that returns static hover tip.
    /// </summary>
    public static HoverTip Static(StaticHoverTipExtension staticHoverTip)
    {
        var text = StringHelper.Slugify(staticHoverTip.ToString());
        var title = new LocString(STATIC_LOC_TABLE, text + ".title");
        var description = new LocString(STATIC_LOC_TABLE, text + ".description");
        return new(title, description);
    }
}