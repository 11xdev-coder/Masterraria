using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour
{
	protected DropArea DropArea;
	protected DraggableComponent CurrentItem = null;
	public InventorySlot invSlotData;

	protected virtual void Awake()
	{
		DropArea = GetComponent<DropArea>() ?? gameObject.AddComponent<DropArea>();
		DropArea.OnDropHandler += OnItemDropped;
		invSlotData = GetComponent<InventorySlot>();
	}

	public void Initialize(DraggableComponent currentItem)
	{
		if (currentItem == null)
		{
			Debug.LogError("Tried to initialize the slot with an null item!");
			return;
		}

		OnItemDropped(currentItem);
	}

	private void OnItemDropped(DraggableComponent draggable)
	{
		draggable.transform.position = transform.position;
		invSlotData.item.sprite = draggable.GetComponentInChildren<SpriteRenderer>().sprite;
		CurrentItem = draggable;
		draggable.OnBeginDragHandler += CurrentItemOnBeginDrag;
	}

	//Current item is being dragged so we listen for the EndDrag event
	private void CurrentItemOnBeginDrag(PointerEventData eventData)
	{
		CurrentItem.OnEndDragHandler += CurrentItemEndDragHandler;
	}

	private void CurrentItemEndDragHandler(PointerEventData eventData, bool dropped)
	{
		CurrentItem.OnEndDragHandler -= CurrentItemEndDragHandler;

		if (!dropped)
		{
			return;
		}
		CurrentItem.OnBeginDragHandler -= CurrentItemOnBeginDrag; //We make sure to remove this listener as the item is no longer in this slot
		CurrentItem = null; //We no longer have an item in this slot, so we remove the refference
	}
}