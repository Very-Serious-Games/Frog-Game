using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private CinematicManager _cinematicManager;

    private void Start()
    {
        _cinematicManager = GetComponent<CinematicManager>();
    }
}
