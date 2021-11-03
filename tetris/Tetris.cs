using Godot;
using System;
using System.Collections.Generic;

enum TetrisState
{
    INIT,
    GAME_START,
    GAME_OVER
}

public class Tetris : Node2D
{
    private GameGrid _gameGrid;
    private Timer _YMovementTimer;
    private Timer _XMovementTimer;
    private Control _menu;
    private Block _block;
    private Vector2 _spawnPoint = new Vector2(10, -2);
    private Vector2 _currentPoint;
    private BlocksBuilder _blockBuilder = new BlocksBuilder();
    private bool _canMove = true;
    private TetrisState _state = TetrisState.INIT;

    public override void _Ready()
    {
        _gameGrid = GetNode<GameGrid>("GameGrid");
        _YMovementTimer = GetNode<Timer>("YMovementTimer");
        _XMovementTimer = GetNode<Timer>("XMovementTimer");
        _menu = GetNode<Control>("CanvasLayer/Menu");
        _menu.Show();
    }

    public override void _Process(float delta)
    {
        if (_state == TetrisState.GAME_START)
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
    }

    private void GameStart()
    {
        _state = TetrisState.GAME_START;
        _gameGrid.ClearGameField();
        SpawnFigure();
        _YMovementTimer.Start();
        _menu.Hide();
    }

    private void GameOver()
    {
        _state = TetrisState.GAME_OVER;
        _YMovementTimer.Stop();
        _menu.Show();
    }

    private void ShowFigure()
    {
        _gameGrid.ShowFigure(_block, _currentPoint);
    }

    private void HideFigure()
    {
        _gameGrid.HideFigure(_block, _currentPoint);
    }

    private void SpawnFigure()
    {
        _block = _blockBuilder.GetBlock();
        if (_gameGrid.IsFigureCanBeMoved(_block, _spawnPoint))
        {
            ResetPoint();
            ShowFigure();
        }
        else
        {
            GameOver();
        }
    }

    private bool MoveFigure(Vector2 direction)
    {
        var moved = false;
        HideFigure();
        var nextPoint = _currentPoint + direction;
        if (_gameGrid.IsFigureCanBeMoved(_block, nextPoint))
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
        if (_gameGrid.IsFigureCanBeMoved(rotated, _currentPoint))
        {
            _block = rotated;
            moved = true;
        }
        ShowFigure();
        return moved;
    }

    private void ResetPoint()
    {
        _currentPoint = _spawnPoint;
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
            _gameGrid.RemoveRows();
            SpawnFigure();
        }
    }

    private void OnXMovementTimerTimeout()
    {
        _canMove = true;
    }

    private void OnPlayButtonPressed()
    {
        GameStart();
    }
}
