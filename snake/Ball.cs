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
    [Export] private float _maxSpeed = 200;
    [Export] private float _slowRadius = 100;
    [Export] private float _mass = 2;
    [Export] private float _followOffset = 50;
    [Export] public bool _active = false;
    [Export] private BallSpriteType _spriteType;
    private AnimatedSprite _animatedSprite;
    private CollisionShape2D _collisionShape;
    private Node2D _target;
    private Vector2 _velocity = Vector2.Zero;
    private Vector2[] _path = new Vector2[0];
    private int _pathIndex = 0;
    public Navigation2D _navigation;
    private Line2D _pathLine;
    private Timer _timer;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _animatedSprite.Animation = _spriteType.ToString();
        _animatedSprite.Play();
        _collisionShape = GetNode<CollisionShape2D>("Trigger/CollisionShape2D");
        _timer = GetNode<Timer>("Timer");
        _pathLine = GetNode<Line2D>("PathLine");
        SetActive(_active);
        if (_targetPath != null)
        {
            _target = GetNode<Node2D>(_targetPath);
        }
    }

    public override void _Process(float delta)
    {
        if (!_active)
            return;

        MoveToTarget();
    }

    public void Init()
    {
        _spriteType = SnakeUtils.GetRandomEnumValue<BallSpriteType>();
    }

    public void SetActive(bool active)
    {
        _active = active;
        _collisionShape.SetDeferred("disabled", active);
        SetProcess(active);
        if (active)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
        }
    }

    public void Follow(Node2D target)
    {
        _target = target;
    }

    public Vector2[] GetNextPosition()
    {
        return _navigation.GetSimplePath(GlobalPosition, _target.GlobalPosition, false);
    }

    private void MoveToTarget()
    {
        _pathLine.GlobalPosition = Vector2.Zero;

        if (_path.Length == 0 || _path.Length <= _pathIndex)
            return;

        var nextPosition = _path[_pathIndex];

        if (GlobalPosition.DistanceTo(nextPosition) < 10)
        {
            _pathIndex++;
        }
        else
        {
            var lastPosition = _path[_path.Length - 1];
            var followPosition = GlobalPosition.DistanceTo(lastPosition) > _followOffset
                ? nextPosition + (nextPosition - GlobalPosition).Normalized() * _followOffset
                : GlobalPosition;
            _velocity = SnakeUtils.Follow(_velocity, GlobalPosition, followPosition, _maxSpeed, _mass);
            _velocity = MoveAndSlide(_velocity);
            _animatedSprite.Rotation = _velocity.Angle();
        }
    }

    private void OnTimerTimeout()
    {
        if (!_active)
            return;
        _path = GetNextPosition();
        _pathLine.Points = _path;
        _pathIndex = 0;
    }
}
