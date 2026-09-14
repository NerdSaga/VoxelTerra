using Godot;

namespace VoxelTerra.Registries;

public class BlockVariant
{
    /// <summary>
    /// Used by block faces to map textures to block atlas texture tiles. This should remain as six elements.
    /// </summary>
    private Vector2I[] faceAtlasTilePositions =
    {
        new Vector2I(0, 0), // Top (+Y)
        new Vector2I(0, 0), // Bottom (-Y)
        new Vector2I(0, 0), // North (+X)
        new Vector2I(0, 0), // South (-X)
        new Vector2I(0, 0), // East (+Z)
        new Vector2I(0, 0), // West (-Z)
    };

    public BlockVariant SetFaceAtlasTilePositions(Vector2I top, Vector2I bottom, Vector2I north, Vector2I south, Vector2I east, Vector2I west)
    {
        this.faceAtlasTilePositions = new Vector2I[]
        {
            top,
            bottom,
            north,
            south,
            east,
            west
        };

        return this;
    }
}