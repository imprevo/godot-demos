using Godot;
using System;
using System.Collections.Generic;

public class BlocksBuilder : Node2D
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private List<List<Vector2>> blocks = new List<List<Vector2>>();

    public BlocksBuilder()
    {
        rng.Randomize();
        blocks.Add(CreateBlockZ());
        blocks.Add(CreateBlockT());
        blocks.Add(CreateBlockI());
        blocks.Add(CreateBlockL());
        blocks.Add(CreateBlockO());
    }

    public List<Vector2> GetBlock()
    {
        var index = rng.RandiRange(0, blocks.Count - 1);
        return blocks[index];
    }

    private List<Vector2> CreateBlockZ()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(-1, -1));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return block;
    }

    private List<Vector2> CreateBlockT()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(-1, -1));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(1, -1));
        block.Add(new Vector2(0, 0));
        return block;
    }

    private List<Vector2> CreateBlockI()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -3));
        block.Add(new Vector2(0, -2));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        return block;
    }

    private List<Vector2> CreateBlockL()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -2));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return block;
    }

    private List<Vector2> CreateBlockO()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(1, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return block;
    }
}
