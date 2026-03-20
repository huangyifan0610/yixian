using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>
/// A power indicating your Star Points.
/// </summary>
public sealed class StarPointPower : PowerModel
{
    /// <summary>
    /// Neither buff nor debuff.
    /// </summary>
    public override PowerType Type => PowerType.None;

    /// <summary>
    /// Unquie status for every player.
    /// </summary>
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// Variables for localization.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        // comma-seperated integers displaying current Star Points.
        new StringVar(STAR_POINT_LIST_VAR, _starPoints.ToString())
    ];
    private const string STAR_POINT_LIST_VAR = "StarPointList";

    /// <summary>
    /// The i-th bit indicates whether the i-th slot is Star Point.
    /// </summary>
    private ulong _starPoints = 0;

    /// <summary>
    /// The third and sixth slots are the default Star Point. 
    /// </summary>
    public StarPointPower()
    {
        SetStarPoint(2);
        SetStarPoint(5);
    }

    /// <summary>
    /// Returns true if the index-th slot is Star Point. 
    /// </summary>
    public bool IsStarPoint(int index)
    {
        return 0 <= index && index < 64 && (_starPoints & (1ul << index)) != 0;
    }

    /// <summary>
    /// Returns true if the slot is default Star Point.
    /// </summary>
    public static bool IsDefaultStarPoint(int index)
    {
        return index == 2 || index == 5;
    }

    /// <summary>
    /// Sets the index-th slot to Star Point.
    /// </summary>
    public void SetStarPoint(int index)
    {
        if (0 <= index && index < 64 && !IsStarPoint(index))
        {
            _starPoints |= 1ul << index;
            var list = (StringVar)DynamicVars[STAR_POINT_LIST_VAR];
            list.StringValue = ToString();
        }
    }

    /// <summary>
    /// Returns a list of comma-seperated integers displaying current Star Points.
    /// </summary>
    public override string ToString()
    {
        var display = new StringBuilder(32);
        bool comma = false;

        for (int index = 0; index < 64; ++index)
        {
            if (IsStarPoint(index))
            {
                if (comma)
                {
                    display.Append(", ");
                }
                else
                {
                    comma = true;
                }

                display.Append(index + 1);
            }
        }

        return display.ToString();
    }
}
