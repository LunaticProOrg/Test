using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDropItem
{
    public readonly Slot current;
    public readonly Slot other;

    public OnDropItem(Slot current, Slot other)
    {
        this.current = current ?? throw new NullReferenceException(nameof(current));
        this.other = other ?? throw new NullReferenceException(nameof(other));
    }
}
