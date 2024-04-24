using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerActionButtonsManager : MonoBehaviour
{
    public static Button hitButton;
    public static Button standButton;
    public static Button restartButton;
    public Dealer dealer;

    public static bool enableToClick = true;
    public static float cooldown = 2f;
    public static float lastClickTime;

    private static readonly object lockObject = new object();

    void Awake()
    {
        hitButton = GameObject.Find("HitButton").GetComponent<Button>();
        standButton = GameObject.Find("StandButton").GetComponent<Button>();
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();

        if (hitButton != null)
            hitButton.interactable = false;

        if (standButton != null)
            standButton.interactable = false;

        if(restartButton != null)
            restartButton.interactable = false;
    }

    public static void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public static void ToggleUIButtons(bool enableHitButton, bool enableStandButton,
        bool enableRestartButton)
    {
        hitButton.interactable = enableHitButton;
        standButton.interactable = enableStandButton;
        restartButton.interactable = enableRestartButton;
    }

    public void HitCard()
    {
        ToggleUIButtons(false, false, false);
        StartCoroutine(ResetReady());

        if (TurnSystem.currentTurn == TurnSystem.PlayerTurn.Player)
        {
            dealer.StartCoroutine(dealer.DrawCardsWithPreciseDelayCoroutine(0.55f, Dealer.Participant.Player));
            //TurnSystem.EndTurn();
        }
        
    }

    IEnumerator ResetReady()
    {
        yield return new WaitForSeconds(10f);
    }

    public void Stand()
    {
        ToggleUIButtons(false, false, false);
        TurnSystem.currentTurn = TurnSystem.PlayerTurn.Dealer;
        dealer.DealerPlay();
    }
}
