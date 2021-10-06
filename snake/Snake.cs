using Godot;
using System;

public class Snake : Node2D
{
    private Node2D _balls;
    private Node2D _lastChild;
    private ControllableBall _head;
    [Export] private NodePath _navigationPath;
    private Navigation2D _navigation;

    public override void _Ready()
    {
        _balls = GetNode<Node2D>("balls");
        _head = GetNode<ControllableBall>("ControllableBall");
        _lastChild = _head;
        _navigation = GetNode<Navigation2D>(_navigationPath);
    }

    public void AttachBall(Ball ball)
    {
        ball.SetActive(true);
        ball.Follow(_lastChild);
        SnakeUtils.MoveChild(ball, _balls);
        _lastChild = ball;
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
