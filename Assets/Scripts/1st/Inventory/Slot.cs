using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class Slot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IDropHandler
{
    private ConsumableConfig item = null;
    private SlotView viewer;
    private int itemsCount = 0;

    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI Text;

    public Image SlotItemImage => Image;

    public int Count => itemsCount;

    public event Action<Slot, PointerEventData> OnPointerDownHandler;
    public event Action<Slot, PointerEventData> OnPointerUpHandler;
    public event Action<Slot, PointerEventData> OnBeginDragHandler;
    public event Action<Slot, PointerEventData> OnDragHandler;
    public event Action<Slot, PointerEventData> OnEndDragHandler;
    public event Action<Slot, PointerEventData> OnDropHandler;


    private void Awake()
    {
        this.viewer = new SlotView(Image, Text);
        Image.raycastTarget = false;
    }

    public void Sell(int amout = 1)
    {
        itemsCount -= amout;

        if(itemsCount == 0)
        {
            item = null;
        }

        viewer.ViewSlot(item, itemsCount);
    }

    public ConsumableConfig GetItemConfig()
    {
        return item;
    }

    public KeyValuePair<string, int> GetItem()
    {
        var kvp = new KeyValuePair<string, int>(item != null ? item.ConsumableName : "", itemsCount);

        return kvp;
    }

    public void AddItem(ConsumableConfig consumableConfig, int count = 1)
    {
        item = consumableConfig;
        itemsCount += count;

        viewer.ViewSlot(item, itemsCount);
    }

    public bool ContainsItem() => item != null && itemsCount > 0;
    public bool ContainsItem(ConsumableConfig consumableConfig) => item == consumableConfig;
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(this, eventData);
    }

    public void RemoveItem(ConsumableConfig coinConfig, int coins)
    {
        if(coinConfig == item)
            Sell(coins);
        else throw new Exception("Not enough coins");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragHandler?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragHandler?.Invoke(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(this, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropHandler?.Invoke(this, eventData);
    }
}
