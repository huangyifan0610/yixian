using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace Yixian.Powers;

/// <summary>
/// Hexagram Power.
/// </summary>
public sealed class HexagramPower : PowerModel
{
    /// <summary>
    /// Neither buff nor debuff.
    /// </summary>
    public override PowerType Type => PowerType.None;

    /// <summary>
    /// Hexagram can be counted.
    /// </summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// Applies hexagram on random percentage.
    /// </summary>
    /// <param name="creature">The owner of hexagram.</param>
    /// <param name="cardSource">The card that requrests the random percentage.</param>
    /// <param name="runState">The runtime state.</param>
    /// <param name="successPercentege">The percentage of succeess.</param>
    /// <returns>Returns the boolean test result.</returns>
    public static async Task<bool> Percentege(Creature creature, CardModel? cardSource, IRunState runState, decimal successPercentege)
    {
        var hexagramPower = creature.GetPower<HexagramPower>();
        if (hexagramPower?.Amount > 0)
        {
            // Consumes one hexagram.
            await PowerCmd.ModifyAmount(hexagramPower, -1m, creature, cardSource);

            // Always returns true.
            return true;
        }
        else
        {
            return GetRng(runState).NextDouble() < (double)successPercentege;
        }
    }

    /// <summary>
    /// Applies hexagram on random range.
    /// </summary>
    /// <param name="creature">The owner of hexagram.</param>
    /// <param name="cardSource">The card that requrests the random percentage.</param>
    /// <param name="runState">The runtime state.</param>
    /// <param name="lower">The lower bound (inclusive).</param>
    /// <param name="upper">The upper bound (inclusive).</param>
    /// <returns></returns>
    public static async Task<int> Range(Creature creature, CardModel? cardSource, IRunState runState, int lower, int upper)
    {
        var hexagramPower = creature.GetPower<HexagramPower>();
        if (hexagramPower?.Amount > 0)
        {
            // Consumes one hexagram.
            await PowerCmd.ModifyAmount(hexagramPower, -1m, creature, cardSource);

            // Always returns the maximum value.
            return upper;
        }
        else
        {
            return GetRng(runState).NextInt(lower, upper + 1);
        }
    }

    /// <summary>
    /// Returns RNG for random effects without hexgrams. 
    /// </summary>
    // FIXME: We need a specialized RNG for hexagrams.
    private static Rng GetRng(IRunState runState) => runState.Rng.Niche;
}