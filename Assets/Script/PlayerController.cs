using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private CharacterController controller;

    [Header("Walking")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float smothingTurn = 1f;
    private float smothingVelocityTurn;

    [Header("Jumping")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDinstance = 0.1f;
    [SerializeField] private float jumpHeight = 1.9f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;

    Vector3 velocityJump;

    private void Update()
    {
        Jumping();
        Moving();
    }

    private void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(-vertical, 0f, horizontal).normalized;

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

        if (isGrounded && velocityJump.y < 0)
            velocityJump.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocityJump.y += gravity * Time.deltaTime;
        controller.Move(velocityJump * Time.deltaTime);
    }
}
