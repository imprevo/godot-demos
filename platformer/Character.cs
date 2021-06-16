using Godot;
using System;

public class Character : KinematicBody2D
{
    private int _acceleration = 900;
    private int _maxSpeed = 400;
    private int _friction = 900;
    private int _gravity = 1000;
    private int _jumpForce = -500;
    private Vector2 _movement;

    private AnimatedSprite _animatedSprite;
    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _movement = new Vector2();
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public override void _Process(float delta)
    {
        // var currentState = _stateMachine.GetCurrentNode();

        if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
        {
            _movement.y = _jumpForce;
        }
        else
        {
            _movement.y += _gravity * delta;
        }

        var movementY = _movement.y;
        var inputX = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");

        if (inputX == 0)
        {
            _movement = _movement.MoveToward(Vector2.Zero, _friction * delta);
        }
        else
        {
            _movement = _movement.MoveToward(new Vector2(inputX, 0) * _maxSpeed, _acceleration * delta);
        }

        _movement.y = movementY;

        _movement = MoveAndSlide(_movement, Vector2.Up);
        RunAnimation();

    }

    private void RunAnimation()
    {
        GD.Print(_movement);

        if (Input.IsActionJustPressed("attack"))
        {
            _stateMachine.Travel("attack1");
        }
        else if (!IsOnFloor())
        {
            _stateMachine.Travel("jump");
        }
        else if (Math.Abs(_movement.x) > 0.001f)
        {
            _stateMachine.Travel("run");
        }
        else
        {
            _stateMachine.Travel("idle");
        }

        if (Math.Abs(_movement.x) > 0.001f)
        {
            _animatedSprite.FlipH = _movement.x < 0;
        }
    }

    public void Hit()
    {
        _stateMachine.Travel("hit");
    }

    public void Die()
    {
        _stateMachine.Travel("die");
        SetPhysicsProcess(false);
    }
}
