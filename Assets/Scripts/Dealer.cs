using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public DeckController deckController;
    public HandController handController;
    public GameObject prefab;

    public Transform spawner;
    public Transform playerHandPosition;
    public Transform dealerHandPosition;

    private CardScriptableObject tempCardScriptableObject;
    private GameObject tempCardGameObject;

    private float delayBetweenCards = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //drawCardToPlayer();
        //drawCard(out tempCardScriptableObject, out tempCardGameObject);

        StartCoroutine(DeliverInitialCards());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //drawCardToPlayer();
        //    // Coloque aqui o código que você deseja executar quando a tecla 'Espaço' for pressionada.
        //    //drawCard(out tempCardScriptableObject,out tempCardGameObject);
        //    StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Player));
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    //drawCardToDealer();
        //    StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Dealer));
        //}
    }

    void drawCard(out CardScriptableObject resultCardScriptable, out GameObject resultCardGameObject)
    {
        //AudioManager.Instance.PlayDrawCardEffect();

        float ajustmentDelayBetweenCardAndAudio = 10.0f;
        //StartCoroutine(PreciseDelayCoroutine(ajustmentDelayBetweenCardAndAudio));

        CardScriptableObject card = deckController.drawCardFromTop();
        if (card != null)
        {
            GameObject instanciaPrefab = Instantiate(prefab, spawner.position, prefab.transform.rotation);
            GameObject frontFace = instanciaPrefab.transform.Find("Model Holder/FrontFace").gameObject;
            GameObject face = frontFace.transform.Find("Face").gameObject;
            Renderer renderer = face.GetComponent<Renderer>();
            Material material = renderer.material;
            Texture2D frontFaceTexture = Resources.Load<Texture2D>("FrontFace/" + card.backImage);
            //Debug.Log(card.backImage);
            material.SetTexture("_MainTex", frontFaceTexture);

            resultCardScriptable = card;
            resultCardGameObject = instanciaPrefab;
        }
        else
        {
            resultCardScriptable = null;
            resultCardGameObject = null;
        }
    }

    public void drawCardToPlayer()
    {
        drawCard(out tempCardScriptableObject, out tempCardGameObject);
        if (tempCardGameObject != null && tempCardScriptableObject != null)
        {
            MovimentCard movimentScript = tempCardGameObject.GetComponent<MovimentCard>();
            movimentScript.targetRot = tempCardGameObject.transform.rotation;

            int numberOfCardsOfPlayer = HandController.playerCards.Count;
            int numberOfRows = (numberOfCardsOfPlayer) / 3;
            int numberCardOfRow = (numberOfCardsOfPlayer) % 3;

            //scale 0.7878
            //Vector3 nextPosition = new Vector3(0 + numberCardOfRow * 0.15f, 0 + numberOfCardsOfPlayer * 0.01f, 0 + numberCardOfRow * 0.15f) +
            //    new Vector3(-0.10f,0f,-0.5f)*numberOfRows;

            //scale 1.5
            Vector3 nextPosition = new Vector3(numberCardOfRow * 0.28f, numberOfCardsOfPlayer * 0.01f, numberCardOfRow * 0.30f) +
                new Vector3(-0.10f, 0f, -0.8f) * numberOfRows;

            changeYFromCardBeforeMoviment(tempCardGameObject, playerHandPosition.position.y + nextPosition.y);
            movimentScript.target = playerHandPosition.position + nextPosition;

            HandController.playerCards.Add(tempCardScriptableObject);
            handController.playerCardCollection.Add(tempCardGameObject);

            //////////////-------
            ScoreBoard scoreBoard = new ScoreBoard(HandController.dealerCards, HandController.playerCards);
            (int, int) scoreDealer = scoreBoard.CalculatePlayerPoints();
            //Debug.Log(scoreDealer);

            string newScorePlayer = NormalizeScore(scoreDealer);
            //Debug.Log(newScorePlayer);

            PlayerPointsUI myScript = FindObjectOfType<PlayerPointsUI>();
            if (myScript != null)
            {
                myScript.UpdateText(newScorePlayer);
                myScript.ChangeStyle(scoreBoard.CalculateStatus(ScoreBoard.Participant.Player));
            }
        }


    }

    void drawCardToDealer()
    {
        drawCard(out tempCardScriptableObject, out tempCardGameObject);
        if (tempCardGameObject != null && tempCardScriptableObject != null)
        {
            MovimentCard movimentScript = tempCardGameObject.GetComponent<MovimentCard>();
            movimentScript.targetRot = tempCardGameObject.transform.rotation;

            int numberOfCardsOfDealer = HandController.dealerCards.Count;

            Vector3 nextPosition = new Vector3(numberOfCardsOfDealer * 0.28f, numberOfCardsOfDealer * 0.01f, 0);
            changeYFromCardBeforeMoviment(tempCardGameObject, nextPosition.y);
            movimentScript.target = dealerHandPosition.position + nextPosition;

            //handController.dealerCards.Add(tempCardScriptableObject);
            handController.AddCard(tempCardScriptableObject);
            handController.dealerCardCollection.Add(tempCardGameObject);
            //////////////-------
            //Debug.Log("OI");
            ScoreBoard scoreBoard = new ScoreBoard(HandController.dealerCards, HandController.playerCards);
            (int, int) scoreDealer = scoreBoard.CalculateDealerPoints();
            //Debug.Log(scoreDealer);

            string newScoreDealer = NormalizeScore(scoreDealer);
            //Debug.Log(newScoreDealer);

            DealerPointsUI myScript = FindObjectOfType<DealerPointsUI>();
            if (myScript != null)
            {
                myScript.UpdateText(newScoreDealer);
                myScript.ChangeStyle(scoreBoard.CalculateStatus(ScoreBoard.Participant.Dealer));
            }
        }
    }

    void changeYFromCardBeforeMoviment(GameObject obj, float height)
    {
        Vector3 newPosition = tempCardGameObject.transform.position;
        newPosition.y = height;
        tempCardGameObject.transform.position = newPosition;
    }

    public void DealerPlay()
    {
        StartCoroutine(DealerActionSequence());
    }

    IEnumerator DeliverInitialCards()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(delayBetweenCards);
            if (i % 2 == 0)
            {
                //drawCardToDealer();
                StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Dealer));
            }
            else
            {
                //drawCardToPlayer();
                StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Player));
            }
                
        }

        
    }

    public IEnumerator DrawCardsWithPreciseDelayCoroutine(float delay, Participant participant)
    {
        AudioManager.Instance.PlayDrawCardEffect();
        yield return new WaitForSeconds(delay);
        if(participant == Participant.Player)
        {
            drawCardToPlayer();
        }
        else
        {
            drawCardToDealer();
        }
        TurnSystem.EndTurn();
    }

    IEnumerator DealerActionSequence()
    {
        ScoreBoard scoreBoard = new ScoreBoard(HandController.dealerCards,
            HandController.playerCards);
        (int, int) scoreDealer = scoreBoard.CalculateDealerPoints();

        //Calculates if dealer has more points than player
        while (!ScoreBoard.DetermineDealerWinStatus() && 
            (scoreDealer.Item1 < 21 || scoreDealer.Item2 < 21))
        {
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Dealer));
            scoreDealer = scoreBoard.CalculateDealerPoints();
        }
        TurnSystem.EndTurn();
    }

    public enum Participant
    {
        Player,
        Dealer
    }

    public static string NormalizeScore((int, int) score)
    {
        if(score.Item1 == 21 ||
            score.Item2 == 21)
        {
            return "21";
        }
        else if (score.Item1 < 21 &&
            score.Item2 > 21)
        {
            return score.Item1.ToString();
        }
        else if (score.Item1 > 21 &&
            score.Item2 < 21)
        {
            return score.Item2.ToString();
        }
        else if (score.Item1 > 21 &&
            score.Item2 > 21 && 
            score.Item1 == score.Item2)
        {
            return score.Item1.ToString();
        }
        else if (score.Item1 > 21 &&
            score.Item2 > 21 &&
            score.Item1 != score.Item2)
        {
            return (score.Item1 < score.Item2 ?
                score.Item1.ToString() :
                score.Item2.ToString());
        }
        else if(score.Item1 < 21 &&
            score.Item2 < 21 &&
            score.Item1 == score.Item2)
        {
            return score.Item1.ToString();
        }
        else if (score.Item1 < 21 &&
            score.Item2 < 21 &&
            score.Item1 != score.Item2)
        {
            return (score.Item1 < score.Item2 ?
                score.Item1.ToString() +
                "/" + score.Item2.ToString() :
                score.Item2.ToString() +
                "/" + score.Item1.ToString());
        }
        return "";
    }
}
