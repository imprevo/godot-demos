using Godot;
using System;
using System.Collections.Generic;

public class Tetris : Node2D
{
    const int COLORS_TOTAL = 7;
    private TileMap _tilemap;
    private Timer _timer;
    private List<Vector2> _block;
    private Vector2 _currentPoint;
    private BlocksBuilder _blockBuilder = new BlocksBuilder();
    private Vector2 _worldsSize = new Vector2(20, 16);
    private int _cellColor = 0;

    public override void _Ready()
    {
        _tilemap = GetNode<TileMap>("TileMap");
        _timer = GetNode<Timer>("Timer");
        Init();
    }

    private void Init()
    {
        ResetPoint();
        SpawnFigure();
        ShowFigure();
        _timer.Start();
    }

    private void ShowFigure()
    {
        foreach (var cell in _block)
        {
            var point = _currentPoint + cell;
            _tilemap.SetCell((int)point.x, (int)point.y, _cellColor);
        }
    }

    private void HideFigure()
    {
        foreach (var cell in _block)
        {
            var point = _currentPoint + cell;
            _tilemap.SetCell((int)point.x, (int)point.y, -1);
        }
    }
    private void SpawnFigure()
    {
        _block = _blockBuilder.GetBlock();
    }

    private void ResetPoint()
    {
        _currentPoint = new Vector2(10, -1);
    }

    private void MovePoint()
    {
        _currentPoint = _currentPoint + new Vector2(0, 1);
    }

    private void ChangeColor()
    {
        _cellColor++;
        _cellColor = _cellColor % COLORS_TOTAL;
    }

    private void OnTimerTimeout()
    {
        if (_currentPoint.y > _worldsSize.y)
        {
            ResetPoint();
            ChangeColor();
            SpawnFigure();
        }
        else
        {
            HideFigure();
            MovePoint();
        }
        ShowFigure();
    }
}
