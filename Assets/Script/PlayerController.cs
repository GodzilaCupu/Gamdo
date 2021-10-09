using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float walkSpeed;
    private float smothingTurn = 0.1f;
    private float smothingVelocityTurn;

    //Jumping
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public LayerMask groundMask;

    public Transform groundCheck;
    public float groundDinstance = 0.4f;

    Vector3 velocityJump;
    bool isGrounded;

    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        Jumping();
        Moving();

        Debug.Log("Velocity Jump" + velocityJump);
    }

    private void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smothingVelocityTurn, smothingTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * walkSpeed * Time.deltaTime);
        }
    }

    private void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDinstance, groundMask);
        //Checking gravity
        if (isGrounded && velocityJump.y < 0)
            velocityJump.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        velocityJump.y += gravity * Time.deltaTime;
        controller.Move(velocityJump * Time.deltaTime);

    }
}
