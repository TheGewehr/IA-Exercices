using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AngelWander : MonoBehaviour
{
    public NavMeshAgent agent;
    enum States
    {
        Seek = 0,
        Flee = 1,
        Wander = 2
    }

    [SerializeField]
    States IASatate;

    [SerializeField]
    GameObject target;

    
    public float maxVelocity;

    [SerializeField]
    float minDistance;

    [SerializeField]
    float turnSpeed;

    bool NewWanderPoint;

    void Seek()
    {
        agent.destination = target.transform.position;
    }

    void Flee()
    {
        agent.destination = -target.transform.position;
    }

    void Wander()
    {
        //Vector3 localTarget = UnityEngine.Random.insideUnitCircle * radius;
        //localTarget += new Vector3(0, 0, offset);
        //Vector3 worldTarget = transform.TransformPoint(localTarget);
        //worldTarget.y = 0f;
        //
        //agent.destination = worldTarget;
        Vector3 center = Vector3.zero;
        Vector3 output = Vector3.zero;

        agent.destination = RandomPoint(transform.position, 100);

        NewWanderPoint = false;
    }

    void wandah()
    {
        float radius = Random.Range(8f,10f);
        float offset = Random.Range(8f, 10f);

        Vector3 localTarget = Random.insideUnitCircle * radius;
        localTarget += new Vector3(0, 0, offset);

        Vector3 worldTarget = transform.TransformPoint(localTarget);
        worldTarget.y = 0f;

       // agent.destination = worldTarget;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(worldTarget,out hit, radius, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
        else
        {
            worldTarget = transform.TransformPoint(-localTarget);
            
            if(NavMesh.SamplePosition(worldTarget, out hit, radius, NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NewWanderPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        //IASatate = States.Wander; // for test purposes

        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;
        float angle = 0;
        float distance = Vector3.Distance(agent.destination, transform.position);

        switch (IASatate)
        {
            case States.Seek:
                {
                    Seek();
                }
                break;
            case States.Flee:
                {
                    Flee();
                }
                break;
            case States.Wander:
                {
                    if(NewWanderPoint == true)
                    {
                        wandah();                        
                    }                   
                }
                break;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        if ((distance > minDistance))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
            transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;

           
        }
        else
        {
            NewWanderPoint = true;
        }
    }

    // Other Usefull Functions

    Vector3 RandomPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                Debug.DrawRay(hit.position, Vector3.up, Color.blue, 1.0f);
                return hit.position;
            }
        }
       
        return Vector3.zero;     
        
    }
}
