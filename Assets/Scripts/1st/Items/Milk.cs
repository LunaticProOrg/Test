using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Consumables/Items/Milk")]
public class Milk : ConsumableConfig
{
    public override string ConsumableName => nameof(Milk);
}