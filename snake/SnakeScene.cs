using Godot;
using System;

public class SnakeScene : Node2D
{
    private PackedScene _ballScene = GD.Load<PackedScene>("res://snake/Ball.tscn");
    private Snake _snake;

    public override void _Ready()
    {
        _snake = GetNode<Snake>("Snake");
        GenerateBall(true);
        GenerateBall(true);
        GenerateBall(false);
    }

    public override void _Process(float delta)
    {
    }

    private void GenerateBall(bool isAttach)
    {
        var ball = _ballScene.Instance<Ball>();
        ball.Init();
        AddChild(ball);
        if (isAttach)
        {
            _snake.AttachBall(ball);
        }
        ball.GlobalPosition = GenerateNextCoordinate();
    }

    private Vector2 GenerateNextCoordinate()
    {
        var wpSize = GetViewport().Size;
        SnakeUtils.rng.Randomize();
        return new Vector2(
            SnakeUtils.rng.RandiRange(50, (int)wpSize.x - 50),
            SnakeUtils.rng.RandiRange(50, (int)wpSize.y - 50)
        );
    }

}
