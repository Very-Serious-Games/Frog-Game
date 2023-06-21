using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonTrigger : MonoBehaviour
{
    public int index;

    public bool fired;

    void OnTriggerEnter(Collider other)
    {
        if (!fired)
        {
            CinematicManager.Instance.OnTriggerCinematic(index);

            fired = true;
        }

    }
}
