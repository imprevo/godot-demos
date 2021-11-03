using Godot;
using System;
using System.Collections.Generic;

public class Tetris : Node2D
{
    const int COLORS_TOTAL = 7;
    private TileMap _tilemap;
    private Timer _YMovementTimer;
    private Timer _XMovementTimer;
    private Block _block;
    private Vector2 _currentPoint;
    private BlocksBuilder _blockBuilder = new BlocksBuilder();
    private int _cellColor = 0;
    private bool _canMove = true;

    private int xFrom = 1;
    private int xTo = 30;
    private int yFrom = 0;
    private int yTo = 17;

    public override void _Ready()
    {
        _tilemap = GetNode<TileMap>("TileMap");
        _YMovementTimer = GetNode<Timer>("YMovementTimer");
        _XMovementTimer = GetNode<Timer>("XMovementTimer");
        Init();
    }

    private void Init()
    {
        SpawnFigure();
        _YMovementTimer.Start();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_left"))
        {
            MoveFigureThrottled(Vector2.Left);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            MoveFigureThrottled(Vector2.Right);
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

    private void MoveFigureThrottled(Vector2 direction)
    {
        if (!_canMove)
            return;

        _canMove = false;
        _XMovementTimer.Start();

        MoveFigure(direction);
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
        _currentPoint = new Vector2(10, -2);
    }

    private void ChangeColor()
    {
        _cellColor++;
        _cellColor = _cellColor % COLORS_TOTAL;
    }

    private void RemoveRows()
    {
        var count = 0;
        for (int y = yTo; y >= count; y--)
        {
            if (IsRowCompleted(y))
            {
                count++;
            }
            else if (count != 0)
            {
                ShiftRow(y, count);
            }
        }
    }

    private bool IsRowCompleted(int y)
    {
        for (int x = xFrom; x <= xTo; x++)
        {
            var cell = _tilemap.GetCell(x, y);
            if (cell == -1)
            {
                return false;
            }
        }
        return true;
    }

    private void ShiftRow(int y, int count)
    {
        for (int x = xFrom; x <= xTo; x++)
        {
            var cell = _tilemap.GetCell(x, y);
            _tilemap.SetCell(x, y + count, cell);
        }
    }

    private void TriggerTimerTimeout()
    {
        _YMovementTimer.Stop();
        OnYMovementTimerTimeout();
        _YMovementTimer.Start();
    }

    private void OnYMovementTimerTimeout()
    {
        if (!MoveFigure(Vector2.Down))
        {
            RemoveRows();
            ChangeColor();
            SpawnFigure();
        }
    }

    private void OnXMovementTimerTimeout()
    {
        _canMove = true;
    }
}
