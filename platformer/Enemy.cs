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
            // TODO: try to use timer as a workaround
            // see issue https://github.com/godotengine/godot/issues/28311
            var shape = GetNode<CollisionShape2D>("CollisionShape2D");
            shape.SetDeferred("disabled", true);
            var hitbox = GetNode<CollisionShape2D>("Hitbox/CollisionShape2D");
            hitbox.SetDeferred("disabled", true);

            SetPhysicsProcess(false);
            _stateMachine.Travel("die");
        }
        else
        {
            _stateMachine.Travel("hit");
        }
    }

    private void OnAnimationFinished(String animationName)
    {
        // TODO: try to use timer as a workaround
        // see issue https://github.com/godotengine/godot/issues/28311
        GD.Print(animationName);
        if (animationName == "die")
        {
            QueueFree();
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
