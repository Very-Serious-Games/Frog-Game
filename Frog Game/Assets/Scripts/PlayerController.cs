using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    public float playerJumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField] SimpleSonarShader_ExampleCollision sonarExample;

    private Transform cameraTransform;

    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * move;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero) { 
            gameObject.transform.forward = move;
        }

        if (Input.GetButton("Jump") && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(playerJumpHeight * -3.0f * gravityValue);
        }

        if (Input.GetKey(KeyCode.V)){
            Vector3 playerPosition = transform.position;
            float force = 100.0f;
            sonarExample?.PerformSonarLogic(playerPosition, force);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
