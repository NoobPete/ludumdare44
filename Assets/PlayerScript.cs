using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float LookSentitivity;
    public float MovementSpeed;
    public float JumpPower;
    public float Gravity;
    public GameObject head;
    // private Rigidbody rb;
    public int currentHealth = 10;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    public TextMeshProUGUI healthText;

    [Header("Step")]
    public Transform feetPosition;
    public float StepSoundColddown = 0.1f;
    private float NextStep = 0f;

    private float rotationY = 0f;

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
        // head rotation        
        rotationY += Input.GetAxis("Mouse Y") * LookSentitivity;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        head.transform.localEulerAngles = new Vector3(-rotationY, head.transform.localEulerAngles.y, 0); //head.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * LookSentitivity);
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
            } else
            {
                if (NextStep < Time.time && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1))
                {
                    NextStep = Time.time + StepSoundColddown;
                    AudioManeger.main.Play(AudioManeger.main.StepSound, feetPosition.position);
                }
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= Gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetButton("Weapon1"))
        {
            print("weapon1 button works!");
        }
        if (Input.GetButton("Weapon2"))
        {
            print("weapon2 button works!");
        }

        healthText.text = currentHealth.ToString();
    }

    void FixedUpdate()
    {

    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;                       
    }
}
