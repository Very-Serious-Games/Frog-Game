using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCinematic : MonoBehaviour
{
    public int index;

    public bool fired;

    void OnTriggerEnter(Collider other)
    {
        if (!fired && other.gameObject.layer == LayerMask.NameToLayer("Frog"))
        {
            CinematicManager.Instance.OnTriggerCinematic(index);            

            fired = true;
        }
    }
}
