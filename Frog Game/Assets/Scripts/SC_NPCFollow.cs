using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_NPCFollow : MonoBehaviour
{
  //Transform that NPC has to follow
    public Transform transformToFollow;
    public List<Transform> m_waypoints;
    //NavMesh Agent variable
    NavMeshAgent agent;
    public Vector3 destination;
    bool arrived = true;

    [SerializeField] SimpleSonarShader_ExampleCollision sonarExample;
    public event EventHandler OnFrienCroak;
    public event EventHandler OnPlayerEnterZone;
    public event EventHandler OnPlayerEndsConversation;
    public State state = State.wondering;
    
    public enum State{
        follow,
        wondering,
        watching
    }

    private Animator _animator;

    public void PerformSonarLogic()
    {
        OnFrienCroak?.Invoke(this, EventArgs.Empty);

        Vector3 npcPosition = transform.position;
        float force = 100.0f;
        sonarExample?.PerformSonarLogic2(npcPosition, force);

    }

    public void PerformWatchingLogic()
    {
        _animator.SetBool("Idle", true);
        OnPlayerEnterZone?.Invoke(this, EventArgs.Empty);
        state = State.watching;
    }

    public void PerformFollowLogic()
    {
        _animator.SetBool("Walk", true);
        _animator.SetBool("Idle", false);
        OnPlayerEndsConversation?.Invoke(this, EventArgs.Empty);
        state = State.follow;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Follow the player
        switch (state)
        {
          case State.follow:
            destination = transformToFollow.position;
            agent.destination = destination;
            break;
          case State.wondering:
            wondering();
            break;
          case State.watching:
            agent.destination = transform.position;
            transform.LookAt(transformToFollow.position);
            break;
          default:
            Debug.Log("error");
            break;
        }
    }

    void wondering(){
      if(arrived){
        destination = m_waypoints[UnityEngine.Random.Range(0, m_waypoints.Count)].position;
        agent.destination = destination;
        arrived = false;
      }
      if(agent.remainingDistance < 5){
        arrived = true;
      }

    }
}
