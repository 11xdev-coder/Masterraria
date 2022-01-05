using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventoryItem : MonoBehaviour, IPointerDownHandler
{
    public bool drop;
    public Inventory inventory;
    public PlayerController player;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // for (int x = 0; x <= (int)eventData.position.x; x++)
        // {
        //     for (int y = 0; y <= (int)eventData.position.y; y++)
        //     {
        //         inventory.Drop(inventory.inventorySlots[x,y].item);
        //     }
        // }
        inventory.Drop(inventory.inventorySlots[0, 0].item);
        
    }
}
