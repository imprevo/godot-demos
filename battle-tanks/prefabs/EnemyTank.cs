using Godot;
using System;

public class EnemyTank : Node2D
{
    [Export] private NodePath _tankPath;
    private Tank _tank;
    private Vector2 _target;

    public override void _Ready()
    {
        _tank = GetNode<Tank>(_tankPath);
        _tank.Rotation = Mathf.Deg2Rad(180);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("left_mouse"))
        {
            _target = GetGlobalMousePosition();
        }

        if (_target == null)
            return;

        _tank.RotateTurret(_target);

        var angleToTarget = _tank.GetAngleTo(_target) + Mathf.Deg2Rad(90);
        var inputMovement = 0;

        if (Math.Abs(angleToTarget) > 0.5)
        {
            var angleInput = angleToTarget < 0 ? -1 : 1;
            _tank.Rotate(angleInput, delta);
        }
        else
        {
            var distanceToTarget = _tank.GlobalPosition.DistanceTo(_target);

            if (distanceToTarget > 300)
            {
                inputMovement = -1;
            }
            else
            {
                _tank.Shoot();
            }
        }

        _tank.Move(inputMovement, delta);
    }
}
