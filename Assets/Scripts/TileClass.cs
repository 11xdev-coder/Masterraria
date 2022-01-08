using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileClass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public TileClass wallVariant;
    //public Sprite tileSprite;
    public Sprite[] tileSprites;
    public bool inBackGround = true;
    public bool naturallyPlaced = true;
    public TileClass tileDrop;
    public ItemClass.ToolType toolToBreak;
    public bool isStackable;

    public static TileClass CreateInstance(TileClass tile, bool isNaturallyPlaced)
    {
        var thisTile = ScriptableObject.CreateInstance<TileClass>();
        thisTile.Init(tile, isNaturallyPlaced);
        return thisTile;
    }

    public void Init(TileClass tile, bool isNaturallyPlaced)
    {
        tileName = tile.tileName;
        wallVariant = tile.wallVariant;
        tileSprites = tile.tileSprites;
        tileDrop = tile.tileDrop;
        inBackGround = tile.inBackGround;
        toolToBreak = tile.toolToBreak;
        isStackable = tile.isStackable;
        naturallyPlaced = isNaturallyPlaced;
    }
}
