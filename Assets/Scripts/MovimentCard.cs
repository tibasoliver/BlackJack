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

    public float distanceToTarget;
    public const float DELTA_DISTANCE = 0.01f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CalculateDistanceRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && distanceToTarget >= DELTA_DISTANCE)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

    }

    public float GetDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, target);
        return distance;
    }

    private IEnumerator CalculateDistanceRoutine()
    {
        while (true)
        {
            if (target != null)
            {
                distanceToTarget = GetDistanceToTarget();
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
