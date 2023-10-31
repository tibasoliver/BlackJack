using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public List<CardScriptableObject> playerCards = new List<CardScriptableObject>();
    public List<CardScriptableObject> dealerCards = new List<CardScriptableObject>();

    public List<GameObject> playerCardCollection = new List<GameObject>();
    public List<GameObject> dealerCardCollection = new List<GameObject>();

    public static HandController instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void add
}
