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
    /// Returns RNG for random effects without hexgrams. 
    /// </summary>
    public static Rng GetRng(IRunState runState)
    {
        // FIXME: We need a specialized RNG for hexagrams.
        return runState.Rng.Niche;
    } 
}