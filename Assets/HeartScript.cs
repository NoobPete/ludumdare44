using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + new Vector3(0, 0.5f) * Mathf.Sin(Time.time);
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 40));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerScript p = other.gameObject.GetComponent<PlayerScript>();
            p.currentHealth += 2;
            Destroy(this.gameObject);
        }
    }
}
