using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardEnums;

public class ScoreBoard 
{
   [SerializeField]
   private List<CardScriptableObject> dealerCards = null;
   [SerializeField]
   private List<CardScriptableObject> playerCards = null;

    public ScoreBoard(List<CardScriptableObject> DealerCards, List<CardScriptableObject> PlayerCards) { 
        dealerCards = DealerCards;
        playerCards = PlayerCards;
    }

    public (int, int) CalculateDealerPoints()
    {
        if( dealerCards == null)
        {
            throw new System.Exception("");
        }
        else if( dealerCards.Count == 0)
        {
            return (0,0);
        }
        else
        {
            int[] sum = new int[2];
            foreach(CardScriptableObject card in dealerCards)
            {
                //Debug.Log(card.value +" valor da carta");
                string rankName = Enum.GetName(typeof(Rank), card.value);

                BasicValue valueDefault = 
                    (BasicValue)Enum.Parse(typeof(BasicValue), rankName);
                AlternativeValue valueAlternative = 
                    (AlternativeValue)Enum.Parse(typeof(AlternativeValue), rankName);

                sum[0] += (int)valueDefault;
                sum[1] += (int)valueAlternative;
 
            }

            return (sum[0], sum[1]);
        }
    }

    public (int, int) CalculatePlayerPoints()
    {
        if (dealerCards == null)
        {
            throw new System.Exception("");
        }
        else if (dealerCards.Count == 0)
        {
            return (0, 0);
        }
        else
        {
            int[] sum = new int[2];
            foreach (CardScriptableObject card in playerCards)
            {
                //Debug.Log(card.value + " valor da carta");
                string rankName = Enum.GetName(typeof(Rank), card.value);

                BasicValue valueDefault =
                    (BasicValue)Enum.Parse(typeof(BasicValue), rankName);
                AlternativeValue valueAlternative =
                    (AlternativeValue)Enum.Parse(typeof(AlternativeValue), rankName);

                sum[0] += (int)valueDefault;
                sum[1] += (int)valueAlternative;

            }

            return (sum[0], sum[1]);
        }
    }

    public BlackjackStatus CalculateStatus(Participant participant)
    {
        (int, int) score = (0, 0);
        int minorValue;
        if (participant == Participant.Dealer)
        {
            try
            {
                score = CalculateDealerPoints();
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
                throw e;
            }

            minorValue = (score.Item1 <= score.Item2) ? score.Item1 : score.Item2;
        }
        else
        {
            try
            {
                score = CalculatePlayerPoints();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                throw e;
            }

            minorValue = (score.Item1 <= score.Item2) ? score.Item1 : score.Item2;
        }

        if (score.Item1==21 || score.Item2==21)
        {
            return BlackjackStatus.Blackjack;
        }
        else if(minorValue < 21)
        {
            return BlackjackStatus.InPlay;
        }
        else
        {
            return BlackjackStatus.Busted;
        }

    }

    public static bool DetermineDealerWinStatus()
    {
        ScoreBoard scoreBoard = new ScoreBoard(HandController.dealerCards,
            HandController.playerCards);
        (int, int) scoreDealer = scoreBoard.CalculateDealerPoints();
        (int, int) scorePlayer = scoreBoard.CalculatePlayerPoints();

        return (DetermineMaxPontuation(scoreDealer)>= DetermineMaxPontuation(scorePlayer) && 
            (scoreDealer.Item1 <= 21 || scoreDealer.Item2 <= 21) ?
            true : false);
    }

    public static int DetermineMaxPontuation((int, int) score)
    {
        if(score.Item1 == score.Item2)
        {
            return score.Item1;
        }
        else if(score.Item1> 21 && score.Item2> 21)
        {
            return (score.Item1 > score.Item2 ? score.Item2 : score.Item1);
        }
        else if(score.Item1 == 21 || score.Item2 == 21)
        {
            return 21;
        }
        else if(score.Item1 < 21 && score.Item2 < 21)
        {
            return (score.Item1 > score.Item2 ? score.Item1 : score.Item2);
        }
        else if(score.Item1 > 21 && score.Item2 < 21 ||
            score.Item1 < 21 && score.Item2 > 21)
        {
            return (score.Item1 < score.Item2 ? score.Item1 : score.Item2);
        }
        else
        {
            return -1;
        }
    }

    public enum BlackjackStatus
    {
        InPlay,
        Blackjack,
        Busted
    }

    public enum Participant
    {
        Dealer,
        Player
    }

}
