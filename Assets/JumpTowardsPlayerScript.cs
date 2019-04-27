using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTowardsPlayerScript : MonoBehaviour
{
    public PlayerScript target;
    public Rigidbody rb;
    public float MovePower;
    [Header("Jump")]
    public float minJumpTime;
    public float maxJumpTime;
    public float jumpPower;
    private float jumpTimer;
    private float timeSinceLastJump = 0f;

    private bool decreaseHealth = false;
    private PlayerScript toRemoveHealthFrom;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<PlayerScript>();
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

        /*ShootableBox health = hit.collider.GetComponent<ShootableBox>();
        if (collide)
        {
            if (health != null)
            {
                health.Damage(gunDamage);
            }
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            decreaseHealth = true;
        }
    }

    private void FixedUpdate()
    {
        if (decreaseHealth == true)
        {
            target.Damage(2);
            decreaseHealth = false;
        }
    }
}
