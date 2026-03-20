using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

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
}