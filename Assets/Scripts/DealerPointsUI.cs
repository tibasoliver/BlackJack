using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DealerPointsUI : MonoBehaviour, IUpdateableText
{
    public TextMeshProUGUI dealerPointsText;

    public void UpdateText(string newText)
    {
        if (dealerPointsText != null)
        {
            dealerPointsText.text = newText;
        }
    }

    public void Awake()
    {
        this.dealerPointsText = GetComponent<TextMeshProUGUI>();
    }
}
