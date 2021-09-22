using Godot;
using System;

public class Snake : Node2D
{
    private Node2D _balls;
    private Ball _lastChild;

    public override void _Ready()
    {
        _balls = GetNode<Node2D>("balls");
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
}
