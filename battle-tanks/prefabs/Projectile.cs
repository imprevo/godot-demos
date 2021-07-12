using Godot;
using System;

public class Projectile : KinematicBody2D
{
    private float _speed = -20000;
    private Vector2 _movement;

    public override void _Ready()
    {
        _movement = new Vector2(0, _speed).Rotated(Rotation);
    }

    public override void _Process(float delta)
    {
        MoveAndSlide(_movement * delta);
    }

    private void OnDestroyTimerTimeout()
    {
        QueueFree();
    }
}

