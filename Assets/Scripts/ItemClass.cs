using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
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
        hammer,
        unbreakable
    };

    public ItemType itemType;
    public ToolType toolType;

    public TileClass tile;
    public ToolClass tool;

    public string itemName;
    public Sprite sprite;
    public bool isStackable;


    public ItemClass(TileClass _tile)
    {
        itemName = _tile.tileName;
        sprite = _tile.tileDrop.tileSprites[0];
        isStackable = _tile.isStackable;
        itemType = ItemType.block;
        tile = _tile;
    }

    public ItemClass(ToolClass _tool)
    {
        itemName = _tool.name;
        sprite = _tool.sprite;
        isStackable = false;
        itemType = ItemType.tool;
        toolType = _tool.toolType;
        tool = _tool;
    }
}
