using Godot;
using System;

public partial class CommonInput : Node
{

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
		{
			if (@event.IsActionReleased("toggle_fullscreen"))
			{
				toggleFullscreen();
			}
		}
    }

	private void toggleFullscreen()
	{
		Window window = GetWindow();
		if (window.Mode == Window.ModeEnum.Windowed)
		{
			window.Mode = Window.ModeEnum.Fullscreen;
			return;
		}

		window.Mode = Window.ModeEnum.Windowed;
	}

}
