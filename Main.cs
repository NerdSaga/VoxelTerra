using Godot;
using System;

public partial class Main : Node3D
{

    [Export] LineEdit SeedLineEdit;
    [Export] Label SeedLineEditLabelError;
    public void _OnButtonPressed()
    {
        if (!SeedLineEdit.Text.IsValidInt())
        {
            SeedLineEditLabelError.Visible = true;
            return;
        }

        int seed = SeedLineEdit.Text.ToInt();

        World world = World.Create(seed);
        GetTree().ChangeSceneToNode(world);
    }
}
