using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 6f;
    public float hitForce = 1000f;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        Vector3 rayOrigin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, speed * Time.deltaTime, layerMask))
        {
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();
            if (health != null)
            {
                health.Damage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            Debug.Log(hit.collider.gameObject.name);
            Destroy(this.gameObject);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
