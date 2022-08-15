using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TradeDialogHandler : ILateDisposable
{
    private readonly SignalBus signalBus;
    private readonly AbstractInventory[] inventories;
    private readonly CoinsView coinsView;
    private readonly AssetDatabase database;
    private readonly ConsumableManagerConfig baseConfig;

    public TradeDialogHandler(SignalBus signalBus, CoinsView coinsView, AssetDatabase database, ConsumableManagerConfig baseConfig, params AbstractInventory[] inventories)
    {
        this.signalBus = signalBus;
        this.inventories = inventories;
        this.coinsView = coinsView;
        this.database = database;
        this.baseConfig = baseConfig;
        this.signalBus.Subscribe<OnDropItem>(OnSellItemSignal);
    }

    public void LateDispose()
    {
        signalBus.Unsubscribe<OnDropItem>(OnSellItemSignal);
    }

    private void OnSellItemSignal(OnDropItem signal)
    {
        var current = GetInventory(signal.current);
        var other = GetInventory(signal.other);

        var isFromPlayer = current is PlayerInventory;
        var itemType = signal.current.GetItemConfig();
        var itemsCount = signal.current.Count;
        var coinConfig = database[nameof(Coin)];
        var money = other.ItemCount(coinConfig);
        var price = Mathf.FloorToInt(itemType.BaseCost * itemsCount * (isFromPlayer ? baseConfig.PriceSellKoefficient : 1f));

        if(!isFromPlayer && money < price) return;

        if(isFromPlayer)
        {
            current.AddItem(coinConfig, price);
            signal.current.RemoveItem(itemType, itemsCount);
            signal.other.AddItem(itemType, itemsCount);
        }
        else
        {
            other.RemoveItem(coinConfig, price);
            signal.other.AddItem(itemType, itemsCount);
            signal.current.RemoveItem(itemType, itemsCount);

        }

        coinsView.Show(isFromPlayer ? current.ItemCount(coinConfig) : other.ItemCount(coinConfig));
    }

    private AbstractInventory GetInventory(Slot slot)
    {
        foreach(var i in inventories)
        {
            if(i.Inventory.Contains(slot)) return i;
        }

        return null;
    }

    private void Sell(ConsumableConfig item, int items, AbstractInventory playerInventory)
    {
        var coinConfig = database[nameof(Coin)];
        var coins = Mathf.FloorToInt((float)item.BaseCost * baseConfig.PriceSellKoefficient);

        playerInventory.AddItem(coinConfig, coins * items);

        var count = playerInventory.ItemCount(coinConfig);
        coinsView.Show(count);
    }

    private void Buy(ConsumableConfig item, int items, AbstractInventory playerInventory)
    {
        var coinConfig = database[nameof(Coin)];
        var coins = Mathf.FloorToInt((float)item.BaseCost);

        playerInventory.RemoveItem(coinConfig, coins * items);

        var count = playerInventory.ItemCount(coinConfig);
        coinsView.Show(count);
    }
}

