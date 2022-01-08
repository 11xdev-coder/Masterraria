using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public TileClass scriptedTileClass;
    public string tileName;
    public TileClass wallVariant;
    public Sprite[] tileSprites;
    public bool inBackGround = true;
    public bool naturallyPlaced = true;
    public TileClass tileDrop;
    public ItemClass.ToolType toolToBreak;
    public bool isStackable;

    void Start()
    {
        tileName = scriptedTileClass.tileName;
        wallVariant = scriptedTileClass.wallVariant;
        tileSprites = scriptedTileClass.tileSprites;
        inBackGround = scriptedTileClass.inBackGround;
        naturallyPlaced = scriptedTileClass.naturallyPlaced;
        tileDrop = scriptedTileClass.tileDrop;
        toolToBreak = scriptedTileClass.toolToBreak;
        isStackable = scriptedTileClass.isStackable;
    }
}
