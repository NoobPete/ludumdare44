using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTowardsPlayerScript : MonoBehaviour
{
    public GameObject target;
    public Rigidbody rb;
    public float MovePower;
    [Header("Jump")]
    public float minJumpTime;
    public float maxJumpTime;
    public float jumpPower;
    private float jumpTimer;
    private float timeSinceLastJump = 0f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();

        SetNewJumpTimer();
    }

    private void SetNewJumpTimer()
    {
        timeSinceLastJump = 0f;
        jumpTimer = UnityEngine.Random.Range(minJumpTime, maxJumpTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastJump += Time.deltaTime;
        if (timeSinceLastJump > jumpTimer)
        {
            SetNewJumpTimer();
            rb.AddForce(Vector3.up * jumpPower);
        }

        transform.LookAt(target.transform);
        rb.AddForce(transform.forward * MovePower * Time.deltaTime);
    }
}
