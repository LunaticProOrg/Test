using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Consumables/Items/Coin")]
public class Coin : ConsumableConfig
{
    public override string ConsumableName => nameof(Coin);
}
