using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float LookSentitivity;
    public float MovementSpeed;
    public float JumpPower;
    public float Gravity;
    public GameObject head;
    // private Rigidbody rb;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();

        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * LookSentitivity);
        head.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * LookSentitivity);
        // rb.MovePosition(transform.position + transform.forward * Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime + transform.right * Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime);

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = Input.GetAxis("Horizontal") * transform.right + transform.forward * Input.GetAxis("Vertical");
            moveDirection *= MovementSpeed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = JumpPower;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= Gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void FixedUpdate()
    {

    }
}
