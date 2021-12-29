using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeClass
{
    public string biomeName;
    public Color biomeColor;
    public TileAtlas tileAtlas;

    [Header("Noise Settings")]
    public Texture2D caveNoiseTexture;
    public float caveFreq = 0.5f;
    public float terrainFreq = 0.5f;

    [Header("Generation Settings")]
    public int dirtLayerHeight = 5;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;

    [Header("Trees")]
    public int treeChance = 10;
    public int minTreeHeight = 7;
    public int maxTreeHeight = 21;

    [Header("Addons")]
    public int tallGrassChance = 10;

    [Header("Ore Settings")]
    public OreClass[] ores;
}
