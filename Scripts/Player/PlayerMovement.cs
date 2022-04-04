using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    // Gravity
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask ground;
    private float gravity = -9.8f;
    private Vector3 velocity;

    [SerializeField] private float jumpHeight;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float horizontalMovement, verticalMovement;

        if (horizontal > 0)
        {
            horizontalMovement = 1;
        } else if (horizontal < 0)
        {
            horizontalMovement = -1;
        } else
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
        //moveDir = transform.TransformDirection(moveDir);

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, ground);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y -= 2f;
        }

        if (!isGrounded)
        {

            anim.SetFloat("HorizontalSpeed", horizontalMovement, 0.1f, Time.deltaTime);
            anim.SetFloat("VerticalSpeed", verticalMovement, 0.1f, Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}

