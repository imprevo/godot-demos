using Godot;
using System;

public class Snake : Node2D
{
    private Node2D _balls;
    private Ball _lastChild;
    [Export] private NodePath _navigationPath;
    private Navigation2D _navigation;

    public override void _Ready()
    {
        _balls = GetNode<Node2D>("balls");
        _navigation = GetNode<Navigation2D>(_navigationPath);
        InitBalls();
    }

    public void AttachBall(Ball ball)
    {
        ball.SetActive(true);
        ball.Follow(GetLastChild());
        SnakeUtils.MoveChild(ball, _balls);
        _lastChild = ball;
    }

    private Ball GetLastChild()
    {
        if (_lastChild == null)
        {
            _lastChild = (Ball)_balls.GetChild(_balls.GetChildCount() - 1);
        }
        return _lastChild;
    }

    private void OnAreaEntered(object area)
    {
        if (area is BallTrigger)
        {
            var ball = ((BallTrigger)area).GetBall();
            AttachBall(ball);
            // TODO: position hotfix. find a better way
            ball.SetDeferred("global_position", ball.GlobalPosition);
        }
    }

    private void InitBalls()
    {
        var count = _balls.GetChildCount();

        for (int i = 0; i < count; i++)
        {
            var ball = (Ball)_balls.GetChild(i);
            ball._navigation = _navigation;
        }
    }
}
