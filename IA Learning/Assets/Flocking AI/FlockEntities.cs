using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockEntities : MonoBehaviour
{
    public FlockingManager myManager;

    public float speed;
    private Vector3 direction;

    private float timePassed;
    private float seconds;

    //private bool turning;

    void Start()
    {
        seconds = 1.5f;
        timePassed = seconds;
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
       // turning = false;
    }

    void Update()
    {
        if (timePassed >= seconds)
        {
            if (Random.Range(0.0f, 100.0f) < 75.0f)
            {
                FlockingRules();
            }

            if (Random.Range(0.0f, 100.0f) < 10.0f)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }

            timePassed = 0.0f;
        }
        timePassed += Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);
    }

    void FlockingRules()
    {
        Debug.Log("Rules created");
        Vector3 cohesion = Vector3.zero;
        Vector3 align = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int num = 0;

        foreach (GameObject go in myManager.allFlockingEntities)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance <= myManager.neighbourDistance)
                {
                    // Cohesion
                    cohesion += go.transform.position;

                    // Align
                    align += go.GetComponent<FlockEntities>().direction;
                    num++;

                    // Separation
                    separation -= (transform.position - go.transform.position) / (distance * distance);
                }
            }
        }

        if (num > 0)
        {
            // Cohesion
            cohesion = (cohesion / num - transform.position).normalized * speed;

            // Align
            align /= num;
            speed = Mathf.Clamp(align.magnitude, myManager.minSpeed, myManager.maxSpeed);
        }

        // Final computation
        direction = (cohesion + align + separation).normalized * speed;
    }
}
