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

    public bool isUpRight = false;

    [Header("Attack Jump")]
    public bool isJumpAttacking = false;
    public float jumpAttackPower = 2000f;
    public float jumpAttackColdDown = 6f;
    public float jumpAttackDistance = 10f;
    private float nextJumpAttackTime = 0f;

    [Header("Shooting Attack")]
    public bool isShooting = false;
    public GameObject shotPrefab;
    public float shotColdDown = 3f;
    private float nextShot = 0;

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

        if (isJumpAttacking)
        {
            if (nextJumpAttackTime < Time.time && Vector3.Distance(this.transform.position, target.transform.position) < jumpAttackDistance) {
                nextJumpAttackTime = Time.time + jumpAttackColdDown;
                rb.AddForce(transform.forward * jumpAttackPower);
            }
        }

        if (isUpRight)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        if (isShooting)
        {
            if (nextShot < Time.time)
            {
                nextShot = Time.time + shotColdDown;
                Vector3 relativePos = target.transform.position - this.transform.position;
                Destroy(Instantiate(shotPrefab, transform.position, Quaternion.LookRotation(relativePos, Vector3.up)), 10f);
            }
        }
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
