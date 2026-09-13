using System.Collections.Generic;
using System.Linq;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.Registries;

public abstract partial class VTRegistry : Node
{
    protected static VTRegistry instance;
    protected Dictionary<string, VTRegistryItem> items = new();
    protected string prefix = "registry_item";

    protected abstract void init();

    void indexItems()
    {
        items.Order();

    }

    protected void register(string key, VTRegistryItem item)
    {
        if (!items.TryAdd(key, item))
        {
            VTDebug.ErrorAbort("Attempted to register a duplicate item: " + key);
        }
    }

    public static void PrintItems()
    {
        foreach (var item in instance.items)
        {
            GD.Print($"{instance.prefix}:{item.Key}");
        }
    }


    public override void _EnterTree()
    {
        instance = this;
        init();
        items = items.OrderBy(x => x.Key).ToDictionary();
    }


    public override void _ExitTree()
    {
    }
}