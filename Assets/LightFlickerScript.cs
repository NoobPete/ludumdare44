using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlickerScript : MonoBehaviour
{
    private Light l;
    public float rangeChange = 1f;
    public float speedMultipler = 1f;
    private float startRange;
    private float offset;


    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
        offset = UnityEngine.Random.Range(0f, 1000f);
        startRange = l.range;
    }

    // Update is called once per frame
    void Update()
    {
        l.range = startRange + Mathf.PerlinNoise(offset + speedMultipler * Time.time, 0) * rangeChange;
    }
}
