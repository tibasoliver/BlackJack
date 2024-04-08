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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //drawCardToPlayer();
            // Coloque aqui o código que você deseja executar quando a tecla 'Espaço' for pressionada.
            //drawCard(out tempCardScriptableObject,out tempCardGameObject);
            StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Player));
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //drawCardToDealer();
            StartCoroutine(DrawCardsWithPreciseDelayCoroutine(0.55f, Participant.Dealer));
        }
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
            Debug.Log(card.backImage);
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

    void drawCardToPlayer()
    {
        drawCard(out tempCardScriptableObject, out tempCardGameObject);
        if (tempCardGameObject != null && tempCardScriptableObject != null)
        {
            MovimentCard movimentScript = tempCardGameObject.GetComponent<MovimentCard>();
            movimentScript.targetRot = tempCardGameObject.transform.rotation;

            int numberOfCardsOfPlayer = handController.playerCards.Count;
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

            handController.playerCards.Add(tempCardScriptableObject);
            handController.playerCardCollection.Add(tempCardGameObject);


        }


    }

    void drawCardToDealer()
    {
        drawCard(out tempCardScriptableObject, out tempCardGameObject);
        if (tempCardGameObject != null && tempCardScriptableObject != null)
        {
            MovimentCard movimentScript = tempCardGameObject.GetComponent<MovimentCard>();
            movimentScript.targetRot = tempCardGameObject.transform.rotation;

            int numberOfCardsOfDealer = handController.dealerCards.Count;

            Vector3 nextPosition = new Vector3(numberOfCardsOfDealer * 0.28f, numberOfCardsOfDealer * 0.01f, 0);
            changeYFromCardBeforeMoviment(tempCardGameObject, nextPosition.y);
            movimentScript.target = dealerHandPosition.position + nextPosition;

            //handController.dealerCards.Add(tempCardScriptableObject);
            handController.AddCard(tempCardScriptableObject);
            handController.dealerCardCollection.Add(tempCardGameObject);
        }
    }

    void changeYFromCardBeforeMoviment(GameObject obj, float height)
    {
        Vector3 newPosition = tempCardGameObject.transform.position;
        newPosition.y = height;
        tempCardGameObject.transform.position = newPosition;
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

    IEnumerator DrawCardsWithPreciseDelayCoroutine(float delay, Participant participant)
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
    }

    enum Participant
    {
        Player,
        Dealer
    }
}
