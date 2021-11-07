using Godot;
using System;

struct GridCoords
{
    public GridCoords(int _x1, int _y1, int _x2, int _y2)
    {
        x1 = _x1;
        y1 = _y1;
        x2 = _x2;
        y2 = _y2;
    }

    public int x1 { get; }
    public int y1 { get; }
    public int x2 { get; }
    public int y2 { get; }
}

public class GameGrid : TileMap
{
    private GridCoords _gameFieldCoords = new GridCoords(0, -3, 13, 14);
    private GridCoords _nextBlockCoords = new GridCoords(20, 3, 24, 7);
    public readonly Vector2 spawnPoint = new Vector2(6, -2);
    private readonly Vector2 _nextSpawnPoint = new Vector2(22, 6);

    public void ClearGameField()
    {
        for (int y = _gameFieldCoords.y1; y <= _gameFieldCoords.y2; y++)
        {
            for (int x = _gameFieldCoords.x1; x <= _gameFieldCoords.x2; x++)
            {
                this.SetCell(x, y, -1);
            }
        }
    }

    public int RemoveRows()
    {
        var count = 0;
        for (int y = _gameFieldCoords.y2; y >= count; y--)
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
        return count;
    }

    public bool IsRowCompleted(int y)
    {
        for (int x = _gameFieldCoords.x1; x <= _gameFieldCoords.x2; x++)
        {
            var cell = this.GetCell(x, y);
            if (cell == -1)
            {
                return false;
            }
        }
        return true;
    }

    public void ShiftRow(int y, int count)
    {
        for (int x = _gameFieldCoords.x1; x <= _gameFieldCoords.x2; x++)
        {
            var cell = this.GetCell(x, y);
            this.SetCell(x, y + count, cell);
        }
    }

    public bool IsFigureCanBeMoved(Block block, Vector2 position)
    {
        foreach (var cell in block.cells)
        {
            var tilemapCell = this.GetCellv(position + cell);
            if (tilemapCell != -1)
            {
                return false;
            }
        }
        return true;
    }

    public void ShowFigure(Block block, Vector2 position)
    {
        foreach (var cell in block.cells)
        {
            var point = position + cell;
            this.SetCell((int)point.x, (int)point.y, (int)block.color);
        }
    }

    public void HideFigure(Block block, Vector2 position)
    {
        foreach (var cell in block.cells)
        {
            var point = position + cell;
            this.SetCell((int)point.x, (int)point.y, -1);
        }
    }

    public void ShowNextFigure(Block block)
    {
        HideNextFigure();
        foreach (var cell in block.cells)
        {
            var x = _nextSpawnPoint.x + cell.x;
            var y = _nextSpawnPoint.y + cell.y;
            this.SetCell((int)x, (int)y, (int)block.color);
        }
    }

    public void HideNextFigure()
    {
        for (int y = _nextBlockCoords.y1; y <= _nextBlockCoords.y2; y++)
        {
            for (int x = _nextBlockCoords.x1; x <= _nextBlockCoords.x2; x++)
            {
                this.SetCell(x, y, -1);
            }
        }
    }
}
