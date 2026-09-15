using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.Registries;

public abstract partial class VTRegistry : Node
{
    protected static VTRegistry instance;
    private List<VTRegistryItem> items = new();
    protected Dictionary<string, VTRegistryItem> itemsDict = new();
    protected VTRegistryItem[] itemsArray = {};
    protected string PREFIX = "vt_registry";

    public static void PrintItems()
    {
        foreach (VTRegistryItem item in instance.itemsArray)
        {
            GD.Print($"{item.ID}: {instance.PREFIX}:{item.ITEM_NAME}");
        }
    }

    protected abstract void init();
    protected void register(VTRegistryItem item)
    {
        items.Add(item);
        itemsDict[item.ITEM_NAME] = item;
    }


    class ItemComparer : IComparer<VTRegistryItem>
    {
        public int Compare(VTRegistryItem x, VTRegistryItem y)
        {
            return x.ITEM_NAME.CompareTo(y.ITEM_NAME);
        }
    }
    private ItemComparer itemComparer = new();
    private void commitItems()
    {
        // Sort the items.
        items.Sort(itemComparer);
        itemsArray = items.ToArray();

        for (int i = 0; i < itemsArray.Length; i++)
        {
            itemsArray[i].ID = i;
        }
    }

    public VTRegistryItem getItem(string itemName)
    {
        return itemsDict[itemName];
    }

    public VTRegistryItem getItem(int itemID)
    {
        return itemsArray[itemID];
    }
    // protected Dictionary<string, VTRegistryItem> itemsDict = new();
    // protected VTRegistryItem[] itemsArray;
    // protected string prefix = "registry_item";

    // protected abstract void init();

    // public abstract VTRegistryItem GetItem(string key);
    // public abstract VTRegistryItem GetItem(int index);

    // protected void commitItems()
    // {
    //     int index = 0;
    //     itemsArray = new VTRegistryItem[itemsDict.Count];
    //     foreach (var item in itemsDict)
    //     {
    //         item.Value.ID = index;
    //         itemsArray[index] = item.Value;
    //         index++;
    //     }
    // }

    // protected void register(string key, VTRegistryItem item)
    // {
    //     if (!itemsDict.TryAdd(key, item))
    //     {
    //         VTDebug.ErrorAbort("Attempted to register a duplicate item: " + key);
    //     }
    // }

    // public static void PrintItems()
    // {
    //     foreach (var item in instance.itemsDict)
    //     {
    //         GD.Print($"{item.Value.ID}: {instance.prefix}:{item.Key}");
    //     }
    // }


    public override void _EnterTree()
    {
        instance = this;
        init();
        commitItems();
    }


    public override void _ExitTree()
    {
    }
}