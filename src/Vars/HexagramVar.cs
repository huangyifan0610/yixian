
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Vars;

/// <summary>
/// Variable indicating the amount of Hexagram.
/// </summary>
public class HexagramVar(string name, decimal amount): DynamicVar(name, amount)
{
    /// <summary>
    /// The default variable name.
    /// </summary>
    public const string DEFAULT_NAME = "Hexagram";
    
    /// <summary>
    /// Construct with default variable name.
    /// </summary>
    public HexagramVar(decimal amount) : this(DEFAULT_NAME, amount) {}

    /// <summary>
    /// Construct with default variable name.
    /// </summary>
    public HexagramVar(int amount) : this(DEFAULT_NAME, amount) {}
}
