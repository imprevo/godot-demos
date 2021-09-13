using Godot;
using System;

public class Snake : Node2D
{
    public override void _Ready() { }

    public void AttachBall(Ball ball)
    {
        GD.PrintS(ball);
        ball.SetActive(true);
        ball.Follow(GetLastChild());
        SnakeUtils.MoveChild(ball, this);
    }

    private Ball GetLastChild()
    {
        return (Ball)GetChild(GetChildCount() - 1);
    }
}
