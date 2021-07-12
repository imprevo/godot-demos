using Godot;
using System;

public class PlayerTank : Node2D
{
    [Export] private NodePath _tankPath;
    private Tank _tank;

    public override void _Ready()
    {
        _tank = GetNode<Tank>(_tankPath);
    }

    public override void _Process(float delta)
    {
        var inputMovement = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        var inputRotation = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        var inputAttack = Input.IsActionPressed("left_mouse");
        var turretTarget = GetGlobalMousePosition();

        _tank.Rotate(inputMovement, delta);
        _tank.Move(inputRotation, delta);
        _tank.RotateTurret(turretTarget);
        if (inputAttack)
        {
            _tank.Shoot();
        }
    }
}
