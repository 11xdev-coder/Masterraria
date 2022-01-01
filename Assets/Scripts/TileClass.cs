using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileClass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public TileClass wallVariant;
    //public Sprite tileSprite;
    public Sprite[] tileSprites;
    public bool inBackGround = true;
    public bool tileDrop = true;
}
