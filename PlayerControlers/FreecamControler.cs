using Godot;
using System;

public partial class FreecamControler : CharacterBody3D
{

    [Export] private Camera3D Camera;
    [Export] private float mouseSensitivity = 0.01f;
    [Export] private float movmentMultiplier = 20;

    private Vector3 inputDirection = Vector3.Zero;
    private Vector3 targetCamRotation = Vector3.Zero;
    private bool _inputEnabled = true;
    private bool inputEnabled
    {
        get
        {
            return _inputEnabled;
        }
        set
        {
            _inputEnabled = value;
            if (value == false)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
                return;
            }

            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public override void _Process(double delta)
    {
        inputDirection.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        inputDirection.Y = Input.GetActionStrength("move_up") - Input.GetActionStrength("move_down");
        inputDirection.Z = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");

        Camera.Rotation = Camera.Rotation.Lerp(targetCamRotation, 13f * (float)delta);

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            inputEnabled = !inputEnabled;
        }
    }


    public override void _PhysicsProcess(double delta)
    {

        if (!inputEnabled)
        {
            return;
        }

        Vector3 velocity = Vector3.Zero;

        Vector3 xBasis = Camera.Basis.X;
        xBasis.Y = 0;
        xBasis = xBasis.Normalized();

        Vector3 zBasis = Camera.Basis.Z;
        zBasis.Y = 0;
        zBasis = zBasis.Normalized();

        velocity += xBasis * inputDirection.X;
        velocity += zBasis * inputDirection.Z;
        velocity += new Vector3(0, 1, 0).Normalized() * inputDirection.Y;
        velocity *= movmentMultiplier;

        Velocity = Velocity.Lerp(velocity, 0.2f);

        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && inputEnabled)
        {
            targetCamRotation.Y += -mouseMotion.Relative.X * mouseSensitivity;
            targetCamRotation.X += -mouseMotion.Relative.Y * mouseSensitivity;
        }
    }


    public override void _Ready()
    {
        targetCamRotation = Camera.Rotation;
        inputEnabled = true;
    }

}
