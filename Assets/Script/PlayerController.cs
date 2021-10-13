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
    KardusController box;
    bool isCarried;

    private void Start()
    {
        box = GameObject.Find("kardus").GetComponent<KardusController>();
        isCarried = box._IsCarried;
        playerAnim.GetComponent<Animator>();
    }

    private void Update()
    {
        Jumping();
        Moving();
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

        if (isRunning && isCarried)
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

    #region Get-Set Method
    public bool _IsRunning { get { return isRunning; } }
    public bool _IsGrounded { get { return isGrounded; } }
    public Animator PlayerAnim { get { return playerAnim; } set { playerAnim = value; } }
    #endregion
}
