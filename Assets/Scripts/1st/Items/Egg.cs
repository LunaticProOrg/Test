using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Consumables/Items/Egg")]
public class Egg : ConsumableConfig
{
    public override string ConsumableName => nameof(Egg);
}