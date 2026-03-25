using System;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Yixian.Characters;

namespace Yixian.Nodes.Screen.CharSelect;

public partial class CharSelectBgYxHeptastarPavilion : Control
{
    [Export] public ButtonGroup? CharAvatarGroup = default;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() => CharAvatarGroup?.Connect(ButtonGroup.SignalName.Pressed, Callable.From<BaseButton>(selected =>
    {
        Main.LOGGER.Info("Character selected: " + selected.Name);
        if (!Enum.TryParse<YxHeptastarPavilionCharacter>(selected.Name, ignoreCase: true, out var character))
        {
            Main.LOGGER.Warn($"Unknown character '{selected.Name}' in Heptastar Pavilion.");
            character = default;
        }
        
        var model = ModelDb.Character<YxHeptastarPavilion>();
        if (model.Character != character)
        {
            model.Character = character;
            // We are in "/root/Game/RootSceneContainer/MainMenu/Submenus/CharacterSelectScreen/AnimatedBg/YX_HEPTASTAR_PAVILION_bg"
            // We want   "/root/Game/RootSceneContainer/MainMenu/Submenus/CharacterSelectScreen/CharSelectButtons/ButtonContainer/YX_HEPTASTAR_PAVILION_button"
            var screen = GetParent().GetParent<NCharacterSelectScreen>();
            var button = screen.GetNode<NCharacterSelectButton>("CharSelectButtons/ButtonContainer/YX_HEPTASTAR_PAVILION_button");
            screen.SelectCharacter(button, model);
        }
    }));
}
