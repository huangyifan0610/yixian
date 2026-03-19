using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Cards.HeptastarPavilion;

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
    /// The i-th bit indicates whether the i-th slot is Star Point.
    /// </summary>
    private ulong _starPoints = 0;

    /// <summary>
    /// Variables for localization.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        // comma-seperated integers displaying current Star Points.
        new StringVar(STAR_POINT_LIST_VAR)
    ];
    private const string STAR_POINT_LIST_VAR = "StarPointList";

    /// <summary>
    /// Returns true if the index-th slot is Star Point. 
    /// </summary>
    public bool this[int index]
    {
        get
        {
            return (_starPoints & (1ul << index)) != 0;
        }
        set
        {
            ulong old = _starPoints;

            if (value)
            {
                _starPoints |= 1ul << index;
            }
            else
            {
                _starPoints &= ~(1ul << index);
            }

            // Also updates the displaying string.
            if (old != _starPoints)
            {
                var starPointList = (StringVar)DynamicVars[STAR_POINT_LIST_VAR];
                starPointList.StringValue = ToString();
            }
        }
    }

    /// <summary>
    /// The third and sixth slots are the default Star Point. 
    /// </summary>
    public StarPointPower()
    {
        // The third slot.
        this[2] = true;
        // The sixth slot.
        this[5] = true;
    }

    /// <summary>
    /// Updates star points for hand cards.
    /// </summary>
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        var hand = Owner?.Player?.PlayerCombatState?.Hand;
        if (hand != null)
        {
            hand.ContentsChanged += () =>
            {
                for (int index = 0; index < hand.Cards.Count; ++index)
                {
                    if (hand.Cards[index] is StarPointCardModel starPointCard && starPointCard.IsStarPointVar != null)
                    {
                        starPointCard.IsStarPointVar.BoolVal = this[index];
                    }
                }
            };
        }
        return base.AfterApplied(applier, cardSource);
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
            if (this[index])
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