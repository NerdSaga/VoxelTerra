using Godot;

namespace VoxelTerra.Debugging;

public partial class VTDebug : Node
{
    private static VTDebug instance;

    [Export] private Label FPSLabel;
    [Export] private Label GPULabel;
    [Export] private Label VSyncLabel;

    /// <summary>
    /// Pushed an error, and pauses the game.
    /// </summary>
    /// <param name="message"></param>
    public static void ErrorAbort(string message)
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
            ErrorAbort("Created multiple instances of VTDebug");
        }

        instance = this;
    }

    public override void _ExitTree()
    {
        instance = null;
    }


    
}