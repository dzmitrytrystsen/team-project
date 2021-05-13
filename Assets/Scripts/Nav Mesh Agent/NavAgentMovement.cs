using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavAgentMovement : MonoBehaviour
{
 //   public Transform transformBase;
    
    private Camera cam;
    private NavMeshAgent agent;
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        print(FindObjectOfType<PlayerBase>());
     //   agent.SetDestination(transformBase.position);
    }
    
    void Update()
    {
       
    }
}
