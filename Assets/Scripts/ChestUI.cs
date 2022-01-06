using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestUI : MonoBehaviour
{
    [Header("Item Settings")]
    public int stackLimit = 99;

    [Header("UI settings")]
    public Vector2 chestOffset;
    public Vector2 multiplier;

    [Header("UIs")]
    public GameObject chestUI;
    public GameObject inventorySlotPrefab;

    [Header("Inventory Settings")]
    public int chestWidth;
    public int chestHeight;
    public InventorySlot[,] chestSlots;
    public GameObject[,] uiSlots;

    public Inventory inventory;
    private void Start()
    {
        chestSlots = new InventorySlot[chestWidth, chestHeight];
        uiSlots = new GameObject[chestWidth, chestHeight];

        SetupUI();
        UpdateInventoryUI();
    }
    public void SetupUI()
    {
        // setup inventory
        for (int x = 0; x < chestWidth; x++)
        {
            for (int y = 0; y < chestHeight; y++)
            {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab,
                    chestUI.transform.GetChild(0).transform);

                inventorySlot.GetComponent<RectTransform>().localPosition = 
                    new Vector3((x * multiplier.x) + chestOffset.x, (y * multiplier.y) + chestOffset.y, -7);

                uiSlots[x,y] = inventorySlot;
                chestSlots[x, y] = null;
            }
        }
    }

    public void UpdateInventoryUI()
    {
        // update inventory
        for (int x = 0; x < chestWidth; x++)
        {
            for (int y = 0; y < chestHeight; y++)
            {
                if (chestSlots[x, y] == null)
                {
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = "0";
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = false;
                }
                else
                {
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = chestSlots[x, y].item.sprite;

                    if (chestSlots[x, y].item.itemType == ItemClass.ItemType.block)
                    {
                        if (chestSlots[x, y].item.tile.inBackGround)
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

                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = chestSlots[x, y].quantity.ToString();
                    uiSlots[x, y].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().enabled = true;
                }
            }
        }
    }

    public ItemClass ItemToPutEnd(int amount)
    {
        for (int y = inventory.inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = inventory.inventoryWidth - 1; x >= 0; x--)
            {
                if (inventory.inventorySlots[x, y] != null)
                {
                    ItemClass toPut = inventory.inventorySlots[x, y].item;
                    if (inventory.inventorySlots[x, y].quantity >= amount)
                    {
                        inventory.Remove(inventory.inventorySlots[x, y].item, amount);
                        Put(toPut, amount);
                        return toPut;
                    }
                    else
                    {
                        inventory.Remove(inventory.inventorySlots[x, y].item, 1);
                        Put(toPut, 1);
                        return toPut;
                    }
                }
            }
        }

        return null;
    }
    
    public ItemClass ItemToLootEnd(int amount)
    {
        for (int y = chestHeight - 1; y >= 0; y--)
        {
            for (int x = chestWidth - 1; x >= 0; x--)
            {
                if (chestSlots[x, y] != null)
                {
                    ItemClass toLoot = chestSlots[x, y].item;
                    if (chestSlots[x, y].quantity >= amount)
                    {
                        inventory.Add(chestSlots[x, y].item, amount);
                        Loot(toLoot, amount);
                        return toLoot;
                    }
                    else
                    {
                        inventory.Add(chestSlots[x, y].item, 1);
                        Loot(toLoot, 1);
                        return toLoot;
                    }
                }
            }
        }

        return null;
    }
    
    public ItemClass ItemToPutStart(int amount)
    {
        for (int y = 0; y < inventory.inventoryHeight; y++)
        {
            for (int x = 0; x < inventory.inventoryWidth; x++)
            {
                if (inventory.inventorySlots[x, y] != null)
                {
                    ItemClass toPut = inventory.inventorySlots[x, y].item;
                    if (inventory.inventorySlots[x, y].quantity >= amount)
                    {
                        inventory.Remove(inventory.inventorySlots[x, y].item, amount);
                        Put(toPut, amount);
                        return toPut;
                    }
                    else
                    {
                        inventory.Remove(inventory.inventorySlots[x, y].item, 1);
                        Put(toPut, 1);
                        return toPut;
                    }
                }
            }
        }

        return null;
    }
    
    public ItemClass ItemToLootStart(int amount)
    {
        for (int y = 0; y < chestHeight; y++)
        {
            for (int x = 0; x < chestWidth; x++)
            {
                if (chestSlots[x, y] != null)
                {
                    ItemClass toLoot = chestSlots[x, y].item;
                    if (chestSlots[x, y].quantity >= amount)
                    {
                        inventory.Add(chestSlots[x, y].item, amount);
                        Loot(toLoot, amount);
                        return toLoot;
                    }
                    else
                    {
                        inventory.Add(chestSlots[x, y].item, 1);
                        Loot(toLoot, 1);
                        return toLoot;
                    }
                }
            }
        }

        return null;
    }

    public void PutAll()
    {
        for (int y = 0; y < inventory.inventoryHeight; y++)
        {
            for (int x = 0; x < inventory.inventoryWidth; x++)
            {
                if (inventory.inventorySlots[x, y] != null)
                {
                    ItemClass toPut = inventory.inventorySlots[x, y].item;
                    Put(toPut, inventory.inventorySlots[x, y].quantity);
                    inventory.Remove(inventory.inventorySlots[x, y].item, inventory.inventorySlots[x, y].quantity);
                }
            }
        }
    }
    
    public void LootAll()
    {
        for (int y = 0; y < chestHeight; y++)
        {
            for (int x = 0; x < chestWidth; x++)
            {
                if (chestSlots[x, y] != null)
                {
                    ItemClass toLoot = chestSlots[x, y].item;
                    inventory.Add(chestSlots[x, y].item, chestSlots[x, y].quantity);
                    Loot(toLoot, chestSlots[x, y].quantity);
                }
            }
        }
    }
    
    #region putButtons
    public void PutEndPressed()
    {
        ItemToPutEnd(1);
    }

    public void PutStartPressed()
    {
        ItemToPutStart(1);
    }
    
    public void PutEnd15Pressed()
    {
        ItemToPutEnd(15);;
    }

    public void PutStart15Pressed()
    {
        ItemToPutStart(15);
    }
    
    public void PutEnd100Pressed()
    {
        ItemToPutEnd(100);;
    }

    public void PutStart100Pressed()
    {
        ItemToPutStart(100);
    }
    #endregion
    
    #region lootButtons
    public void LootEndPressed()
    {
        ItemToLootEnd(1);
    }

    public void LootStartPressed()
    {
        ItemToLootStart(1);
    }
    
    public void LootEnd15Pressed()
    {
        ItemToLootEnd(15);;
    }

    public void LootStart15Pressed()
    {
        ItemToLootStart(15);
    }
    
    public void LootEnd100Pressed()
    {
        ItemToLootEnd(100);;
    }

    public void LootStart100Pressed()
    {
        ItemToLootStart(100);
    }
    #endregion

    
    public bool Put(ItemClass item, int amount)
    {
        Vector2Int itemPos = Contains(item);
        bool added = false;
        if (itemPos != Vector2Int.one * -1)
        {
            //if (inventorySlots[itemPos.x, itemPos.y].quantity <= stackLimit)
            //{
            chestSlots[itemPos.x, itemPos.y].quantity += amount;
            added = true;
            //}
        }
        if(!added)
        {
            for (int y = chestHeight - 1; y >= 0; y--)
            {
                if (added)
                    break;
                for (int x = 0; x < chestWidth; x++)
                {
                    if (chestSlots[x, y] == null)
                    {
                        chestSlots[x, y] = new InventorySlot
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
    //
    public Vector2Int Contains(ItemClass item)
    {
        for (int y = chestHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < chestWidth; x++)
            {
                if (chestSlots[x, y] != null)
                {
                    if (chestSlots[x, y].item.sprite == item.sprite)
                    {
                        if(item.isStackable && chestSlots[x, y].quantity <= stackLimit)
                            return new Vector2Int(x, y);
                    }
                }
            }
        }
    
        return Vector2Int.one * -1;
    }
    //
    // // public void Drag()
    // // {
    // //     //Cursor.SetCursor(texture, Cursor.Auto);
    // //     //Debug.Log(GetClosestSlot().GetItemName());
    // //     if (isMovingItem)
    // //     {
    // //         EndItemMove();
    // //     }
    // //     else
    // //     {
    // //         BeginToDrag();
    // //     }
    // // }
    //
    //
    public bool Loot(ItemClass item, int amount)
    {
        for (int y = chestHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < chestWidth; x++)
            {
                if (chestSlots[x, y] != null)
                {
                    if (chestSlots[x, y].item.itemName == item.itemName)
                    {
                        chestSlots[x, y].quantity -= amount;
                        if (chestSlots[x, y].quantity == 0)
                        {
                            chestSlots[x, y] = null;
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

