using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [Header("Item Settings")]
    public int stackLimit = 99;
    public ToolClass pickaxe;
    public ToolClass axe;
    public ToolClass hammer;
    public TileClass chest;
    public GameObject dropPrefab;
    public GameObject drop;
    public string assetsDataPath;
    public string directory;

    [Header("UI settings")]
    public Vector2 inventoryOffset;
    public Vector2 hotbarOffset;
    public Vector2 multiplier;

    [Header("UIs")]
    public GameObject inventoryUI;

    public GameObject hotbarUI;
    public GameObject inventorySlotPrefab;

    [Header("Inventory Settings")]
    public int inventoryWidth;
    public int inventoryHeight;
    public InventorySlot[,] inventorySlots;
    public GameObject[,] uiSlots;

    [Header("Hotbar settings")]
    public InventorySlot[] hotbarSlots;
    public GameObject[] hotbarUISlots;

    public ItemClass tileDropItem;


    private void Start()
    {
        assetsDataPath = Application.dataPath;
        directory = assetsDataPath + "/saves/world1/inventory.txt";
        
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        hotbarSlots = new InventorySlot[inventoryWidth];
        hotbarUISlots = new GameObject[inventoryWidth];

        SetupUI();
        UpdateInventoryUI();
        Add(new ItemClass(pickaxe), 1);
        Add(new ItemClass(axe), 1);
        Add(new ItemClass(hammer), 1);
        Add(new ItemClass(chest), 1);
    }
    
    public void SetupUI()
    {
        // setup inventory
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab,
                    inventoryUI.transform.GetChild(0).transform);

                inventorySlot.GetComponent<RectTransform>().localPosition = 
                    new Vector3((x * multiplier.x) + inventoryOffset.x, (y * multiplier.y) + inventoryOffset.y, -7);

                uiSlots[x,y] = inventorySlot;
                inventorySlots[x, y] = null;
            }
        }
        // setup hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            GameObject hotbarSlot = Instantiate(inventorySlotPrefab,
                hotbarUI.transform.GetChild(0).transform);

            hotbarSlot.GetComponent<RectTransform>().localPosition =
                new Vector3((x * multiplier.x) + hotbarOffset.x, hotbarOffset.y, -7);

            hotbarUISlots[x] = hotbarSlot;
            hotbarSlots[x] = null;
        }
    }

    public void UpdateInventoryUI()
    {
        // update inventory
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                if (inventorySlots[x, y] == null)
                {
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = "0";
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = false;
                }
                else
                {
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x, y].item.sprite;

                    if (inventorySlots[x, y].item.itemType == ItemClass.ItemType.block)
                    {
                        if (inventorySlots[x, y].item.tile.inBackGround)
                        {
                            uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color =
                                new Color(0.5f, 0.5f, 0.5f);
                        }
                        else
                        {
                            uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color =
                                Color.white;
                        }
                    }

                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = inventorySlots[x, y].quantity.ToString();
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = true;
                }
            }
        }
        // update hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            if (inventorySlots[x, inventoryHeight - 1] == null)
            {
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;

                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = "0";
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = false;
            }
            else
            {
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x, inventoryHeight - 1].item.sprite;

                if (inventorySlots[x, inventoryHeight - 1].item.itemType == ItemClass.ItemType.block)
                {
                    if (inventorySlots[x, inventoryHeight - 1].item.tile.inBackGround)
                    {
                        hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color =
                            new Color(0.5f, 0.5f, 0.5f);
                    }
                    else
                    {
                        hotbarUISlots[x].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color =
                            Color.white;
                    }
                }

                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = inventorySlots[x, inventoryHeight - 1].quantity.ToString();
                hotbarUISlots[x].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = true;
            }
        }
    }

    public bool Add(ItemClass item, int amount)
    {
        Vector2Int itemPos = Contains(item);
        bool added = false;
        if (itemPos != Vector2Int.one * -1)
        {
            //if (inventorySlots[itemPos.x, itemPos.y].quantity <= stackLimit)
            //{
            if (inventorySlots[itemPos.x, itemPos.y].quantity >= amount)
            {
                inventorySlots[itemPos.x, itemPos.y].quantity += amount;
                added = true;
            }
            //}
        }
        if(!added)
        {
            for (int y = inventoryHeight - 1; y >= 0; y--)
            {
                if (added)
                    break;
                for (int x = 0; x < inventoryWidth; x++)
                {
                    if (inventorySlots[x, y] == null)
                    {
                        inventorySlots[x, y] = new InventorySlot
                            (item, new Vector2Int(x, y), amount);
                        added = true;
                        break;
                    }
                }
            }
        }

        UpdateInventoryUI();
        return added;
    }

    public Vector2Int Contains(ItemClass item)
    {
        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] != null)
                {
                    if (inventorySlots[x, y].item.sprite == item.sprite)
                    {
                        if(item.isStackable && inventorySlots[x, y].quantity <= stackLimit)
                            return new Vector2Int(x, y);
                    }
                }
            }
        }

        return Vector2Int.one * -1;
    }

    // public void Drag()
    // {
    //     //Cursor.SetCursor(texture, Cursor.Auto);
    //     //Debug.Log(GetClosestSlot().GetItemName());
    //     if (isMovingItem)
    //     {
    //         EndItemMove();
    //     }
    //     else
    //     {
    //         BeginToDrag();
    //     }
    // }

    public void Drop(ItemClass item)
    {
        GameObject newTileDrop = Instantiate(dropPrefab, new Vector2(transform.position.x + -transform.localScale.x, transform.position.y + 0.5f), Quaternion.identity);
        newTileDrop.GetComponent<SpriteRenderer>().sprite =
            item.sprite;
        tileDropItem = item;
        newTileDrop.GetComponent<TileDropController>().item = tileDropItem;
        Remove(item, 1);
    }
    

    public bool Remove(ItemClass item, int amount)
    {
        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] != null)
                {
                    if (inventorySlots[x, y].item.itemName == item.itemName)
                    {
                        inventorySlots[x, y].quantity -= amount;
                        if (inventorySlots[x, y].quantity <= 0)
                        {
                            inventorySlots[x, y] = null;
                        }

                        UpdateInventoryUI();
                        return true;
                    }
                    
                }
            }
        }
        return false;
    }

}

