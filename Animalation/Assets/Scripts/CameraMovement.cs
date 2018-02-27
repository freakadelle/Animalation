using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float cameraZoom = 35;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPosition = target.TransformPoint(target.position.x, target.position.y, 0);
        if(target != null)
        {
            Vector3 targetPosition = target.position + (Vector3.back * cameraZoom);
            velocity = new Vector3(velocity.x, velocity.y, 0);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
            //transform.position = target.position + (Vector3.back * 10);
        }

    }
}
