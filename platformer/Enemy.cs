using Godot;
using System;

public class Enemy : KinematicBody2D
{
    private Stats _stats;

    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _stats = GetNode<Stats>("Stats");
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public override void _Process(float delta)
    {
        // _stateMachine.Travel("attack");
    }

    public void Hit(int damage)
    {
        _stats.Hit(damage);

        if (_stats.health <= 0)
        {
            SetPhysicsProcess(false);
            _stateMachine.Travel("die");
        }
        else
        {
            _stateMachine.Travel("hit");
        }
    }

    private void OnHitboxHit(int damage)
    {
        Hit(damage);
    }
}
