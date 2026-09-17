namespace VoxelTerra.Registries;

/// <summary>
/// A registry item with a unique name and ID.
/// </summary>
public class VTRegistryItem
{
    public int ID {get; set;}
    public string ITEM_NAME = "vt_registry_item";

    public VTRegistryItem(string itemName)
    {
        ITEM_NAME = itemName;
    }
}

/// <summary>
/// This interface enforces the ItemName static field to be added to each VTRegistryItem.
/// </summary>
public interface IVTRegistryItem
{
    public abstract static string ItemName {get;}
}