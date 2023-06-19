using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    GameManager gameManagerC;
    Camera cameraC;

    bool cinematicMode;
    Vector3 savedGameplayPosition;
    float savedGameplaySize;

    // Start is called before the first frame update
    void Start()
    {        
        cameraC = GetComponent<Camera>();
        cinematicMode = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterCinematicMode()
    {
        if (!cinematicMode)
        {
            savedGameplayPosition = transform.position;
            //savedGameplaySize = cameraC.orthographicSize;

            cinematicMode = true;
        }
    }

    public void ExitCinematicMode()
    {
        if (cinematicMode)
        {
            transform.position = savedGameplayPosition;
            //cameraC.orthographicSize = savedGameplaySize;

            cinematicMode = false;
        }
    }

    public void SetSize(float size)
    {
        cameraC.orthographicSize = size;
    }
}
