using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCard : MonoBehaviour
{
    public float rotationSpeed = 45.0f;
    public bool isFeatureActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFeatureActive)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
