using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableConfig : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int BaseCost { get; private set; }
    public abstract string ConsumableName { get; }
}
