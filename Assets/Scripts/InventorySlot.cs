using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot
{
    public Vector2Int position;
    public int quantity;
    public ItemClass item;

    public InventorySlot(ItemClass _item, Vector2Int _position, int _quantity)
    {
        position = _position;
        quantity = _quantity;
        item = _item;
    }
    // public InventorySlot(InventorySlot slot)
    // {
    //     this.item = slot.GetItem();
    //     this.quantity = slot.GetQuantity();
    // }
    //
    // public string GetItemName()
    // {
    //     return item.itemName;
    // }
    //
    // public ItemClass GetItem()
    // {
    //     return item;
    //
    // }
    //
    // public int GetQuantity()
    // {
    //     return quantity;
    // }
    //
    // public void AddItem(int _quantity, ItemClass _item)
    // {
    //     this.quantity = _quantity;
    //     this.item = _item;
    // }
}
