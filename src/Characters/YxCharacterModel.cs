using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Characters;

/// <summary>Abstract character model in Yixian Mod.</summary>
public abstract class YxCharacterModel : CharacterModel
{
    /// <summary>Returns character animations.</summary>
    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        // Important animations.
        AnimState idle = new("Idle", isLooping: true);
        CreatureAnimator creatureAnimator = new(idle, controller);
        creatureAnimator.AddAnyState("Idle", idle);

        AnimState gameRelax = new("Game_Relax", isLooping: true);
        gameRelax.AddBranch("Idle", idle);
        creatureAnimator.AddAnyState("Relaxed", gameRelax);

        creatureAnimator.AddAnyState("Dead", new("Dead"));
        creatureAnimator.AddAnyState("Hit", new("Hurt") { NextState = idle });
        creatureAnimator.AddAnyState("Attack", new("Attack_1") { NextState = idle });
        creatureAnimator.AddAnyState("Cast", new("Cast_1") { NextState = idle });

        // More animations.
        creatureAnimator.AddAnyState("Attack2", new("Attack_2") { NextState = idle });
        creatureAnimator.AddAnyState("Attack3", new("Attack_3") { NextState = idle });
        creatureAnimator.AddAnyState("Attack4", new("Attack_4") { NextState = idle });
        creatureAnimator.AddAnyState("Attack5", new("Attack_5") { NextState = idle });
        creatureAnimator.AddAnyState("Attack6", new("Attack_6") { NextState = idle });
        creatureAnimator.AddAnyState("MultiAttack", new("MultiAttack") { NextState = idle });
        creatureAnimator.AddAnyState("Appearance", new("Appearance") { NextState = idle });
        // AnimState appearance2 = new("Appearance_2") { NextState = idle };
        // AnimState cast2 = new("Cast_2") { NextState = idle };
        // AnimState block = new("Block");
        // AnimState blockReady = new("BlockReady");
        // AnimState gameAppearance = new("Game_Appearance");
        // AnimState gameBreakthrough = new("Game_Breakthrough");
        // AnimState gameChange = new("Game_Change");
        // AnimState gameDiscard = new("Game_Discard");
        // AnimState gameHold = new("Game_Hold");
        // AnimState gameIdle = new("Game_Idle", isLooping: true);
        // AnimState gameJump = new("Game_Jump");
        // AnimState gameReady = new("Game_Ready");
        // AnimState gameReadyIdle = new("Game_ReadyIdle");
        // AnimState gameReadyRelax = new("Game_ReadyRelax");
        // AnimState goAhead1 = new("GoAhead_1");
        // AnimState goAhead2 = new("GoAhead_2");
        // AnimState goBack1 = new("GoBack_1");
        // AnimState goBack2 = new("GoBack_2");
        // AnimState multiCast = new("MultiCast");
        // AnimState test = new("Test");
        // AnimState victory = new("Victory");
        return creatureAnimator;
    }

    /// <summary>Returns vfx for attack animation.</summary>
    public virtual string GetAttackVfx(int hitCount) => hitCount switch
    {
        0 => "Attack",
        1 => "Attack",
        2 => "Attack2",
        3 => "Attack3",
        4 => "Attack4",
        5 => "Attack5",
        6 => "Attack6",
        _ => "MultiAttack",
    };
}
