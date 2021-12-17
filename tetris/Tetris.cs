using Godot;
using Core;

namespace TetrisGame
{
    enum TetrisState
    {
        INIT,
        GAME_START,
        GAME_OVER
    }

    public class Tetris : Node2D
    {
        private GameGrid _gameGrid;
        private Score _score;
        private Timer _YMovementTimer;
        private Timer _XMovementTimer;
        private Control _menu;
        private Block _block;
        private Block _nextBlock;
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
            _score = GetNode<Score>("CanvasLayer/Score");
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
            _score.ClearScore();
            _gameGrid.ClearGameField();
            _nextBlock = _blockBuilder.GetBlock();
            UpdateMovementSpeed();
            SpawnFigure();
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
            _block = _nextBlock;
            _nextBlock = _blockBuilder.GetBlock();
            _gameGrid.ShowNextFigure(_nextBlock);
            if (_gameGrid.IsFigureCanBeMoved(_block, _gameGrid.spawnPoint))
            {
                ResetPoint();
                ShowFigure();
                _YMovementTimer.Start();
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
            _currentPoint = _gameGrid.spawnPoint;
        }

        private void UpdateScore(int rowsCount)
        {
            if (rowsCount > 0)
            {
                _score.AddScore(rowsCount * rowsCount);
                UpdateMovementSpeed();
            }
        }

        private void UpdateMovementSpeed()
        {
            _YMovementTimer.WaitTime = 1 - _score.level * 0.09f;
        }

        private void TriggerTimerTimeout()
        {
            _YMovementTimer.Stop();
            OnYMovementTimerTimeout();
        }

        private void OnYMovementTimerTimeout()
        {
            if (_state != TetrisState.GAME_START)
                return;

            if (!MoveFigure(Vector2.Down))
            {
                var rowsCount = _gameGrid.RemoveRows();
                UpdateScore(rowsCount);
                SpawnFigure();
            }
            else
            {
                _YMovementTimer.Start();
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

        private void OnExitButtonPressed()
        {
            SceneManager.manager.ChangeScene(Scene.MAIN);
        }
    }
}
