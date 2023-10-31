using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;
    private void Awake()
    {
        instance = this;
        SetupDeck();
        SortDeck();
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SortDeck()
    {
        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();

        int totalCards = deckToUse.Count;
        //Debug.Log(totalCards);

        for (int i = 0; i < totalCards; i++)
        {
            int indexSorted = UnityEngine.Random.Range(0, deckToUse.Count);
            //Debug.Log(indexSorted);
            tempDeck.Add(deckToUse[indexSorted]);
            deckToUse.RemoveAt(indexSorted);
        }

        deckToUse.Clear();
        deckToUse.AddRange(tempDeck);
    }

    private void SetupDeck()
    {
        deckToUse.Clear();

        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                CardScriptableObject card = new CardScriptableObject();
                card.naipe = i;
                card.value = j;
                card.backImage = setNameImageFromCard(card);

                deckToUse.Add(card);
            }
        }
    }

    public string setNameImageFromCard(CardScriptableObject card)
    {
        string pointsCardName;
        if(card.value>0 && card.value < 10)
        {
            pointsCardName = (card.value+1).ToString();
        }
        else
        {
            CardEnums.Rank rank = (CardEnums.Rank)Enum.ToObject(typeof(CardEnums.Rank), card.value);
            pointsCardName = rank.ToString().ToLower();
        }

        string naipe;
        CardEnums.Suit suit = (CardEnums.Suit)Enum.ToObject(typeof(CardEnums.Suit), card.naipe);
        naipe = "_of_" + suit.ToString().ToLower();


        return pointsCardName + naipe;
    }

    public CardScriptableObject drawCardFromTop()
    {
        if(deckToUse.Count > 0)
        {
            CardScriptableObject topCard = new CardScriptableObject();
            topCard = deckToUse[0];
            deckToUse.RemoveAt(0);

            return topCard;
        }

        return null;
    }
}
