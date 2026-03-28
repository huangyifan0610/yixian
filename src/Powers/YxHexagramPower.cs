using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using Yixian.Patches;

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
    private static Rng GetRng(IRunState state) => state.Rng.GetRngForHexagram();

    /// <summary>
    /// Attempts to consume one stack of <paramref name="hexagram"/>. On success, returns 
    /// <paramref name="max"/>; On failure, returns random range between <paramref name="min"/> and
    /// <paramref name="max"/> (both inclusive).
    /// </summary>
    public static int Range(this YxHexagramPower? hexagram, IRunState state, int min, int max, out bool hexagramUsed)
    {
        if (hexagram?.Amount > 0)
        {
            PowerCmd.Decrement(hexagram);
            hexagramUsed = true;
            return max;
        }
        else
        {
            hexagramUsed = false;
            return GetRng(state).NextInt(min, max + 1);
        }
    }

    /// <summary>
    /// Attempts to consume one stack of <paramref name="hexagram"/>. On success, returns true;
    /// On failure, returns true for a chance of <paramref name="percent0to100"/>.
    /// </summary>
    public static bool Test(this YxHexagramPower? hexagram, IRunState state, decimal percent0to100, out bool hexagramUsed)
    {
        if (hexagram?.Amount > 0)
        {
            PowerCmd.Decrement(hexagram);
            hexagramUsed = true;
            return true;
        }
        else
        {
            hexagramUsed = false;
            return GetRng(state).NextInt(100) < percent0to100;
        }
    }

    /// <summary>
    /// Repeats <paramref name="hexagram"/>.Percentage(<paramref name="state"/>, 
    /// <paramref name="percent0to100"/>) for <paramref name="repeat"/> times. 
    /// Returns the number of times the boolean test succeeds.
    /// </summary>
    public static int Test(this YxHexagramPower? hexagram, IRunState state, decimal percent0to100, int repeat, out int hexagramUsed)
    {
        if (hexagram?.Amount >= repeat)
        {
            PowerCmd.ModifyAmount(hexagram, -repeat, hexagram.Owner, null);
            hexagramUsed = repeat;
            return repeat;
        }
        else
        {
            if (hexagram?.Amount > 0)
            {
                hexagramUsed = hexagram.Amount;
                hexagram.SetAmount(0);
            }
            else
            {
                hexagramUsed = 0;
            }

            int success = hexagramUsed;
            for (int j = success; j < repeat; ++j)
            {
                if (GetRng(state).NextInt(100) < percent0to100)
                {
                    ++success;
                }
            }

            return success;
        }
    }
}
