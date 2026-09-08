using Godot;

namespace VoxelTerra.Debugging;
public partial class VTDebug : Node
{
    private static VTDebug instance;

    public static void ErrorAbort(string message)
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Window.GetFocusedWindow().Title = "ErrorAbort: " + message;
        GD.PushError(message);
        instance.GetTree().Paused = true;
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