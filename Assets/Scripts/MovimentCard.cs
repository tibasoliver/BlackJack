using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentCard : MonoBehaviour
{
    public bool isMovingToTarget = false;
    public Vector3 target;
    public Quaternion targetRot;
    public float moveSpeed = 2.0f;
    public float rotateSpeed = 540f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(target!= null)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        
    }
}
