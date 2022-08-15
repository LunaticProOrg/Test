using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : AbstractInventory
{
    protected override string InventoryKey => "Npc_Inventory";

    public override void InitializeStoreData(AssetDatabase database)
    {
        if(model.items.Count == 0)
        {
            foreach(var item in database.GetAllConsumables)
            {
                if(item.ConsumableName == nameof(Coin)) continue;
                AddItem(item, 1);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        InitializeStoreData(database);
    }
}
