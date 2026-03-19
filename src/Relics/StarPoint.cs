using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using Yixian.Powers;

namespace Yixian.Relics;

/// <summary>
/// Star Point Buff.
/// </summary>
public sealed class StarPoint : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StarPointPower>()];

    /// <summary>
    /// See relic <see cref="Vajra"/>.
    /// </summary>
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            Flash();
            // The amount does not matter.
            await PowerCmd.Apply<StarPointPower>(Owner.Creature, 1, Owner.Creature, null);
        }
    }
}
