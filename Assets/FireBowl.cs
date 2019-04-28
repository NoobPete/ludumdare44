using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBowl : MonoBehaviour
{
    public float chance = 0.25f;
    public GameObject[] toDelete;

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.Random.value < chance)
        {
            foreach (GameObject o in toDelete)
            {
                Destroy(o);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
