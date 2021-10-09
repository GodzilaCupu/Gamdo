using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 followpos = new Vector3(target.position.x + offset.x, this.gameObject.transform.position.y, target.position.z + offset.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, followpos, ref velocity, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }

}
