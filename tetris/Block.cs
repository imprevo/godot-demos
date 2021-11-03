using Godot;
using System;
using System.Collections.Generic;

public enum BlockColor
{
    GreenLight,
    GreenDark,
    RedLight,
    RedDark,
    Purple,
    GrayLight,
    GrayDark,
}

public class Block
{
    public BlockColor color;
    public List<Vector2> cells;

    public Block(List<Vector2> _cells, BlockColor _color)
    {
        cells = _cells;
        color = _color;
    }

    public Block Rotate()
    {
        return new Block(RotateClockwise(), color);
    }

    private List<Vector2> RotateClockwise()
    {
        var rotated = new List<Vector2>();
        foreach (var cell in cells)
        {
            rotated.Add(new Vector2(cell.y, -cell.x));
        }
        return rotated;
    }
}
