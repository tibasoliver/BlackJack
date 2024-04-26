using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static List<CardScriptableObject> playerCards;
    public static List<CardScriptableObject> dealerCards;

    public List<GameObject> playerCardCollection;
    public List<GameObject> dealerCardCollection;

    public event Action<CardScriptableObject> OnCardAdded;

    public IUpdateableText dealerPointsUI;

    public static HandController instance;
    private void Awake()
    {
        playerCards = new List<CardScriptableObject>();
        dealerCards = new List<CardScriptableObject>();
        playerCardCollection = new List<GameObject>();
        dealerCardCollection = new List<GameObject>();

        instance = this;
        this.OnCardAdded += CardAddedHandler;
    }

    public void AddCard(CardScriptableObject card)
    {
        dealerCards.Add(card);
        OnCardAdded?.Invoke(card);
    }

    private string CalculatePoint()
    {
        int totalMaximumDealerPoints = 0;
        int totalMinimumDealerPoints = 0;
        
        for( int i = 0; i < dealerCards.Count(); i++ )
        {
            int value = dealerCards[i].value;
            int cardValueMix;

            if(value == 1)
            {
                cardValueMix = 1;
            }
            else if(value > 1 && value <= 10){
                cardValueMix = value;
            }
            else
            {
                cardValueMix = 10;
            }

            totalMaximumDealerPoints += cardValueMix;
        }

        return totalMaximumDealerPoints.ToString();
    }

    private void CardAddedHandler(CardScriptableObject card)
    {
        //Debug.Log("Um cartão foi adicionado: " + card.name + " com valor: "+ (card.value + 1));
    }
}
