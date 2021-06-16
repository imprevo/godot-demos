using Godot;
using System;

public class Character : KinematicBody2D
{
    private int _speed = 400;
    private int _gravity = 300;

    private AnimatedSprite _animatedSprite;
    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public override void _Process(float delta)
    {
        // var currentState = _stateMachine.GetCurrentNode();
        var velocity = new Vector2();

        if (Input.IsActionPressed("ui_right"))
        {
            _animatedSprite.FlipH = false;
            velocity.x = _speed;
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            _animatedSprite.FlipH = true;
            velocity.x = -_speed;
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            velocity.y = -_gravity * 2;
        }

        RunAnimation(velocity);

        velocity.y += _gravity;
        MoveAndSlide(velocity);
    }

    private void RunAnimation(Vector2 velocity)
    {
        if (Input.IsActionPressed("ui_select"))
        {
            _stateMachine.Travel("attack1");
        }
        else if (velocity.Length() == 0)
        {
            _stateMachine.Travel("idle");
        }
        else
        {
            _stateMachine.Travel("run");
        }
    }

    public void Hurt()
    {
        _stateMachine.Travel("hurt");
    }

    public void Die()
    {
        _stateMachine.Travel("death");
        SetPhysicsProcess(false);
    }
}
