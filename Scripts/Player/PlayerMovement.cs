using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Animator anim;
    [SerializeField] private float groundDistance = .1f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask wall;
    private readonly float gravity = -9.8f;
    private Vector3 velocity;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float stepDown;
    [SerializeField] private float jumpDamp;
    private bool isJumping = false;

    private PlayerWeapon playerWeapon;
    private PauseManager pause;

    private Vector3 rootMotion;

    private AudioSource walkAudio;
    private AudioManager audioManager;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerWeapon = GetComponent<PlayerWeapon>();
        pause = FindObjectOfType<PauseManager>();
        walkAudio = GetComponent<AudioSource>();
        walkAudio.loop = true;
        audioManager = new AudioManager(walkAudio);
    }

    private void Start()
    {
        isJumping = controller.isGrounded;
    }


    // Update is called once per frame
    void Update()
    {
        if (pause.isPause)
        {
            audioManager.StopAudio();
            return;
        }
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        SetAnimMovement(horizontal, vertical, out float horizontalMovement, out float verticalMovement);
        SetPlayerSpeed();

        anim.SetFloat("HorizontalSpeed", horizontalMovement, 0.1f, Time.deltaTime);
        anim.SetFloat("VerticalSpeed", verticalMovement, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void SetPlayerSpeed()
    {
        if (!playerWeapon.CheckHoldingWeapon())
        {
            anim.SetFloat("Move Speed", 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Move Speed", .8f, 0.1f, Time.deltaTime);
        }
    }

    private void SetAnimMovement(float horizontal, float vertical, out float horizontalMovement, out float verticalMovement)
    {
        if (horizontal > 0)
        {
            horizontalMovement = 1;
        }
        else if (horizontal < 0)
        {
            horizontalMovement = -1;
        }
        else
        {
            horizontalMovement = 0;
        }

        if (vertical > 0)
        {
            verticalMovement = 1;
        }
        else if (vertical < 0)
        {
            verticalMovement = -1;
        }
        else
        {
            verticalMovement = 0;
        }

        if (verticalMovement != 0 || horizontalMovement != 0)
        {
            audioManager.PlayAudioWithLoop();
        } else
        {
            audioManager.StopAudio();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        } else
        {
            UpdateOnGround();
        }
        
    }

    private void UpdateOnGround()
    {
        controller.Move(rootMotion + Vector3.down * stepDown);
        rootMotion = Vector3.zero;

        if (!controller.isGrounded)
        {
            SetAirState(0);
        }
    }

    private void UpdateInAir()
    {
        audioManager.StopAudio();
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);
        isJumping = !controller.isGrounded;
        anim.SetBool("isJumping", isJumping);
        rootMotion = Vector3.zero;
    }

    private void OnAnimatorMove()
    {
        rootMotion += anim.deltaPosition;
    }


    private void Jump()
    {
        if (!isJumping)
        {
            audioManager.StopAudio();
            float jumpVelo = Mathf.Sqrt(jumpHeight * -2 * gravity);
            SetAirState(jumpVelo);
        }

    }

    private void SetAirState(float jumpVelo)
    {
        isJumping = true;
        velocity = anim.velocity * jumpDamp;
        velocity.y = jumpVelo;
        anim.SetBool("isJumping", true);
    }
}

