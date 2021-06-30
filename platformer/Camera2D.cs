using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    private Node2D _target;
    private float _cameraOffset = 200;
    private float _minPosition;

    public override void _Ready()
    {
        _minPosition = Position.x;
    }

    public override void _Process(float delta)
    {
        if (_target != null)
        {
            var nextX = Math.Max(_minPosition, _target.GlobalPosition.x + _cameraOffset);
            Position = new Vector2(Mathf.Lerp(Position.x, nextX, 2 * delta), Position.y);
        }
    }

    public void setTarget(Node2D target)
    {
        _target = target;
    }
}
