using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRotation : MonoBehaviour
{
    public float playerTurnSmoothing = 10f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float cameraRigRot;

    private void Awake()
    {
        GameObject camera = FindObjectOfType<CameraControls>().gameObject;
        if (camera)
            cameraRigRot = camera.transform.rotation.eulerAngles.y;
    }

    public void Rotate(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        Quaternion correctedQ;
        correctedQ = Quaternion.LookRotation(Quaternion.AngleAxis(cameraRigRot, Vector3.up) * direction); //Correct the rotation quaternion 
        transform.rotation = Quaternion.Slerp(transform.rotation, correctedQ, Time.deltaTime * playerTurnSmoothing); //Rotate the player, and LERP THE ROTATION... 
    }
}
