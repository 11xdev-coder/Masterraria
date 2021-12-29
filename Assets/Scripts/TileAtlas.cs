using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Atlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    [Header("Environment")]
    public TileClass grass;
    public TileClass dirt;
    public TileClass stone;
    public TileClass log;
    public TileClass leaf;
    public TileClass snow;
    public TileClass sand;
    public TileClass mud;
    public TileClass cactus;

    [Header("Ores")]
    public TileClass copper;
    public TileClass coal;
    public TileClass iron;
    public TileClass gold;

    [Header("Addons")]
    public TileClass tallGrass;
}
