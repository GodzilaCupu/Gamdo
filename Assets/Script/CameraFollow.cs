using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Range(1f, 10f)]
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX,maxX;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;


    float xBoundaries;
    void LateUpdate()
    {
        xBoundaries = Mathf.Clamp(target.position.x,minX, maxX);
        Vector3 followpos = new Vector3(xBoundaries+offset.x, this.gameObject.transform.position.y, target.position.z + offset.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, followpos, ref velocity, smoothSpeed * Time.deltaTime);

        transform.position = smoothPos;
    }

}
