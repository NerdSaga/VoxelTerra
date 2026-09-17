using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.Registries;

/// <summary>
/// An abstract registry class that as functions for getting registry items for objects
/// such as blocks that appear in the oxel terrain, and others. Classes that are
/// children of this class are AutoLoads.
/// </summary>
public abstract partial class VTRegistry : Node
{
    protected static VTRegistry instance;
    private List<VTRegistryItem> items = new();
    protected Dictionary<string, VTRegistryItem> itemsDict = new();
    protected VTRegistryItem[] itemsArray = {};
    protected string PREFIX = "vt_registry";

    /// <summary>
    /// Prints all items that are in this registry to the console. Used for debugging.
    /// </summary>
    public static void PrintItems()
    {
        foreach (VTRegistryItem item in instance.itemsArray)
        {
            GD.Print($"{item.ID}: {instance.PREFIX}:{item.ITEM_NAME}");
        }
    }

    /// <summary>
    /// This abstract function is where all items in this registry are registered.
    /// </summary>
    protected abstract void init();

    /// <summary>
    /// Adds a new item to this registry. The item should have a uniquely defined item name.
    /// </summary>
    /// <param name="item"></param>
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
    
    /// <summary>
    /// Sorts, indexes, and commits all items added from register() to itemsArray.
    /// </summary>
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