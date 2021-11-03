using Godot;
using System;

public class GameGrid : TileMap
{
    private int xFrom = 1;
    private int xTo = 30;
    private int yFrom = -3;
    private int yTo = 17;

    public void ClearGameField()
    {
        for (int y = yFrom; y <= yTo; y++)
        {
            for (int x = xFrom; x <= xTo; x++)
            {
                this.SetCell(x, y, -1);
            }
        }
    }

    public void RemoveRows()
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

    public bool IsRowCompleted(int y)
    {
        for (int x = xFrom; x <= xTo; x++)
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
        for (int x = xFrom; x <= xTo; x++)
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

    public void ShowFigure(Block block, Vector2 position, int cellColor)
    {
        foreach (var cell in block.cells)
        {
            var point = position + cell;
            this.SetCell((int)point.x, (int)point.y, cellColor);
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
}
