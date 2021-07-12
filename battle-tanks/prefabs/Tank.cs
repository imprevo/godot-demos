using Godot;
using System;

public class Tank : KinematicBody2D
{
    private float _acceleration = 600;
    private float _rotationSpeed = 2;
    private float _maxSpeed = 300;
    private float _friction = 900;
    private Vector2 _movement;

    [Export] private NodePath _projectilesNodePath;
    private Position2D _projectileSpawnPoint;
    private PackedScene _projectile = GD.Load<PackedScene>("res://battle-tanks/prefabs/Projectile.tscn");
    private Timer _shootTimer;
    private bool _canShoot;

    private Sprite _bodySprite;
    private Sprite _turretSprite;

    public override void _Ready()
    {
        _movement = new Vector2();
        _bodySprite = GetNode<Sprite>("BodySprite");
        _turretSprite = GetNode<Sprite>("TurretSprite");
        _projectileSpawnPoint = GetNode<Position2D>("TurretSprite/ProjectileSpawnPoint");
        _shootTimer = GetNode<Timer>("ShootTimer");
        _canShoot = true;
    }

    public override void _Process(float delta)
    {
        var inputMovement = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        var inputRotation = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        var inputAttack = Input.IsActionPressed("left_mouse");
        var turretTarget = GetGlobalMousePosition();

        Rotate(inputMovement, delta);
        Move(inputRotation, delta);
        RotateTurret(turretTarget);
        if (inputAttack)
        {
            Shoot();
        }
    }

    public void Move(float input, float delta)
    {
        if (input == 0)
        {
            _movement = _movement.MoveToward(Vector2.Zero, _friction * delta);
        }
        else
        {
            var direction = new Vector2(0, input).Normalized().Rotated(Rotation);
            _movement = _movement.MoveToward(direction * _maxSpeed, _acceleration * delta);
        }

        _movement = MoveAndSlide(_movement);
    }

    public void Rotate(float input, float delta)
    {
        Rotation += input * _rotationSpeed * delta;
    }

    public void RotateTurret(Vector2 target)
    {
        if (target == null)
            return;

        _turretSprite.Rotation = GetAngleTo(target) + Mathf.Deg2Rad(90);
    }

    public void Shoot()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        _shootTimer.Start();

        var projectile = _projectile.Instance<Node2D>();
        projectile.Position = _projectileSpawnPoint.GlobalPosition;
        projectile.Rotation = _projectileSpawnPoint.GlobalRotation;
        var projectilesNode = GetNode<Node2D>(_projectilesNodePath);
        projectilesNode.AddChild(projectile);
    }

    private void OnShootTimeout()
    {
        _canShoot = true;
    }
}
