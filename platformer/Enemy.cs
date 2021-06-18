using Godot;
using System;

public class Enemy : KinematicBody2D
{
    private Health _health;

    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _health = new Health(3);
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public void Hit(int damage)
    {
        _health.Hit(damage);

        if (_health.value <= 0)
        {
            SetPhysicsProcess(false);
            _stateMachine.Travel("die");
        }
        else
        {
            _stateMachine.Travel("hit");
        }
    }

    private void OnAttack(Area2D area)
    {
        if (area.IsInGroup("hurtbox"))
        {
            Hit(1);
        }
    }
}
