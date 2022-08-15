using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public abstract class AbstractInventory : MonoBehaviour
{

    public bool Validate;
    protected abstract string InventoryKey { get; }
    protected InventoryModel model;

    protected List<Slot> _inventory;

    protected AssetDatabase database;

    public List<Slot> Inventory => _inventory;

    private void OnValidate()
    {
        Validate = false;
        int count = 0;

        foreach(Transform item in transform)
        {
            if(item.gameObject.name == "Title") continue;

            item.gameObject.name = "Slot " + count;
            count++;
        }
    }

    [Inject]
    private void Construct(AssetDatabase database)
    {
        this.database = database;
    }

    private void Awake()
    {
        _inventory = new List<Slot>();
        _inventory.AddRange(GetComponentsInChildren<Slot>());

        database.InitializeDatabase();
    }

    protected virtual void Start()
    {
        LoadInventory();
    }

    private void OnDestroy()
    {
        model.items.Clear();

        foreach(var s in _inventory)
        {
            var kvp = s.GetItem();

            model.AddItem(kvp.Key, kvp.Value);
        }

        model.Save(InventoryKey);
    }

    private void LoadInventory()
    {
        model = SaveLoadExtension.Override(InventoryKey, new InventoryModel());

        if(model.items != null && model.items.Count > 0)
        {
            var loaded = new List<KeyValuePair<ConsumableConfig, int>>();

            foreach(var kvp in model.items)
            {
                loaded.Add(new KeyValuePair<ConsumableConfig, int>(database[kvp.itemName], kvp.itemsCount));
            }

            int countAll = loaded.Count;

            int current = 0;

            foreach(var s in _inventory)
            {
                if(current < countAll)
                {
                    s.AddItem(loaded[current].Key, loaded[current].Value);
                }
                else break;

                current++;
            }
        }
    }

    public void RemoveItem(ConsumableConfig coinConfig, int coins)
    {
        TryGetSlot(coinConfig, out var slot);
        slot.RemoveItem(coinConfig, coins);
    }

    protected bool TryGetSlot(ConsumableConfig config, out Slot slot)
    {
        slot = null;

        foreach(var s in _inventory)
        {
            if(s.ContainsItem(config))
            {
                slot = s;
                break;
            }

            if(!s.ContainsItem()) slot = s;
        }

        return slot != null;
    }

    public abstract void InitializeStoreData(AssetDatabase database);

    public bool ContainsItem(ConsumableConfig config)
    {
        return TryGetSlot(config, out var slot) && slot.Count > 0;
    }

    public int ItemCount(ConsumableConfig config)
    {
        if(TryGetSlot(config, out var slot))
            return slot.Count;

        return 0;
    }

    public bool AddItem(ConsumableConfig config, int count = 1)
    {
        if(TryGetSlot(config, out var slot))
        {
            slot.AddItem(config, count);

            return true;
        }

        return false;
    }
}

