using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator playerAnim;

    [Header("Walking")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float smothingTurn = 1f;
    [SerializeField] private bool isRunning;
    private float smothingVelocityTurn;

    [Header("Jumping")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDinstance = 0.1f;
    [SerializeField] private float jumpHeight = 1.9f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;

    Vector3 velocityJump;

    [Header("Throw")]
    [SerializeField] private GameObject grabPos;
    [SerializeField] private float throwForce;
    private bool isAbleToGrab = false;
    private bool isCarried = false;

    private float distance;
    private GameObject box;

    private void Start()
    {
        playerAnim.GetComponent<Animator>();
        box = GameObject.FindGameObjectWithTag("Box");
    }

    private void Update()
    {
        Jumping();
        Moving();
        Throw();
        CheckAnim();
    }

    private void Moving()
    {
        isRunning = false;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(-vertical, 0f, horizontal).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smothingVelocityTurn, smothingTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * walkSpeed * Time.deltaTime);
            isRunning = true;
        }
    }

    private void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDinstance, groundMask);

        if (isGrounded && velocityJump.y < 0)
            velocityJump.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocityJump.y += gravity * Time.deltaTime;
        controller.Move(velocityJump * Time.deltaTime);
    }

    private void Throw()
    {
        distance = Vector3.Distance(box.gameObject.transform.position, grabPos.transform.position);

        if (distance <= 1.4f)
            isAbleToGrab = true;
        else
            isAbleToGrab = false;

        if (isGrounded && !isRunning)
        {
            if (isAbleToGrab && Input.GetKeyDown(KeyCode.F))
            {
                box.GetComponent<Rigidbody>().isKinematic = true;
                box.transform.position = grabPos.transform.position;
                box.transform.parent = grabPos.transform;
                isCarried = true;
                playerAnim.SetBool("Box", true);
                playerAnim.ResetTrigger("JumpThrow");
                playerAnim.ResetTrigger("RunThrow");
            }
        }
        if (isCarried && Input.GetKey(KeyCode.LeftControl))
        {
            box.GetComponent<Rigidbody>().isKinematic = false;
            box.transform.parent = null;
            isCarried = false;
            playerAnim.SetBool("Box", false);
            playerAnim.SetBool("Idle", true);

            if (isRunning)
                playerAnim.SetTrigger("RunThrow");
            else if (!isGrounded)
                playerAnim.SetTrigger("JumpThrow");
            
            if (!isGrounded && isRunning)
                playerAnim.SetTrigger("JumpThrow");
        }
        else if (isCarried && Input.GetKey(KeyCode.LeftShift))
        {
            box.GetComponent<Rigidbody>().isKinematic = false;
            box.transform.parent = null;
            isCarried = false;
            box.GetComponent<Rigidbody>().AddForce(grabPos.transform.forward * throwForce);
            playerAnim.SetBool("Box", false);
            playerAnim.SetBool("Idle", true);

            if (isRunning)
                playerAnim.SetTrigger("RunThrow");
            else if (!isGrounded)
                playerAnim.SetTrigger("JumpThrow");
            
            if (!isGrounded && isRunning)
                playerAnim.SetTrigger("JumpThrow");

        }
    }


    private void CheckAnim()
    {

        if (isRunning)
        {
            playerAnim.SetBool("Run", true);
        }
        else
        {
            playerAnim.SetBool("Run", false);
        }

        if (isGrounded)
        {
            playerAnim.SetBool("Jump", false);
            playerAnim.SetBool("Idle", true);
        }
        else
        {
            playerAnim.SetBool("Jump", true);
            playerAnim.SetBool("Idle", false);
        }

        if (isRunning && !isGrounded)
        {
            playerAnim.SetBool("Idle", false);
            playerAnim.SetBool("Jump", true);
            playerAnim.SetBool("Run", true);
        }
        else if (isRunning && isGrounded)
        {
            playerAnim.SetBool("Idle", false);
            playerAnim.SetBool("Jump", false);
            playerAnim.SetBool("Run", true);
        }

        if(isRunning && isCarried)
        {
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Run", true);
        }
        else if (!isRunning && isCarried)
        {
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Run", false);
        }

        if (isGrounded && isCarried)
        {
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Jump", false);
        }
        else if (!isGrounded && isCarried)
        {
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Jump", true);
        }

        if (isGrounded && isCarried && isRunning)
        {
            playerAnim.SetBool("Jump", false);
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Run", true);
        } 
        else if (!isGrounded && isCarried && isRunning)
        {
            playerAnim.SetBool("Box", true);
            playerAnim.SetBool("Run", true);
            playerAnim.SetBool("Jump", true);
        }



    }
}
