using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    public string VerticalMoveInputAxis = "Vertical";
    public string HorizontalMoveInputAxis = "Horizontal";
    public string HorizontalTurnInputAxis = "Horizontal";

    // rotation that occurs in angles per second holding down input
    public float rotationRate = 360;

    // units moved per second holding down move input
    public float moveRate = 10;

    private Rigidbody rb;
    public float rotAngle = 0;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveAxis = Input.GetAxis(VerticalMoveInputAxis);
        float moveHorizontalAxis = Input.GetAxis(HorizontalMoveInputAxis);
        float turnAxis = Input.GetAxis(HorizontalTurnInputAxis);



        if (Input.GetAxis(HorizontalMoveInputAxis) == 0 && Input.GetAxis(VerticalMoveInputAxis) != 0)
            Move(moveAxis);

        if (Input.GetAxis(HorizontalMoveInputAxis) != 0 && Input.GetAxis(VerticalMoveInputAxis) == 0)
            Move(moveHorizontalAxis);

        if (Input.GetAxis(HorizontalMoveInputAxis) != 0 && Input.GetAxis(VerticalMoveInputAxis) != 0)
            Turn(turnAxis);

    }



    private void Move(float input)
    {
        // Make sure to set drag high so the sliding effect is very minimal (5 drag is acceptable for now)

        // mention this trash function automatically converts to local space
        rb.AddForce(transform.forward * input * moveRate, ForceMode.Force);
        print ("moving");
    }

    private void Turn(float input)
    {
        Vector3 from = new Vector3(0f, 0f, 1f);
        Vector3 to = new Vector3(Input.GetAxis(HorizontalTurnInputAxis), 0f, Input.GetAxis(VerticalMoveInputAxis));
        //transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
        rotAngle = Vector3.SignedAngle(from, to, Vector3.up);
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotAngle, ref turnSmoothVelocity, turnSmoothTime);
        rb.AddForce(transform.forward * input * moveRate, ForceMode.Force);
        print("turning and moving");
    }



}
