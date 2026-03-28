using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Heptastar Soulstat Power.</summary>
/// <remarks>Takes an extra turn.</remarks>
public sealed class YxHeptastarSoulstatPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The buff counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Takes an extra turn.</summary>
    public override bool ShouldTakeExtraTurn(Player player) => player.Creature == Owner;

    /// <summary>Decrease the power.</summary>
    public override async Task AfterTakingExtraTurn(Player player)
    {
        if (player.Creature == Owner)
        {
            await PowerCmd.ModifyAmount(this, -1m, Owner, null);
        }
    }
}
