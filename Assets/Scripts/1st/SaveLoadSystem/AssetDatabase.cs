using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "Configs/Consumables/Database")]
public class AssetDatabase : ScriptableObject
{
    [SerializeField] private ConsumableConfig[] consumables;

    private Dictionary<string, ConsumableConfig> Assets;

    public ConsumableConfig[] GetAllConsumables => consumables;

    public ConsumableConfig this[string id]
    {
        get
        {
            if(Assets == null)
            {
                throw new Exception("Initialize Database first!");
            }
            if(string.IsNullOrEmpty(id)) throw new Exception("Incorrect id!");
            if(!Assets.ContainsKey(id)) throw new Exception(string.Format("No such id found in Database! Name of: {0}", id));



            return Assets[id];
        }
    }

    public void InitializeDatabase()
    {
        Assets ??= new Dictionary<string, ConsumableConfig>();

        Assets = consumables.ToDictionary(a => a.ConsumableName, a => a);
    }
}
