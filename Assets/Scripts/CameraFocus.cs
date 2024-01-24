using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform target;  // Reference to the player's Transform
    public Vector3 offset = new Vector3(0f, 5f, -10f);  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired camera position based on the player's position and offset
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);

            // Make the camera look at the player
            transform.LookAt(target.position);
        }

    }
}
