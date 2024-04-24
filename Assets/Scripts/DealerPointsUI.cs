using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ScoreBoard;

public class DealerPointsUI : MonoBehaviour, IUpdateableText
{
    public static TextMeshProUGUI dealerPointsText;
    public static Image imageScore;
    public Renderer renderer;

    public void UpdateText(string newText)
    {
        if (dealerPointsText != null)
        {
            dealerPointsText.text = newText;
        }
    }

    public void Awake()
    {
        DealerPointsUI.dealerPointsText = GetComponent<TextMeshProUGUI>();
        imageScore = GetComponentInParent<Image>();
        dealerPointsText.color = Color.black;
        imageScore.material.SetColor("_Color", new Color(250 / 255f, 255 / 255f, 109 / 255f));
        ToggleScoreUI();

    }

    public void ChangeStyle(BlackjackStatus status)
    {
        if (status == BlackjackStatus.Busted)
        {
            dealerPointsText.color = new Color32(255, 255, 255, 255);
            imageScore.material.SetColor("_Color", Color.red);
        }
        else if (status == BlackjackStatus.Blackjack)
        {
            dealerPointsText.color = Color.white;
            imageScore.material.SetColor("_Color", Color.blue);
        }
        else
        {
            dealerPointsText.color = Color.black;
            imageScore.material.SetColor("_Color", new Color(250 / 255f, 255 / 255f, 109 / 255f));
        }
    }

    public static void ToggleScoreUI()
    {
        Color color = imageScore.material.GetColor("_Color");

        dealerPointsText.color = new Color(0 / 255f, 0 / 255f, 0 / 255f,
            (color.a == 0f) ? 1f : 0f);
        imageScore.material.SetColor("_Color",
            new Color(color.r, color.g, color.b, (color.a == 0f) ? 1f : 0f));
    }
}
