using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public PlayerController player;
    public CamController camera;
    public GameObject tileDrop;

    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;
    public float seed; 

    public BiomeClass[] biomes;

    [Header("Biomes")]
    public float biomeFreq;
    public Gradient biomeGradient;
    public Texture2D biomeMap;

    //[Header("Trees")]
    //public int treeChance = 10;
    //public int minTreeHeight = 7;
    //public int maxTreeHeight = 21;

    //[Header("Addons")]
    //public int tallGrassChance = 10;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int worldSize = 600;
    //public int dirtLayerHeight = 5;
    public bool generateCaves = true;
    //public float surfaceValue = 0.25f;
    //public float heightMultiplier = 4f;
    public int heightAddition = 25;

    [Header("Noise Settings")]
    public float caveFreq = 0.5f;
    public float terrainFreq = 0.5f;
    public Texture2D caveNoiseTexture;

    [Header("Ore Settings")]
    public OreClass[] ores;
    //public float copperRarity;
    //public float copperSize;
    //public float coalRarity, coalSize;
    //public float ironRarity, ironSize;
    //public float goldRarity, goldSize;

    //public Texture2D copperSpread;
    //public Texture2D coalSpread;
    //public Texture2D ironSpread;
    //public Texture2D goldSpread;

    private GameObject[] worldChunks;

    private List<Vector2> worldTiles = new List<Vector2>();
    private List<GameObject> worldTileObjects = new List<GameObject>();
    private List<TileClass> worldTileClasses = new List<TileClass>();

    private BiomeClass curBiome;
    private Color[] biomeCols;


    // CreateChunks() - создает чанки
    // DrawCavesAndOres() - рисуеты белый шум для пещер и руд
    // DrawTextures() - рисует белые шумы для отдельных биомов
    // DrawBiomeTexture() - рисует карту биомов по которой будут располагатся биомы
    // GetCurrentBiome() - получает текущий биом
    // GenerateTerrain() - генерирует мир
    // GenerateNoiseTextures() - генерирует белый шум
    // GenerateTree() - генерирует дерево
    // PlaceTile() - ставит блок

    private void OnValidate()
    {
        //biomeColors = new Color[biomes.Length];
        //for (int i = 0; i < biomeColors.Length; i++)
        //{
        //    biomeColors[i] = biomes[i].biomeColor;
        //}
        DrawTextures();
        DrawCavesAndOres();
    }

    public void Start()
    {
        // идем через все руды
        for (int i = 0; i < ores.Length; i++)
        {
            // инициализируем текстуры руд
            ores[i].spreadTexture = new Texture2D(worldSize, worldSize);
        }

        biomeCols = new Color[biomes.Length];
        for (int i = 0; i < biomes.Length; i++)
        {
            biomeCols[i] = biomes[i].biomeColor;
        }

        seed = Random.Range(-100, 1000000);

        //DrawTextures();
        DrawBiomeMap();
        DrawCavesAndOres();

        CreateChunks();
        GenerateTerrain();

        camera.Spawn(new Vector3(player.spawnPos.x, player.spawnPos.y, camera.transform.position.z));
        camera.worldSize = worldSize;
        player.Spawn();
    }

    private void Update()
    {
        RefreshChunks();
    }

    public void CreateChunks()
    {
        // кол-во чанков
        int numChunks = worldSize / chunkSize;
        // инициализируем список с кол-вом чанков
        worldChunks = new GameObject[numChunks];
        // идем через все чанки
        for(int c = 0; c < numChunks; c++)
        {
            // инициализируем чанк
            GameObject newChunk = new GameObject();
            // задаем чанку имя переменной с(номер чанка)
            newChunk.name = c.ToString();
            // делаем так чтобы чанк был ребенком этого объекта
            newChunk.transform.parent = this.transform;
            // создаем чанк
            worldChunks[c] = newChunk;
        }
    }

    public void DrawCavesAndOres()
    {
        // инициализируем изображение
        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        float v;
        float o;
        for (int x = 0; x < caveNoiseTexture.width; x++)
        {
            for (int y = 0; y < caveNoiseTexture.height; y++)
            {
                // получаем текущий биом
                curBiome = GetCurrentBiome(x, y);
                // создаем perlinNoise
                v = Mathf.PerlinNoise((x + seed) * caveFreq, (y + seed) * caveFreq);
                // если переменная v больше чем surfaceValue
                if (v > curBiome.surfaceValue)
                {
                    // то задаем белый цвет пикселю
                    caveNoiseTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    // если нет то задаем черный
                    caveNoiseTexture.SetPixel(x, y, Color.black);
                }

                for(int i = 0; i < ores.Length; i++)
                {
                    // изменяем пиксель спреда руды на черный
                    ores[i].spreadTexture.SetPixel(x, y, Color.black);
                    // если длина списка руд в текущем биоме превышает i + 1
                    if (curBiome.ores.Length >= i + 1)
                    {
                        // создаем белый шум
                        o = Mathf.PerlinNoise((x + seed) * curBiome.ores[i].frequency, (y + seed) * curBiome.ores[i].frequency);
                        // если переменная v(белый шум) больше размеров руды текущего биома
                        if (o > curBiome.ores[i].size) // то изменяем пиксель спреда руды на белый
                            ores[i].spreadTexture.SetPixel(x, y, Color.white);
                        // применяем изменения
                        ores[i].spreadTexture.Apply();
                    }
                }
            }       
        }
        // применяем изменения
        caveNoiseTexture.Apply();

        //for (int x = 0; x < worldSize; x++)
        //{
        //    for (int y = 0; y < worldSize; y++)
        //    {
        //        // получаем текущий биом
        //        curBiome = GetCurrentBiome(x, y);
        //        // проходимся по все рудам
        //        for (int i = 0; i < ores.Length; i++)
        //        {
        //            // изменяем пиксель спреда руды на черный
        //            ores[i].spreadTexture.SetPixel(x, y, Color.black);
        //            // если длина списка руд в текущем биоме превышает i + 1
        //            if (curBiome.ores.Length >= i + 1)
        //            {
        //                // создаем белый шум
        //                float v = Mathf.PerlinNoise((x + seed) * curBiome.ores[i].frequency, (y + seed) * curBiome.ores[i].frequency);
        //                // если переменная v(белый шум) больше размеров руды текущего биома
        //                if (v > curBiome.ores[i].size) // то изменяем пиксель спреда руды на белый
        //                    ores[i].spreadTexture.SetPixel(x, y, Color.white);
        //                // применяем изменения
        //                ores[i].spreadTexture.Apply();
        //            }
        //        }
        //    }
        //}

    }

    public void DrawTextures()
    {
        // инициализируем текстуру
        biomeMap = new Texture2D(worldSize, worldSize);
        // вызываем другую функцию
        //DrawBiomeTexture();
        // проходимся по всем биомам
        for (int b = 0; b < biomes.Length; b++)
        {
            // инициализируем caveNoiseTexture биомов
            biomes[b].caveNoiseTexture = new Texture2D(worldSize, worldSize);
            // проходимся по всем рудам биома
            for (int o = 0; o < biomes[b].ores.Length; o++)
            {
                // инициализируем spreadTexture руды в биоме
                biomes[b].ores[o].spreadTexture = new Texture2D(worldSize, worldSize);
                //biomes[b].ores[1].spreadTexture = new Texture2D(worldSize, worldSize);
                //biomes[b].ores[2].spreadTexture = new Texture2D(worldSize, worldSize);
                //biomes[b].ores[3].spreadTexture = new Texture2D(worldSize, worldSize);
                GenerateNoiseTextures(biomes[b].ores[o].frequency, biomes[b].ores[o].size, biomes[b].ores[o].spreadTexture);
            }

            // генерируем белый шум
            //GenerateNoiseTextures(biomes[b].caveFreq, biomes[b].surfaceValue, biomes[b].caveNoiseTexture);
            // Ores
            // проходимся по всем рудам биома
            //for (int o = 0; o < biomes[b].ores.Length; o++)
            //{
            //    // генерируем для руд белый шум
            //    GenerateNoiseTextures(biomes[b].ores[o].frequency, biomes[b].ores[o].size, biomes[b].ores[o].spreadTexture);
            //}
        }
    }

    //public void DrawBiomeTexture()
    //{
    //    for (int x = 0; x < biomeMap.width; x++)
    //    {
    //        for (int y = 0; y < biomeMap.height; y++)
    //        {
    //            // создаем белый шум
    //            float v = Mathf.PerlinNoise((x + seed) * biomeFreq, (y + seed) * biomeFreq);
    //            // вычисляем цвет градиента биомов
    //            Color col = biomeGradient.Evaluate(v);
    //            // ставим цвет пикселя на цвет который вычислили
    //            biomeMap.SetPixel(x, y, col);
                
    //        }
    //    }
    //    // применяем изменения
    //    biomeMap.Apply();
    //}
    
    public BiomeClass GetCurrentBiome(int x, int y)
    {
        // идем через все биомы
        //for (int i = 0; i < biomes.Length; i++)
        //{
        //    // проверяем, если цвет биома равен цвету этой же позиции на карте
        //    //Debug.Log("Biome Color " + biomes[i].biomeColor);
        //    //Debug.Log("Biome Map " + biomeMap.GetPixel(x, y));
        //    if (biomes[i].biomeColor == biomeMap.GetPixel(x, y))
        //    {
        //        // возвращаем класс биом
        //        return biomes[i];
        //    }
        //}
        //// если нет то возвращяем текущий биом
        //return curBiome;

        if (System.Array.IndexOf(biomeCols, biomeMap.GetPixel(x, y)) >= 0)
        {
            return biomes[System.Array.IndexOf(biomeCols, biomeMap.GetPixel(x, y))];
        }

        return curBiome;
    }

    public void GenerateTerrain()
    {
        // создаем список со спрайтами
        TileClass tileSprites;

        for (int x = 0; x < worldSize; x++)
        {
            // узнаем текущий биом
            curBiome = GetCurrentBiome(x, 0);
            // узнаем высоту террейна
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * curBiome.heightMultiplier + heightAddition;

            if (x == worldSize / 2)
            {
                player.spawnPos = new Vector2(x, height + 1);
            }

            for (int y = 0; y < height; y++)
            {
                // узнаем текущий биом
                curBiome = GetCurrentBiome(x, y);
                //height = Mathf.PerlinNoise((x + seed) * curBiome.terrainFreq, seed * curBiome.terrainFreq) * curBiome.heightMultiplier + heightAddition;
                //if (y >= height) {
                //    break;
                //}
                // если y под высотой уровня земли то
                if (y < height - curBiome.dirtLayerHeight)
                {
                    // изменяем спрайт на камень
                    tileSprites = curBiome.tileAtlas.stone;
                    // copper
                    // если цвет спреда руды(меди) больше 0.5 и не превышает высоту спавна
                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight)
                    {
                        // изменяем спрайт на медь
                        tileSprites = curBiome.tileAtlas.copper;
                    }
                    // coal
                    // если цвет спреда руды(угля) больше 0.5 и не превышает высоту спавна
                    else if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                    {
                        // изменяем спрайт на уголь
                        tileSprites = curBiome.tileAtlas.coal;
                    }
                    // iron
                    // если цвет спреда руды(железо) больше 0.5 и не превышает высоту спавна
                    else if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                    {
                        // изменяем спрайт на железо
                        tileSprites = tileAtlas.iron;
                    }
                    // gold
                    // если цвет спреда руды(золото) больше 0.5 и не превышает высоту спавна
                    else if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                    {
                        // изменяем спрайт на золото
                        tileSprites = tileAtlas.gold;
                    }
                }
                // иначе если y на уровне земли
                else if (y < height - 1)
                {
                    // меняем спрайт на землю
                    tileSprites = curBiome.tileAtlas.dirt;
                }
                // иначе
                else
                {
                    // top layer of terrains
                    // меняем спрайт на дерн
                    tileSprites = curBiome.tileAtlas.grass;
                }
                // надо ли генерировать пещеры?
                if (generateCaves)
                {
                    // если да то если цвет пикселя в caveNoiseTexture превышает 0.5 то
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        // ставим нужный нам спрайт
                        PlaceTile(tileSprites, x, y);
                    }
                    else
                    {
                        if(tileSprites.wallVariant != null)
                            PlaceTile(tileSprites.wallVariant, x, y);
                    }
                }
                else
                {
                    // иначе просто ставим нужный нам спрайт
                    PlaceTile(tileSprites, x, y);
                }
                // если мы выше террейна
                if (y >= height - 1)
                {
                    // то вычисляем шанс спавна дерева
                    int t = Random.Range(0, curBiome.treeChance);
                    // если шанс срабатывает
                    if (t == 1)
                    {
                        // generate tree
                        // если есть блок чтобы сгенерировать дерево
                        if (worldTiles.Contains(new Vector2(x, y)))
                        {
                            if (curBiome.biomeName == "Desert")
                            {
                                GenerateCactus(curBiome.tileAtlas, Random.Range(curBiome.minTreeHeight, curBiome.maxTreeHeight), x, y + 1);
                            }
                            else
                            {
                                // генерируем дерево
                                GenerateTree(Random.Range(curBiome.minTreeHeight, curBiome.maxTreeHeight), x, y + 1);
                            }
                        }
                    }
                    // иначе
                    else
                    {
                        // вычисляем шанс спавна высокой травы
                        int g = Random.Range(0, curBiome.tallGrassChance);
                        // если шанс срабатывает
                        if (g == 1)
                        {
                            // если есть блок чтобы сгенерировать траву
                            if (worldTiles.Contains(new Vector2(x, y)))
                            {
                                // и есть спрайт для травы
                                if (curBiome.tileAtlas.tallGrass != null)
                                    // генерируем траву
                                    PlaceTile(curBiome.tileAtlas.tallGrass, x, y + 1);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DrawBiomeMap()
    {
        float b;
        Color col;
        biomeMap = new Texture2D(worldSize, worldSize);
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                // создаем белый шум
                b = Mathf.PerlinNoise((x + seed) * biomeFreq, (y + seed) * biomeFreq);
                col = biomeGradient.Evaluate(b);
                biomeMap.SetPixel(x, y, col);
                
            }
        }

        biomeMap.Apply();
    }

    public void GenerateNoiseTextures(float freq, float limit, Texture2D noiseTexture)
    {
        float v;
        for(int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                // создаем белый шум
                v = Mathf.PerlinNoise((x + seed) * freq, (y + seed) * freq);

                if (v > limit)
                {
                    // красим пиксель в белый
                    noiseTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    // если нет то в черный
                    noiseTexture.SetPixel(x, y, Color.black);
                }               
            }
        }
        // применяем изменения
        noiseTexture.Apply();
    }

    void GenerateCactus(TileAtlas atlas, int treeHeight, int x, int y)
    {
        // define the tree
        for (int h = 0; h <= treeHeight; h++)
        {
            // ставим блок дерева
            PlaceTile(atlas.log, x, y + h);
        }
    }

    void GenerateTree(int treeHeight, int x, int y)
    {
        // define the tree
        for(int h = 0; h <= treeHeight; h++)
        {
            // ставим блок дерева
            PlaceTile(tileAtlas.log, x, y + h);
        }

        // generate leaves
        // ставим блоки листьев
        PlaceTile(tileAtlas.leaf, x, y + treeHeight);
        PlaceTile(tileAtlas.leaf, x, y + treeHeight + 1);
        PlaceTile(tileAtlas.leaf, x, y + treeHeight + 2);

        PlaceTile(tileAtlas.leaf, x - 1, y + treeHeight);
        PlaceTile(tileAtlas.leaf, x - 1, y + treeHeight + 1);

        PlaceTile(tileAtlas.leaf, x + 1, y + treeHeight);
        PlaceTile(tileAtlas.leaf, x + 1, y + treeHeight + 1);
    }

    public void RemoveTile(int x, int y)
    {
        if (worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            if (worldTileClasses[worldTiles.IndexOf(new Vector2(x, y))].wallVariant)
            {
               PlaceTile(worldTileClasses[worldTiles.IndexOf(new Vector2(x, y))].wallVariant, x ,y);
            }

            Destroy(worldTileObjects[worldTiles.IndexOf(new Vector2(x, y))]);

            if (worldTileClasses[worldTiles.IndexOf(new Vector2(x, y))].tileDrop)
            {
                GameObject newTileDrop = Instantiate(tileDrop, new Vector2(x, y + 0.5f), Quaternion.identity);
                newTileDrop.GetComponent<SpriteRenderer>().sprite =
                    worldTileClasses[worldTiles.IndexOf(new Vector2(x, y))].tileSprites[0];
            }

            worldTileObjects.RemoveAt(worldTiles.IndexOf(new Vector2(x, y)));
            worldTileClasses.RemoveAt(worldTiles.IndexOf(new Vector2(x, y)));
            worldTiles.RemoveAt(worldTiles.IndexOf(new Vector2(x, y)));
        }
    }

    public void RefreshChunks()
    {
        for (int i = 0; i < worldChunks.Length; i++)
        {
            if (Vector2.Distance(new Vector2((i * chunkSize) + (chunkSize / 2), 0), new Vector2(player.transform.position.x, 0)) >
                Camera.main.orthographicSize * 5f)
            {
                worldChunks[i].SetActive(false);
            }
            else
            {
                worldChunks[i].SetActive(true);
            }
        }
    }

    public void CheckTile(TileClass tile, int x, int y)
    {
        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            if (!worldTiles.Contains(new Vector2Int(x, y)))
            {
                PlaceTile(tile, x, y);
            }
            else
            {
                if (worldTileClasses[worldTiles.IndexOf(new Vector2Int(x, y))].inBackGround)
                {
                    RemoveTile(x, y);
                    PlaceTile(tile, x, y);
                }
            }
        }
    }

    public void PlaceTile(TileClass tile, int x, int y)
    {
        bool backgroundElement = tile.inBackGround;
        // проверка нету ли этого блока уже
        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            // инициализируем блок
            GameObject newTile = new GameObject();
            // получаем координаты чанка
            int chunkCoord = Mathf.RoundToInt(Mathf.Round(x / chunkSize) * chunkSize);
            // делим
            chunkCoord /= chunkSize;
            // делаем блок ребенком чанка
            newTile.transform.parent = worldChunks[chunkCoord].transform;
            // добавляем компонент
            newTile.AddComponent<SpriteRenderer>();
            if (!backgroundElement)
            {
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            // получаем спрайт индекс
            int spriteIndex = Random.Range(0, tile.tileSprites.Length);
            // меняем спрайт
            newTile.GetComponent<SpriteRenderer>().sprite = tile.tileSprites[spriteIndex];
            if (tile.inBackGround)
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = -10;
            }
            else
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = -5;
            }

            if (tile.name.ToUpper().Contains("WALL"))
            {
                newTile.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            }

            // меняем имя
            newTile.name = tile.tileSprites[0].name;
            // задаем нужную позицию
            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
            // добавляем в список
            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
            worldTileObjects.Add(newTile);
            worldTileClasses.Add(tile);
            
        }
    }
}
