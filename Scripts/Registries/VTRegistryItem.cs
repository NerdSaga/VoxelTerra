namespace VoxelTerra.Registries;

public class VTRegistryItem
{
    public int ID {get; set;}
    public string ITEM_NAME = "vt_registry_item";

    public VTRegistryItem(string itemName)
    {
        ITEM_NAME = itemName;
    }
}

public interface IVTRegistryItem
{
    public abstract static string ItemName {get;}
}