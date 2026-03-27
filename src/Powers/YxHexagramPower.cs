using System;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace Yixian.Powers;

/// <summary>Hexagram Power.</summary>
public sealed class YxHexagramPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The hexagram counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;
}

/// <summary>Extension to <see cref="YxHexagramPower"/>.</summary>
public static class YxHexagramPowerExtension
{
    /// <summary>Returns RNG for random effects without hexgrams.</summary>
    // FIXME: We need a specialized RNG for hexagrams.
    private static Rng GetRng(IRunState runState) => runState.Rng.Niche;

    /// <summary>
    /// Attempts to consume one stack of <paramref name="hexagram"/>. On success, returns 
    /// <paramref name="max"/>; On failure, returns random range between <paramref name="min"/> and
    /// <paramref name="max"/> (both inclusive).
    /// </summary>
    public static int Range(this YxHexagramPower? hexagram, IRunState runState, int min, int max)
    {
        if (hexagram?.Amount > 0)
        {
            hexagram.SetAmount(hexagram.Amount - 1);
            return max;
        }
        else
        {
            return GetRng(runState).NextInt(min, max + 1);
        }
    }

    /// <summary>
    /// Attempts to consume one stack of <paramref name="hexagram"/>. On success, returns true;
    /// On failure, returns true for a chance of <paramref name="percentage"/>.
    /// </summary>
    public static bool Test(this YxHexagramPower? hexagram, IRunState runState, decimal percentage)
    {
        if (hexagram?.Amount > 0)
        {
            hexagram.SetAmount(hexagram.Amount - 1);
            return true;
        }
        else
        {
            return GetRng(runState).NextInt(100) < (percentage * 100m);
        }
    }

    /// <summary>
    /// Repeats <paramref name="hexagram"/>.Percentage(<paramref name="runState"/>, 
    /// <paramref name="percentage"/>) for <paramref name="repeat"/> times. 
    /// Returns the number of times the boolean test succeeds.
    /// </summary>
    public static int Test(this YxHexagramPower? hexagram, IRunState runState, decimal percentage, int repeat)
    {
        if (hexagram?.Amount >= repeat)
        {
            hexagram.SetAmount(hexagram.Amount - repeat);
            return repeat;
        }
        else
        {
            int success = 0;

            if (hexagram?.Amount > 0)
            {
                success = hexagram.Amount;
                hexagram.SetAmount(0);
            }

            for (int j = success; j < repeat; ++j)
            {
                if (GetRng(runState).NextInt(100) < (percentage * 100m))
                {
                    ++success;
                }
            }

            return success;
        }
    }
}
