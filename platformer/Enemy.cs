using Godot;
using System;

// TODO: use state
enum EnemyState
{
    IDLE,
    PATROL,
    ATTACK,
}

public class Enemy : KinematicBody2D
{
    public Stats stats;
    public Character player;

    private int _acceleration = 900;
    private int _maxSpeed = 200;
    private int _friction = 900;
    private int _gravity = 1000;
    private int target = 800;

    private bool _isAttacking = false;
    private Vector2 _movement;

    private Sprite _sprite;
    private Area2D _hurtbox;
    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _movement = new Vector2();
        stats = GetNode<Stats>("Stats");
        _sprite = GetNode<Sprite>("NightBorne");
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _hurtbox = GetNode<Area2D>("Hurtbox");
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public override void _Process(float delta)
    {
        if (stats.IsAlive())
        {
            Move(delta);
            RunAnimation();
        }
    }

    public void setPlayer(Character character)
    {
        player = character;
    }

    // TODO: reuse duplicated code
    private void Move(float delta)
    {
        _movement.y += _gravity * delta;

        var movementY = _movement.y;
        var distanceToPlayer = Position.DistanceTo(player.Position);

        if (player.stats.IsAlive() && distanceToPlayer < 300)
        {
            target = (int)player.Position.x;
            _isAttacking = distanceToPlayer < 100;
        }
        else
        {
            _isAttacking = false;
            if (Position.x < 300)
            {
                target = 800;
            }
            else if (Position.x > 800 || (int)Position.x == target)
            {
                target = 200;
            }
        }


        var inputX = Position.x < target ? 1 : -1;

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
    }

    // TODO: reuse duplicated code
    private void RunAnimation()
    {
        if (!stats.IsAlive())
        {
            _stateMachine.Travel("die");
        }
        else if (_isAttacking)
        {
            _stateMachine.Travel("attack");
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
            var mod = _movement.x < 0 ? -1 : 1;
            _hurtbox.Position = new Vector2(Math.Abs(_hurtbox.Position.x) * mod, _hurtbox.Position.y);
            _sprite.FlipH = _movement.x < 0;
        }
    }

    public void Hit(int damage)
    {
        stats.Hit(damage);

        if (!stats.IsAlive())
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
