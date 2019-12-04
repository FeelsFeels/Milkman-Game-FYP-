using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovement : MonoBehaviour
{
    public float playerTurnSmoothing = 10f;
    public float moveRate = 10;  // units moved per second holding down move input
    float cameraRigRot;

    private void Awake()
    {
        GameObject camera = FindObjectOfType<CameraControls>().gameObject;
        if (camera)
            cameraRigRot = camera.transform.rotation.eulerAngles.y;
    }

    public void Move(Vector3 direction)
    {
        // Movement based on camera's rotation at the start.
        Vector3 velocity = Quaternion.AngleAxis(cameraRigRot, Vector3.up) * (direction * moveRate); //Multiply the direction by the camera rotation
        Vector3 movement = velocity * Time.deltaTime;
        transform.Translate(movement, Space.World);

        //Quaternion correctedQ;
        //correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * direction); //Correct the rotation quaternion 
        //transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
    }
}
