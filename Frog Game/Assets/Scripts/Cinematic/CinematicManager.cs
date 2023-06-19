using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Transform gameCamera;

    public Transform[] cameraPositions;
    public enum CinematicCommandId
    {
        enterCinematicMode,
        exitCinematicMode,
        wait,
        log,
        activateOverlay,
        desactivateOverlay,
        showDialog,
        setCameraPosition,
        setCameraSize,
        cameraShake,
        SetObjectActive,
        SetObjectPosition,
        SetPlayerFacing
    }

    [System.Serializable]
    public struct CinematicCommand
    {
        public CinematicCommandId id;
        public string param1;
        public string param2;
        public string param3;
        public string param4;
    }

    [System.Serializable]
    public struct CinematicSequence
    {
        public string name;
        public CinematicCommand[] commands;
    };

    [Header("Cinematic system")]
    public CinematicSequence[] sequences;

    [Header("Dialog system")]
    [Space]
    public Transform[] dialogCommon;
    public Transform[] dialogCharacters;
    public Transform dialogText;

    [System.Serializable]
    public struct DialogData
    {
        public int character;
        public string text;
    };

    public DialogData[] dialogsData;

    // Cinematic system

    int sequenceIndex;
    int commandIndex;

    bool waiting;
    float waitTimer;

    // Dialogs system

    bool isCinematicMode;

    bool showingDialog;

    TextMeshPro dialogTextC;

    int dialogIndex;

    GameCamera gameCameraC;

    KeyCode[] debugKey = { KeyCode.S, KeyCode.T, KeyCode.A, KeyCode.R };
    int debugKeyProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        // init state 

        isCinematicMode = false;
        waiting = false;

        // Init dialog system

        showingDialog = false;
        dialogIndex = 0;

        dialogTextC = dialogText.GetComponent<TextMeshPro>();

        gameCameraC = gameCamera.GetComponent<GameCamera>();
        OnTriggerCinematic(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCinematicMode)
        {
            if (showingDialog)
            {
                for (int i = 0; i < dialogCommon.Length; i++) { dialogCommon[i].gameObject.SetActive(true); }
                for (int i = 0; i < dialogCharacters.Length; i++) { dialogCharacters[i].gameObject.SetActive(false); }

                int character = dialogsData[dialogIndex].character;
                string text = dialogsData[dialogIndex].text;

                dialogCharacters[character].gameObject.SetActive(true);
                dialogTextC.text = text;

                if (Input.GetButtonDown("Fire1"))
                {
                    showingDialog = false;
                    for (int i = 0; i < dialogCommon.Length; i++) { dialogCommon[i].gameObject.SetActive(false); }
                    for (int i = 0; i < dialogCharacters.Length; i++) { dialogCharacters[i].gameObject.SetActive(false); }
                    commandIndex++;
                }

            }
            else if (waiting)
            {
                if (waitTimer <= 0)
                {
                    waiting = false;
                    commandIndex++;
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                }
            }
            else if (commandIndex < sequences[sequenceIndex].commands.Length)
            {
                CinematicCommand command = sequences[sequenceIndex].commands[commandIndex];

                if (command.id == CinematicCommandId.enterCinematicMode)
                {
                    GC.Collect();
                    // TODO bloquear movimiento
                    isCinematicMode = true;
                }
                else if (command.id == CinematicCommandId.exitCinematicMode)
                {
                    // TODO desbloquear movimiento
                    isCinematicMode = false;
                }
                else if (command.id == CinematicCommandId.wait)
                {
                    float time = Single.Parse(command.param1);

                    waiting = true;
                    waitTimer = time;
                }
                else if (command.id == CinematicCommandId.log)
                {
                    string message = command.param1;

                    Debug.Log(message);
                }
                else if (command.id == CinematicCommandId.showDialog)
                {
                    int index = Int32.Parse(command.param1);

                    showingDialog = true;
                    dialogIndex = index;
                }
                else if (command.id == CinematicCommandId.setCameraPosition)
                {
                    int index = Int32.Parse(command.param1);

                    gameCamera.position = cameraPositions[index].position;
                }
                else if (command.id == CinematicCommandId.setCameraSize)
                {
                    float size = Single.Parse(command.param1);

                    gameCameraC.SetSize(size);
                }
                else if (command.id == CinematicCommandId.cameraShake)
                {
                    float duration = Single.Parse(command.param1);
                    float amplitude = Single.Parse(command.param2);
                }
                else if (command.id == CinematicCommandId.SetObjectActive)
                {
                    int objectIndex = Int32.Parse(command.param1);
                    int isActive = Int32.Parse(command.param2); // 0 - False, 1 - True
                }
                else if (command.id == CinematicCommandId.SetObjectPosition)
                {
                    int objectIndex = Int32.Parse(command.param1);
                    int positionIndex = Int32.Parse(command.param2);
                }
                else if (command.id == CinematicCommandId.SetPlayerFacing)
                {
                    int facing = Int32.Parse(command.param2); // 0 - Derecha, 1 - Izquierda
                }
                else
                {
                    Debug.Log("Comando de cinematica no implementado");
                }

                if (!waiting && !showingDialog)
                {
                    commandIndex++;
                }
            }
        }
    }

    public void OnTriggerCinematic(int index)
    {
        if (!isCinematicMode)
        {
            isCinematicMode = true;
            sequenceIndex = index;
            commandIndex = 0;
        }
        else
        {
            Debug.Log("No puede iniciarse la cinematica " + "porque ya hay una en ejecucion");
        }
    }

    public bool IsCinematicMode()
    {
        return isCinematicMode;
    }
}
