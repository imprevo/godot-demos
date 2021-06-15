using Godot;
using System;

public class Character : KinematicBody2D
{
    private int _speed = 400;
    private int _gravity = 300;

    private AnimatedSprite _animatedSprite;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _Process(float delta)
    {
        var velocity = new Vector2();
        velocity.y = _gravity;

        if (Input.IsActionPressed("ui_right"))
        {
            _animatedSprite.FlipH = false;
            _animatedSprite.Play("run");
            velocity.x = _speed;
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            _animatedSprite.FlipH = true;
            _animatedSprite.Play("run");
            velocity.x = -_speed;
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            velocity.y = -_gravity * 2;
        }
        else if (Input.IsActionPressed("ui_select"))
        {
            _animatedSprite.Play("attack");
        }
        else
        {
            _animatedSprite.Play("idle");
        }

        MoveAndSlide(velocity);
    }
}
