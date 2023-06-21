using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 5.0f;
    public float playerJumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField] SimpleSonarShader_ExampleCollision sonarExample;
    [SerializeField] List<SC_NPCFollow> frienCroak;
    [SerializeField] GameObject optionsMenu;


    [SerializeField] private float jumpStrength = 0f;
    [SerializeField] private float multiplier = 20f;
    [SerializeField] private float maxJumpStrength = 10f;


    private Transform cameraTransform;

    private Animator _animator;

    private Outline outline;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
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
            _animator.SetBool("Jump", true);
            jumpStrength += Time.deltaTime * multiplier;
            Debug.Log(jumpStrength);

        }
        else if (Input.GetButtonUp("Jump"))
        {
            _animator.SetBool("KeyUP", true);
            playerVelocity.y += Mathf.Sqrt(playerJumpHeight * -3.0f * gravityValue * jumpStrength);
            jumpStrength = 0f;

        }

        if (Input.GetKeyDown(KeyCode.R)) {
            AudioManager.instance.PlaySFX("frog_sound");
            Vector3 playerPosition = transform.position;
            float force = 100.0f;
            sonarExample?.PerformSonarLogic(playerPosition, force);
            foreach (SC_NPCFollow item in frienCroak)
            {
                if (item.state == SC_NPCFollow.State.follow)
                {
                    item.PerformSonarLogic();
                }
                
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            optionsMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        if (Input.GetKey(KeyCode.V)) {
            foreach (SC_NPCFollow item in frienCroak)
            {
                item.PerformFollowLogic();

            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        if (!groundedPlayer)
        {
            _animator.SetBool("OnAir", true);
        }

        // TO DO resetear el booleano de la animacion de OnAir. Ver donde ponerlo.

        controller.Move(playerVelocity * Time.deltaTime);
    }
}