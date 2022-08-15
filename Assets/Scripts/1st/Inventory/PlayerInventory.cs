using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInventory : AbstractInventory
{
    protected override string InventoryKey => "Player_Inventory";
    private CoinsView coinsView;

    [Inject]
    private void Construct(CoinsView coinsView)
    {
        this.coinsView = coinsView;
    }

    public override void InitializeStoreData(AssetDatabase database)
    {
        // cheat
        var coinConfig = database[nameof(Coin)];

        if(!ContainsItem(coinConfig))
            AddItem(coinConfig, 1000);

        coinsView.Show(ItemCount(coinConfig));
    }

    protected override void Start()
    {
        base.Start();

        InitializeStoreData(database);
    }
}
