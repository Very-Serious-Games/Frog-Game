using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicNavMeshAgent : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] List<Transform> m_waypoints;
    [SerializeField] float distanceToDestination = 0.5f;
     [SerializeField]  int m_currentWaypointIndex = 0;

    [SerializeField] SimpleSonarShader_ExampleCollision sonarExample;
    public event EventHandler OnFrienCroak;

    // Start is called before the first frame update
    void Start()
    {
        m_agent.SetDestination(m_waypoints[m_currentWaypointIndex].position);
    }

    public void PerformSonarLogic()
    {
        OnFrienCroak?.Invoke(this, EventArgs.Empty);

        Vector3 playerPosition = transform.position;
        float force = 100.0f;
        sonarExample?.PerformSonarLogic(playerPosition, force);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_agent.remainingDistance < distanceToDestination)  {

            m_currentWaypointIndex++;
            if (m_currentWaypointIndex >= m_waypoints.Count) {
                m_currentWaypointIndex = 0;
            }

            m_agent.SetDestination(m_waypoints[m_currentWaypointIndex].position);
            
        }
        Vector3 v = m_agent.transform.position;
        v.y++;
        m_agent.Move(v);
              

    }
}
