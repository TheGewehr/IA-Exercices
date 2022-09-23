using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAIKinematic : MonoBehaviour
{
    enum States
    {
        Seek = 0,
        Flee = 1
    }

   

    
   [SerializeField]
    States IASatate;

    [SerializeField]
    GameObject target;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float minDistance;

    [SerializeField]
    float turnSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        

       

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;
        float angle = 0;
        float distance = Vector3.Distance(target.transform.position, transform.position);

        switch (IASatate)
        {
            case States.Seek:
                {
                    direction = target.transform.position - transform.position;
                    direction.y = 0f;

                    if(minDistance < distance)
                    {
                        movement = direction.normalized * maxVelocity * Time.deltaTime;
                    }
                }
                break;
            case States.Flee:
                {
                    direction = transform.position - target.transform.position;

                    movement = direction.normalized * maxVelocity * Time.deltaTime;
                }
                break;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        if((distance > minDistance) || IASatate == States.Flee) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime * turnSpeed);
            transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;
        }
        
    }
}
