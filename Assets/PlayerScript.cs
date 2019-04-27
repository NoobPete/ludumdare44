using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float LookSentitivity;
    public float MovementSpeed;
    public GameObject head;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * LookSentitivity);
        head.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * LookSentitivity);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * Input.GetAxis("Vertical") * MovementSpeed + transform.right * Input.GetAxis("Horizontal") * MovementSpeed);
    }
}
