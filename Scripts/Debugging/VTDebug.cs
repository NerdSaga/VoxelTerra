using Godot;

namespace VoxelTerra.Debugging;

/// <summary>
/// An AutoLoad class that is used for various debugging tasks. It also has it's own user interface 🔥
/// </summary>
public partial class VTDebug : Node
{
    private static VTDebug instance;

    [Export] private Control MainControl;
    [Export] private Label FPSLabel;
    [Export] private Label GPULabel;
    [Export] private Label VSyncLabel;

    /// <summary>
    /// Pushes an error to Godot and pauses the game.
    /// </summary>
    /// <param name="message"></param>
    public static void ErrorPause(string message)
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Window.GetFocusedWindow().Title = "ErrorAbort: " + message;
        GD.PushError(message);
        instance.GetTree().Paused = true;
    }

    public override void _Process(double delta)
    {
        FPSLabel.Text = $"FPS: {Engine.GetFramesPerSecond().ToString()}";
        VSyncLabel.Text = $"V-Sync: {DisplayServer.WindowGetVsyncMode(0).ToString()}";
        GPULabel.Text = $"Graphics: {RenderingServer.GetVideoAdapterName()} | {RenderingServer.GetCurrentRenderingDriverName()}";
    }


    public override void _EnterTree()
    {
        if (instance != null)
        {
            ErrorPause("Created multiple instances of VTDebug");
        }

        instance = this;
    }

    public override void _ExitTree()
    {
        instance = null;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.IsActionReleased("toggle_debug_menu"))
            {
                MainControl.Visible = !MainControl.Visible;
            }
        }
    }

    public override void _Ready()
    {
        MainControl.Visible = false;
    }


}