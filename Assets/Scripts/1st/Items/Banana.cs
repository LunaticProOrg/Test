using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Consumables/Items/Banana")]
public class Banana : ConsumableConfig
{
    public override string ConsumableName => nameof(Banana);
}