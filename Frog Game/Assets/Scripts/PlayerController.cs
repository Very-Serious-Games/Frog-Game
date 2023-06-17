using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float playerJumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField] private float jumpStrength = 0f;
    [SerializeField] private float multiplier = 20f;
    [SerializeField] private float maxJumpStrength = 10f;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * move;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButton("Jump") && groundedPlayer && jumpStrength < maxJumpStrength)
        {

            jumpStrength += Time.deltaTime * multiplier;
            Debug.Log(jumpStrength);

        }
        else if (Input.GetButtonUp("Jump"))
        {

            playerVelocity.y += Mathf.Sqrt(playerJumpHeight * -3.0f * gravityValue * jumpStrength);
            jumpStrength = 0f;

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}