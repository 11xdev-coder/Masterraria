using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ToolClass tool;

    public Vector2 offset;
    public Vector2 multiplier;

    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;

    public int inventoryWidth;
    public int inventoryHeight;
    public InventorySlot[,] inventorySlots;
    public GameObject[,] uiSlots;

    private void Start()
    {
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];

        SetupUI();
        UpdateInventoryUI();
        Add(new ItemClass(tool));
    }
    public void SetupUI()
    {
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab,
                    inventoryUI.transform.GetChild(0).transform);

                inventorySlot.GetComponent<RectTransform>().localPosition = 
                    new Vector3((x * multiplier.x) + offset.x, (y * multiplier.y) + offset.y);

                uiSlots[x,y] = inventorySlot;
                inventorySlots[x, y] = null;
            }
        }
    }

    public void UpdateInventoryUI()
    {
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                if (inventorySlots[x, y] == null)
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
                else
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x,y].item.sprite;
                }
            }
        }
    }

    public void Add(ItemClass item)
    {
        bool added = false;
        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            if (added)
                break;
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] == null)
                {
                    inventorySlots[x, y] = new InventorySlot
                        {item = item, position = new Vector2Int(x, y), quantity = 1};
                    added = true;
                    break;
                }
            }
        }

        UpdateInventoryUI();
    }

    public void Remove(ItemClass item)
    {

    }
}
