using Godot;

public class ActiveTileMap : TileMap
{
    [Export]
    private NodePath baseTileMapPath;
    private Vector2 activeTile;
    private TileMap baseTileMap;

    public override void _Ready()
    {
        baseTileMap = GetNode<TileMap>(baseTileMapPath);
    }

    public void SetActiveTile(Vector2 tile)
    {
        var cell = baseTileMap.GetCell((int)tile.x, (int)tile.y);
        SetCell((int)tile.x, (int)tile.y, cell);
        activeTile = tile;
    }

    public void ResetActiveTile()
    {
        SetCell((int)activeTile.x, (int)activeTile.y, -1);
        activeTile = Vector2.Inf;
    }
}
