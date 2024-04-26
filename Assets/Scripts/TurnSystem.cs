using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static PlayerTurn currentTurn;
    public static Transform positionToInstantiateEndStatusDealer;
    public static Transform positionToInstantiateEndStatusPlayer;
    public static Transform parent;

    public static GameObject prefabBustedImage;
    public static GameObject prefabWonImage;
    public static GameObject prefabLostImage;
    public static GameObject prefabBlackJackImage;

    private void Awake()
    {

        positionToInstantiateEndStatusDealer = 
            GameObject.Find("PositionDealerStatus").transform;
        positionToInstantiateEndStatusPlayer =
            GameObject.Find("PositionPlayerStatus").transform;

        prefabBustedImage = Resources.Load<GameObject>("Prefabs/Busted");
        prefabWonImage = Resources.Load<GameObject>("Prefabs/Won");
        prefabLostImage = Resources.Load<GameObject>("Prefabs/Lost");
        prefabBlackJackImage = Resources.Load<GameObject>("Prefabs/Blackjack");

        //GameObject tableObject = GameObject.Find("Table");
        //Transform parent = tableObject.transform.Find("Canvas");
    }

    public static void SwitchTurn(PlayerTurn playerTurn)
    {
        currentTurn = playerTurn;
    }

    public static void EndTurn()
    {
        //Calculates points
        ScoreBoard scoreBoard = new ScoreBoard(HandController.dealerCards,
            HandController.playerCards);
        (int, int) scoreDealer = scoreBoard.CalculateDealerPoints();
        (int, int) scorePlayer = scoreBoard.CalculatePlayerPoints();
        string newScoreDealer = Dealer.NormalizeScore(scoreDealer);
        string newScorePlayer = Dealer.NormalizeScore(scorePlayer);
        int intValue;

        if(currentTurn == PlayerTurn.Dealer)
        {
            if(ScoreBoard.DetermineDealerWinStatus() == true)
            {
                currentTurn = PlayerTurn.EndGame;
                PlayerActionButtonsManager.ToggleUIButtons(false, false, true);
            }
            else if (scoreBoard.CalculateStatus(ScoreBoard.Participant.Dealer)== ScoreBoard.BlackjackStatus.Busted)
            {
                currentTurn = PlayerTurn.EndGame;
                PlayerActionButtonsManager.ToggleUIButtons(false, false, true);
            }
            else
            {
                PlayerActionButtonsManager.ToggleUIButtons(false, false, false);
            }  
            
        }

        if (currentTurn == PlayerTurn.Player || 
            (currentTurn == PlayerTurn.None && 
            ((HandController.dealerCards.Count + HandController.playerCards.Count)>=4)))
        {
            if (newScorePlayer == "21" || newScoreDealer == "21")
            {
                PlayerActionButtonsManager.ToggleUIButtons(false, false, true);
                currentTurn = PlayerTurn.EndGame;
            }
            else if (int.TryParse(newScorePlayer, out intValue))
            {
                if (intValue > 21)
                {
                    PlayerActionButtonsManager.ToggleUIButtons(false, false, true);
                    currentTurn = PlayerTurn.EndGame;
                }
                else if (intValue < 21)
                {
                    PlayerActionButtonsManager.ToggleUIButtons(true, true, false);
                    currentTurn = PlayerTurn.Player;
                }
            }
            else
            {
                PlayerActionButtonsManager.ToggleUIButtons(true, true, false);
                currentTurn = PlayerTurn.Player;
            }

            if (currentTurn == PlayerTurn.EndGame)
            {
                PlayerActionButtonsManager.ToggleUIButtons(false, false, true);
            }
        }

        if(currentTurn == PlayerTurn.EndGame)
        {
            PlayerActionButtonsManager.ToggleUIButtons(false, false, true);

            ScoreBoard.BlackjackStatus statusDealer =  
                scoreBoard.CalculateStatus(ScoreBoard.Participant.Dealer);
            ScoreBoard.BlackjackStatus statusPlayer =
                scoreBoard.CalculateStatus(ScoreBoard.Participant.Player);

            Vector2 positionPlayerStatus = new Vector2(-560f,-200f);
            Vector2 positionDealerStatus = new Vector2(-560f, 326f);

            GameObject instance;
            RectTransform rectTransform;

            GameObject tableObject = GameObject.Find("Table");
            Transform parent = tableObject.transform.Find("Canvas");
            bool showedStatus = false;

            if (statusPlayer == ScoreBoard.BlackjackStatus.Busted && !showedStatus)
            {
                instance = Instantiate(prefabBustedImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionPlayerStatus;

                instance = Instantiate(prefabWonImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionDealerStatus;

                showedStatus = true;
            }

            if (statusDealer == ScoreBoard.BlackjackStatus.Busted && !showedStatus)
            {
                instance = Instantiate(prefabBustedImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionDealerStatus;

                instance = Instantiate(prefabWonImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionPlayerStatus;

                showedStatus = true;
            }

            if(statusDealer == ScoreBoard.BlackjackStatus.Blackjack &&
                !showedStatus)
            {
                instance = Instantiate(prefabBlackJackImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionDealerStatus;

                instance = Instantiate(prefabLostImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionPlayerStatus;

                showedStatus = true;
            }

            if (statusPlayer == ScoreBoard.BlackjackStatus.Blackjack &&
                !showedStatus)
            {
                instance = Instantiate(prefabLostImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionDealerStatus;

                instance = Instantiate(prefabBlackJackImage, parent);
                rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = positionPlayerStatus;

                showedStatus = true;
            }

            if (!showedStatus)
            {
                if (ScoreBoard.DetermineDealerWinStatus())
                {
                    instance = Instantiate(prefabWonImage, parent);
                    rectTransform = instance.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = positionDealerStatus;

                    instance = Instantiate(prefabLostImage, parent);
                    rectTransform = instance.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = positionPlayerStatus;
                }
                else
                {
                    instance = Instantiate(prefabLostImage, parent);
                    rectTransform = instance.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = positionDealerStatus;

                    instance = Instantiate(prefabWonImage, parent);
                    rectTransform = instance.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = positionPlayerStatus;
                }

                showedStatus = true;
            }

        }
            
    }

    public enum PlayerTurn
    {
        None,
        Dealer,
        Player,
        EndGame
    }

    
}
