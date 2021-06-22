using Godot;
using System;

public class Character : KinematicBody2D
{
    public Stats stats;

    private int _acceleration = 900;
    private int _maxSpeed = 400;
    private int _friction = 900;
    private int _gravity = 1000;
    private int _jumpForce = -500;
    private int _jumpCount = 0;
    private int _maxJumpCount = 2;
    private Vector2 _movement;

    private AnimatedSprite _animatedSprite;
    private Area2D _hurtbox;
    private AnimationNodeStateMachinePlayback _stateMachine;

    public override void _Ready()
    {
        _movement = new Vector2();
        stats = GetNode<Stats>("Stats");
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _hurtbox = GetNode<Area2D>("Hurtbox");
        var animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        _stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
    }

    public override void _Process(float delta)
    {
        if (stats.HP > 0)
        {
            Move(delta);
            RunAnimation();
        }
    }

    private void Move(float delta)
    {
        // var currentState = _stateMachine.GetCurrentNode();

        if (IsOnFloor())
        {
            _jumpCount = 0;
        }

        if (Input.IsActionJustPressed("ui_up") && _jumpCount < _maxJumpCount)
        {
            _movement.y = _jumpForce;
            _jumpCount++;
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
    }

    private void RunAnimation()
    {
        if (!stats.IsAlive())
        {
            _stateMachine.Travel("die");
        }
        else if (Input.IsActionJustPressed("attack"))
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
            var mod = _movement.x < 0 ? -1 : 1;
            _hurtbox.Position = new Vector2(Math.Abs(_hurtbox.Position.x) * mod, _hurtbox.Position.y);
            _animatedSprite.FlipH = _movement.x < 0;
        }
    }

    public void Hit(int damage)
    {
        stats.Hit(damage);

        if (!stats.IsAlive())
        {
            _stateMachine.Travel("die");
            SetPhysicsProcess(false);
        }
        else
        {
            _stateMachine.Travel("hit");
        }
    }

    public void Heal(int hp)
    {
        stats.Heal(hp);
    }

    private void OnHitboxHit(int damage)
    {
        Hit(damage);
    }

    private void OnHitboxAreaEntered(object area)
    {
        if (area is Potion)
        {
            if (!stats.IsMaxHP())
            {
                var potion = (Potion)area;
                potion.Use(this);
            }
        }
    }
}


