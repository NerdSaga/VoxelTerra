using Godot;

public static class VoxelTemplateData {
    
    public static readonly Vector3[] VERTEX =
    {
        // Top (+Y)
        // new Vector3(1, 1, 0),
        // new Vector3(1, 1, 1),
        // new Vector3(0, 1, 1),
        // new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 1),

        // Bottom (-Y)
        new Vector3(1, 0, 1),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, 0),
        
        // North (+X)
        new Vector3(1, 1, 1),
        new Vector3(1, 1, 0),
        new Vector3(1, 0, 1),
        new Vector3(1, 0, 0),
        
        // South (-X)
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 1),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 1),

        // East (+Z)
        new Vector3(0, 1, 1),
        new Vector3(1, 1, 1),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 1),

        // West (-Z)
        new Vector3(1, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 0),
    };

    public static readonly Vector3[] NORMAL =
    {
        new Vector3(0, 1, 0).Normalized(),
        new Vector3(0, 1, 0).Normalized(),
        new Vector3(0, 1, 0).Normalized(),
        new Vector3(0, 1, 0).Normalized(),

        new Vector3(0, -1, 0).Normalized(),
        new Vector3(0, -1, 0).Normalized(),
        new Vector3(0, -1, 0).Normalized(),
        new Vector3(0, -1, 0).Normalized(),

        new Vector3(1, 0, 0).Normalized(),
        new Vector3(1, 0, 0).Normalized(),
        new Vector3(1, 0, 0).Normalized(),
        new Vector3(1, 0, 0).Normalized(),

        new Vector3(-1, 0, 0).Normalized(),
        new Vector3(-1, 0, 0).Normalized(),
        new Vector3(-1, 0, 0).Normalized(),
        new Vector3(-1, 0, 0).Normalized(),

        new Vector3(0, 0, 1).Normalized(),
        new Vector3(0, 0, 1).Normalized(),
        new Vector3(0, 0, 1).Normalized(),
        new Vector3(0, 0, 1).Normalized(),

        new Vector3(0, 0, -1).Normalized(),
        new Vector3(0, 0, -1).Normalized(),
        new Vector3(0, 0, -1).Normalized(),
        new Vector3(0, 0, -1).Normalized(),
    };

    public static readonly Vector2[] UV =
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
    };

    public static readonly Color[] COLOR = {};

}