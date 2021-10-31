using Godot;
using System;
using System.Collections.Generic;

public class Tetris : Node2D
{
    const int COLORS_TOTAL = 7;
    private TileMap _tilemap;
    private Timer _timer;
    private Block _block;
    private Vector2 _currentPoint;
    private BlocksBuilder _blockBuilder = new BlocksBuilder();
    private int _cellColor = 0;

    public override void _Ready()
    {
        _tilemap = GetNode<TileMap>("TileMap");
        _timer = GetNode<Timer>("Timer");
        Init();
    }

    private void Init()
    {
        SpawnFigure();
        _timer.Start();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_left"))
        {
            MoveFigure(Vector2.Left);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            MoveFigure(Vector2.Right);
        }
        if (Input.IsActionPressed("ui_down"))
        {
            TriggerTimerTimeout();
        }
        if (Input.IsActionJustPressed("ui_up"))
        {
            RotateFigure();
        }
    }

    private void ShowFigure()
    {
        foreach (var cell in _block.cells)
        {
            var point = _currentPoint + cell;
            _tilemap.SetCell((int)point.x, (int)point.y, _cellColor);
        }
    }

    private void HideFigure()
    {
        foreach (var cell in _block.cells)
        {
            var point = _currentPoint + cell;
            _tilemap.SetCell((int)point.x, (int)point.y, -1);
        }
    }

    private void SpawnFigure()
    {
        _block = _blockBuilder.GetBlock();
        ResetPoint();
        ShowFigure();
    }

    private bool MoveFigure(Vector2 direction)
    {
        var moved = false;
        HideFigure();
        var nextPoint = _currentPoint + direction;
        if (IsFigureCanBeMoved(_block, nextPoint))
        {
            _currentPoint = nextPoint;
            moved = true;
        }
        ShowFigure();
        return moved;
    }

    private bool RotateFigure()
    {
        var moved = false;
        var rotated = _block.Rotate();
        HideFigure();
        if (IsFigureCanBeMoved(rotated, _currentPoint))
        {
            _block = rotated;
            moved = true;
        }
        ShowFigure();
        return moved;
    }

    private bool IsFigureCanBeMoved(Block block, Vector2 point)
    {
        foreach (var cell in block.cells)
        {
            var tilemapCell = _tilemap.GetCellv(point + cell);
            if (tilemapCell != -1)
            {
                return false;
            }
        }
        return true;
    }

    private void ResetPoint()
    {
        _currentPoint = new Vector2(10, -1);
    }

    private void ChangeColor()
    {
        _cellColor++;
        _cellColor = _cellColor % COLORS_TOTAL;
    }

    private void TriggerTimerTimeout()
    {
        _timer.Stop();
        OnTimerTimeout();
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        if (!MoveFigure(Vector2.Down))
        {
            ChangeColor();
            SpawnFigure();
        }
    }
}
