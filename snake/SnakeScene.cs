using Godot;
using System;

public class SnakeScene : Node2D
{
    private PackedScene _ballScene = GD.Load<PackedScene>("res://snake/Ball.tscn");
    private Snake _snake;

    public override void _Ready()
    {
        _snake = GetNode<Snake>("Snake");
        GenerateBall();
    }

    public override void _Process(float delta)
    {
    }

    private void GenerateBall()
    {
        var ball = _ballScene.Instance<Ball>();
        ball.Position = GenerateNextCoordinate();
        ball.Init();
        AddChild(ball);
        _snake.AttachBall(ball);
    }

    private Vector2 GenerateNextCoordinate()
    {
        // TODO: random
        return new Vector2(100, 200);
    }

}
