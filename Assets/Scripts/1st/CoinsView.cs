using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinsView
{

    private readonly TextMeshProUGUI text;

    public CoinsView(TextMeshProUGUI text)
    {
        this.text = text;
    }

    public void Show(int amount)
    {
        text.text = amount.ToString();
    }

}
