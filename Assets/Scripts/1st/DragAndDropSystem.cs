using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using System;

public class DragAndDropSystem : IDisposable, IInitializable
{
    private readonly AbstractInventory[] inventories;
    private readonly SignalBus signalBus;

    private Slot current = null;
    private Canvas canvas;

    public DragAndDropSystem(SignalBus signalBus, params AbstractInventory[] inventories)
    {
        this.inventories = inventories;
        this.signalBus = signalBus;
    }

    private void OnPointerDownHandler(Slot slot, PointerEventData eventData)
    {
        canvas ??= slot.GetComponentInParent<Canvas>();

        if(current == null && slot.ContainsItem() && slot.GetItemConfig().ConsumableName != nameof(Coin))
        {
            current = slot;
            current.SlotItemImage.transform.SetParent(canvas.transform);
        }
    }

    private void OnDropHandler(Slot slot, PointerEventData eventData)
    {
        if(current != null)
        {
            if(current != slot && !slot.ContainsItem())
            {
                var currentInventory = GetInventory(current);
                var dropInventory = GetInventory(slot);

                if(currentInventory != dropInventory)
                {
                    signalBus.Fire(new OnDropItem(current, slot));
                }
                else
                {
                    var item = current.GetItemConfig();
                    var count = current.Count;

                    current.RemoveItem(item, count);
                    slot.AddItem(item, count);
                }
            }

            current = null;
        }
    }

    private AbstractInventory GetInventory(Slot slot)
    {
        foreach(var i in inventories)
        {
            if(i.Inventory.Contains(slot)) return i;
        }

        return null;
    }

    private void OnEndDragHandler(Slot slot, PointerEventData eventData)
    {

    }

    private void OnDragHandler(Slot slot, PointerEventData eventData)
    {
        if(current != null)
        {
            var position = new Vector3(eventData.position.x, eventData.position.y, -5f);
            current.SlotItemImage.transform.position = eventData.position;
        }
    }

    private void OnBeginDragHandler(Slot slot, PointerEventData eventData)
    {

    }

    private void OnPointerUpHandler(Slot slot, PointerEventData eventData)
    {
        if(current != null)
        {
            current.SlotItemImage.transform.SetParent(current.transform);
            current.SlotItemImage.transform.localPosition = Vector3.zero;
        }
    }

    public void Dispose()
    {
        foreach(var inventory in inventories)
        {
            foreach(var s in inventory.Inventory)
            {
                s.OnPointerDownHandler -= OnPointerDownHandler;
                s.OnPointerUpHandler -= OnPointerUpHandler;
                s.OnBeginDragHandler -= OnBeginDragHandler;
                s.OnDragHandler -= OnDragHandler;
                s.OnEndDragHandler -= OnEndDragHandler;
                s.OnDropHandler -= OnDropHandler;
            }
        }
    }

    public void Initialize()
    {
        foreach(var inventory in inventories)
        {
            foreach(var s in inventory.Inventory)
            {
                s.OnPointerDownHandler += OnPointerDownHandler;
                s.OnPointerUpHandler += OnPointerUpHandler;
                s.OnBeginDragHandler += OnBeginDragHandler;
                s.OnDragHandler += OnDragHandler;
                s.OnEndDragHandler += OnEndDragHandler;
                s.OnDropHandler += OnDropHandler;
            }
        }
    }
}
