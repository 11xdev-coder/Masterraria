using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass
{
    public enum ItemType
    {
        block,
        tool
    };

    public enum ToolType
    {
        sword,
        pickaxe,
        axe,
        hammer
    };

    public ItemType itemType;
    public ToolType toolType;

    public TileClass tile;
    public ToolClass tool;

    public string name;
    public Sprite sprite;
    public bool isStackable;

    public ItemClass(TileClass _tile)
    {
        name = _tile.name;
        sprite = _tile.tileSprites[0];
        isStackable = _tile.isStackable;
    }

    public ItemClass(ToolClass _tool)
    {
        name = _tool.name;
        sprite = _tool.sprite;
        isStackable = false;
    }
}
