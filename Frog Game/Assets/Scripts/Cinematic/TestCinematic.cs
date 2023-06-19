using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCinematic : MonoBehaviour
{
    public int index;

    bool fired;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!fired)
        {
            CinematicManager.Instance.OnTriggerCinematic(index);

            fired = true;
        }

    }
}
