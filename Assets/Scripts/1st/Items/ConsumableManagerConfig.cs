using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Consumables/Manager Config")]
public class ConsumableManagerConfig : ScriptableObject
{
    [field: SerializeField] public float PriceSellKoefficient;
}

