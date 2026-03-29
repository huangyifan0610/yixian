using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Preemptive Strike Power.</summary>
public sealed class YxPreemptiveStrikePower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>Single stack.</summary>
    public override PowerStackType StackType => PowerStackType.Single;
}
