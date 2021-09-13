using Godot;
using System;

public enum BallSpriteType
{
    AQUA,
    GREEN,
    PINK,
    PURPLE,
    RED,
    WHITE,
    YELLOW,
}

public class Ball : KinematicBody2D
{
    [Export] private NodePath _targetPath;
    [Export] private float _maxSpeed = 500;
    [Export] private float _slowRadius = 100;
    [Export] private float _mass = 2;
    [Export] private float _followOffset = 70;
    [Export] private bool _active = false;
    [Export] private BallSpriteType _spriteType;
    private AnimatedSprite _animatedSprite;
    private Node2D _target;
    private Vector2 _velocity = Vector2.Zero;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _animatedSprite.Animation = _spriteType.ToString();
        _animatedSprite.Play();
        if (_targetPath != null)
        {
            _target = GetNode<Node2D>(_targetPath);
        }
    }

    public override void _Process(float delta)
    {
        if (!_active)
            return;
        var targetPosition = _target == null
            ? GetGlobalMousePosition()
            : _target.GlobalPosition;
        var distanceToTarget = GlobalPosition.DistanceTo(targetPosition);
        var followPosition = distanceToTarget > _followOffset
            ? targetPosition - (targetPosition - GlobalPosition).Normalized() * _followOffset
            : GlobalPosition;
        _velocity = SnakeUtils.ArriveTo(_velocity, GlobalPosition, followPosition, _maxSpeed, _slowRadius, _mass);
        _velocity = MoveAndSlide(_velocity);
        _animatedSprite.Rotation = _velocity.Angle();
    }

    public void Init()
    {
        // TODO: random
        _spriteType = BallSpriteType.PINK;
    }

    public void SetActive(bool active)
    {
        _active = active;
    }

    public void Follow(Node2D target)
    {
        _target = target;
    }
}
