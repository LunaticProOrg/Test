using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InventoryModel
{
    [JsonProperty] public List<Slot> items;

    public class Slot
    {
        [JsonProperty] public readonly string itemName;
        [JsonProperty] public readonly int itemsCount;

        public Slot(string itemName, int itemsCount)
        {
            this.itemName = itemName;
            this.itemsCount = itemsCount;
        }
    }

    public InventoryModel()
    {
        items = new List<Slot>();
    }

    public void Save(string key)
    {
        SaveLoadExtension.Save(this, key);
    }

    public void AddItem(string itemName, int count)
    {
        if(string.IsNullOrEmpty(itemName) || string.IsNullOrWhiteSpace(itemName)) return;
        if(count <= 0) return;

        if(items.Exists(a => a.itemName == itemName))
        {
            var index = items.FindIndex(a => a.itemName == itemName);
            items[index] = new Slot(itemName, count);
        }
        else
        {
            items.Add(new Slot(itemName, count));
        }
    }
}
