using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{

    [SerializeField] SC_NPCFollow npcTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
        npcTriggered.PerformWatchingLogic();
        gameObject.SetActive(false);
        }
    }
}
