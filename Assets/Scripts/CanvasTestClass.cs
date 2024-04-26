using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasTestClass : MonoBehaviour
{
    public GameObject Canvas;
    void Awake()
    {
        Canvas.SetActive(true);
    }
    private void Start()
    {
        Canvas.SetActive(true);
    }
    private void Update()
    {
        Canvas.SetActive(true);
    }
    void OnDisable()
    {
        //this.gameObject.SetActive(true);
        //StackTrace stackTrace = new StackTrace();
    }
}
