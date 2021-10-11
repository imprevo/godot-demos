using Godot;
using System;
using System.Collections.Generic;

public class Tetris : Node2D
{
    const int COLORS_TOTAL = 7;
    private TileMap _tilemap;
    private Timer _timer;
    private List<Vector2> _figure;
    private Vector2 _currentPoint;
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
        CreateFigure();
        ShowFigure();
        _timer.Start();
    }

    private void ShowFigure()
    {
        foreach (var cell in _figure)
        {
            _tilemap.SetCell((int)cell.x, (int)cell.y, _cellColor);
        }
    }

    private void HideFigure()
    {
        foreach (var cell in _figure)
        {
            _tilemap.SetCell((int)cell.x, (int)cell.y, -1);
        }
    }

    private void ResetPoint()
    {
        _currentPoint = new Vector2(10, -1);
    }

    private void MovePoint()
    {
        _currentPoint = _currentPoint + new Vector2(0, 1);
    }

    private void CreateFigure()
    {
        List<Vector2> res = new List<Vector2>(4);
        res.Add(_currentPoint + new Vector2(-1, -1));
        res.Add(_currentPoint + new Vector2(0, -1));
        res.Add(_currentPoint + new Vector2(0, 0));
        res.Add(_currentPoint + new Vector2(1, 0));
        _figure = res;
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
        }
        else
        {
            HideFigure();
            MovePoint();
        }
        CreateFigure();
        ShowFigure();
    }
}
