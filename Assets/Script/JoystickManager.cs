using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;


public class JoystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button Joystick")]
    [SerializeField] private Button[] btnJoystick;

    [Header("Movement Joystick")]
    [SerializeField] private Image bgMovementJoystick;
    [SerializeField] private Image iMovementJoystick;
    [SerializeField] private float offset;
    private Vector2 InputDir;

    [Header("Player Walking")]
    [SerializeField] private CharacterController contoller;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject objGerak, objPutar;
    [SerializeField] private float maxWalkSpeed, zAxisPlayer,rotateY;
    [SerializeField] private bool isRunning;

    [Header("Jump")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDinstance = 0.35f;
    [SerializeField] private float jumpHeight = 1.7f;
    [SerializeField] private float gravity = -19.62f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    Vector3 velocityJump;

    [Header("Box Configuration")]
    KardusController box;
    bool isCarried;


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        playerController = playerObj.GetComponent<PlayerController>();
        contoller = playerObj.GetComponent<CharacterController>();
        box = GameObject.FindGameObjectWithTag("Box").GetComponent<KardusController>();
        
        isCarried = box._IsCarried;

        SetCenterJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCarried();
        JumpingJoystick();
    }

    private void CheckCarried()
    {
        GameObject GrabPos = GameObject.Find("GrabPos");

        if (GrabPos.transform.childCount > 0)
        {
            box = GrabPos.transform.GetChild(0).GetComponent<KardusController>();
            isCarried = box._IsCarried;
        }
    }

    private void SetCenterJoystick()
    {
        GameObject joyStick = GameObject.Find("JostickMovement");
        bgMovementJoystick = joyStick.GetComponent<Image>();
        iMovementJoystick = joyStick.transform.GetChild(0).GetComponent<Image>();
        InputDir = Vector2.zero;
    }

   private void SetJoystick()
    {
        for (int i = 0;i < btnJoystick.Length; i++)
        {
            switch (i)
            {
                case 0:
                    btnJoystick[0].onClick.AddListener(JumpingJoystick);
                    break;

                case 1:
                    btnJoystick[1].onClick.AddListener(box.GrabJoystick);
                    break;

                case 2:
                    btnJoystick[2].onClick.AddListener(box.DropJoystick);
                    break;

                case 3:
                    btnJoystick[3].onClick.AddListener(box.ThrowJoystick);
                    break;

                default:
                    Debug.Log("Setup Btn Joystick Complate");
                    break;
            }
        }
    }

    public void JumpingJoystick()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDinstance, groundMask);

        if (isGrounded && velocityJump.y < 0)
            velocityJump.y = -2f;

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
        {
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocityJump.y += gravity * Time.deltaTime;
        contoller.Move(velocityJump * Time.deltaTime);
        playerController._IsGrounded = isGrounded;
    }

    #region Joystick Movement
    public void OnDrag(PointerEventData eventData)
    {

        Vector2 pos = Vector2.zero;
        float bgimageSizeX = bgMovementJoystick.rectTransform.sizeDelta.x;
        float bgimageSizeY = bgMovementJoystick.rectTransform.sizeDelta.y;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgMovementJoystick.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x /= bgimageSizeX;
            pos.y /= bgimageSizeY;
            InputDir = new Vector2(pos.x, pos.y);
            InputDir = InputDir.magnitude > 1 ? InputDir.normalized : InputDir;

            // Menentukan Arah Gerak Player
            iMovementJoystick.rectTransform.anchoredPosition = new Vector2(InputDir.x * (bgimageSizeX / offset), InputDir.y * (bgimageSizeY / offset));

            // Menentukan Aarah Putar Player
            zAxisPlayer = Mathf.Atan2(-InputDir.y, InputDir.x) * Mathf.Rad2Deg;
            objPutar.transform.eulerAngles = new Vector3(0, zAxisPlayer+rotateY, 0);
            playerController._IsRunning = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        InputDir = Vector2.zero;
        playerController._IsRunning = false;
        iMovementJoystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    private void LateUpdate()
    {
        objGerak.transform.Translate(-InputDir.y * maxWalkSpeed * Time.deltaTime, 0, InputDir.x * maxWalkSpeed * Time.deltaTime);

    }

    #endregion
}
