using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ScoreBoard;

public class PlayerPointsUI : MonoBehaviour, IUpdateableText, IStyleChange
{
    public static TextMeshProUGUI playerPointsText;
    public static Image imageScore;

    public void UpdateText(string newText)
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = newText;
        }
    }

    public void Awake()
    {
        PlayerPointsUI.playerPointsText = GetComponent<TextMeshProUGUI>();
        imageScore = GetComponentInParent<Image>();
            playerPointsText.color = Color.black;
            imageScore.material.SetColor("_Color", new Color(250 / 255f, 255 / 255f, 109 / 255f));

        ToggleScoreUI();
    }

    public void ChangeStyle(BlackjackStatus status)
    {
        if(status == BlackjackStatus.Busted)
        {
            playerPointsText.color = new Color32(255, 255, 255, 255);
            imageScore.material.SetColor("_Color", Color.red);
        }
        else if(status == BlackjackStatus.Blackjack)
        {
            playerPointsText.color = Color.white;
            imageScore.material.SetColor("_Color", Color.blue);
        }
        else
        {
            playerPointsText.color = Color.black;
            imageScore.material.SetColor("_Color", new Color(250/255f, 255/255f, 109/255f));
        }
    }

    public static void ToggleScoreUI()
    {
        Color color = imageScore.material.GetColor("_Color");

        playerPointsText.color = new Color(0 / 255f, 0 / 255f, 0 / 255f,
            (color.a == 0f) ? 1f : 0f);
        imageScore.material.SetColor("_Color", 
            new Color(color.r, color.g, color.b, (color.a == 0f) ? 1f : 0f));
    }
}
