using Godot;
using System;

public class ControllableBall : KinematicBody2D
{
    private float _acceleration = 600;
    private float _rotationSpeed = 2;
    private float _maxSpeed = 300;
    private float _friction = 900;
    private Vector2 _movement;

    public override void _Process(float delta)
    {
        var inputRotation = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        var inputMovement = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        Rotate(inputRotation, delta);
        Move(inputMovement, delta);
    }

    public void Rotate(float input, float delta)
    {
        Rotation += input * _rotationSpeed * delta;
    }

    public void Move(float input, float delta)
    {
        if (input == 0)
        {
            _movement = _movement.MoveToward(Vector2.Zero, _friction * delta);
        }
        else
        {
            var direction = new Vector2(0, input).Normalized().Rotated(Rotation);
            _movement = _movement.MoveToward(direction * _maxSpeed, _acceleration * delta);
        }

        _movement = MoveAndSlide(_movement);
    }
}
