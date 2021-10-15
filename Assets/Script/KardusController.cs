using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KardusController : MonoBehaviour
{
    private PlayerController player;
    private GameObject grabPos;

    GameStoryController gameController;
  
    [SerializeField] private float distance, throwForce;
    [SerializeField] private bool isAbleToGrab,isRunning,isGrounded,isCarried;


    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameStoryController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        grabPos = GameObject.Find("GrabPos");

        isAbleToGrab = false;
        isCarried = false;
    }

    private void Update()
    {
        Grabing();
        Throwing();
    }

    private void FixedUpdate()
    {
        CheckBool();
    }

    private void CheckBool()
    {
        isRunning = player._IsRunning;
        isGrounded = player._IsGrounded;
    }

    private void Grabing()
    {
        if(gameController.isOpened == false)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, grabPos.transform.position);

            if (distance <= 0.9f)
                isAbleToGrab = true;
            else
                isAbleToGrab = false;

            if (isGrounded && !isRunning)
            {
                if (isAbleToGrab && Input.GetKeyDown(KeyCode.G))
                {
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    this.transform.position = grabPos.transform.position;
                    this.transform.parent = grabPos.transform;
                    isCarried = true;
                    player.PlayerAnim.SetBool("Box", true);
                    player.PlayerAnim.ResetTrigger("JumpThrow");
                    player.PlayerAnim.ResetTrigger("RunThrow");
                }
            }
        }
    }

    private void Throwing()
    {
        if (gameController.isOpened == false)
        {
            if (isCarried && Input.GetKey(KeyCode.LeftShift))
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.transform.parent = null;
                isCarried = false;
                this.GetComponent<Rigidbody>().AddForce(grabPos.transform.forward * throwForce);
                player.PlayerAnim.SetBool("Box", false);
                player.PlayerAnim.SetBool("Idle", true);

                if (isRunning)
                    player.PlayerAnim.SetTrigger("RunThrow");
                else if (!isGrounded)
                    player.PlayerAnim.SetTrigger("JumpThrow");

                if (!isGrounded && isRunning)
                    player.PlayerAnim.SetTrigger("JumpThrow");
            }
            else if (isCarried && Input.GetKey(KeyCode.LeftControl))
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.transform.parent = null;
                isCarried = false;
                player.PlayerAnim.SetBool("Box", false);
                player.PlayerAnim.SetBool("Idle", true);

                if (isRunning)
                    player.PlayerAnim.SetTrigger("RunThrow");
                else if (!isGrounded)
                    player.PlayerAnim.SetTrigger("JumpThrow");

                if (!isGrounded && isRunning)
                    player.PlayerAnim.SetTrigger("JumpThrow");
            }
        }       
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag=="BoxTrigger")
        {
            DataBase.SetCurrentProgres("Box", DataBase.GetCurrentProgres("Box")+1);
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public bool _IsCarried { get { return isCarried; } }

}
