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

    public ItemClass ItemToPutEnd()
    {
        for (int y = inventory.inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = inventory.inventoryWidth - 1; x >= 0; x--)
            {
                if (inventory.inventorySlots[x, y] != null)
                {
                    ItemClass toReturn = inventory.inventorySlots[x, y].item;
                    inventory.Remove(inventory.inventorySlots[x, y].item);
                    return toReturn;
                }
            }
        }

        return null;
    }
    
    public ItemClass ItemToPutStart()
    {
        for (int y = 0; y < inventory.inventoryHeight; y++)
        {
            for (int x = 0; x < inventory.inventoryWidth; x++)
            {
                if (inventory.inventorySlots[x, y] != null)
                {
                    ItemClass toReturn = inventory.inventorySlots[x, y].item;
                    inventory.Remove(inventory.inventorySlots[x, y].item);
                    return toReturn;
                }
            }
        }

        return null;
    }

    public void PutEndPressed()
    {
        Put(ItemToPutEnd());
    }

    public void PutStartPressed()
    {
        Put(ItemToPutStart());
    }
    
    public bool Put(ItemClass item)
    {
        Vector2Int itemPos = Contains(item);
        bool added = false;
        if (itemPos != Vector2Int.one * -1)
        {
            //if (inventorySlots[itemPos.x, itemPos.y].quantity <= stackLimit)
            //{
            chestSlots[itemPos.x, itemPos.y].quantity += 1;
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
                            (item, new Vector2Int(x, y), 1);
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
    // public bool RemoveFromChest(ItemClass item)
    // {
    //     for (int y = inventoryHeight - 1; y >= 0; y--)
    //     {
    //         for (int x = 0; x < inventoryWidth; x++)
    //         {
    //             if (chestSlots[x, y].item != null)
    //             {
    //                 if (chestSlots[x, y].item.itemName == item.itemName)
    //                 {
    //                     chestSlots[x, y].quantity -= 1;
    //                     if (chestSlots[x, y].quantity == 0)
    //                     {
    //                         chestSlots[x, y] = null;
    //                     }
    //
    //                     UpdateInventoryUI();
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //
    //     return false;
    // }
}

