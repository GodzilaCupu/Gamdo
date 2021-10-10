using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour
{
    public GameObject grabPos;
    public float distance, throwForce;
    public bool isAbleToGrab = false;
    public bool isCarried = false;

    private void Update()
    {
        Throw();
    }

    private void Throw()
    {
        distance = Vector3.Distance(this.gameObject.transform.position, grabPos.transform.position);

        if (distance <= 1.4f)
            isAbleToGrab = true;
        else
            isAbleToGrab = false;


        if (isAbleToGrab && Input.GetKeyDown(KeyCode.F))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.position = grabPos.transform.position;
            this.transform.parent = grabPos.transform;
            isCarried = true;
        }

        if (isCarried && Input.GetKey(KeyCode.LeftControl))
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.parent = null;
            isCarried = false;
        }
        else
        if (isCarried && Input.GetKey(KeyCode.LeftShift))
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.parent = null;
            isCarried = false;
            GetComponent<Rigidbody>().AddForce(grabPos.transform.forward * throwForce);
        }

    }
}
