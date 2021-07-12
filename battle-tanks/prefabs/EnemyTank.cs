using Godot;
using System;

public class EnemyTank : Node2D
{
    [Export] private NodePath _tankPath;
    private Tank _tank;

    public override void _Ready()
    {
        _tank = GetNode<Tank>(_tankPath);
        _tank.Rotation = Mathf.Deg2Rad(180);
    }

    public override void _Process(float delta)
    {
        // TODO: get player position
        var turretTarget = GetGlobalMousePosition();

        _tank.RotateTurret(turretTarget);
    }
}
