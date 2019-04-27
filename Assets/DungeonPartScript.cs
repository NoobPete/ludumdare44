using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPartScript : MonoBehaviour
{
    public Bounds b;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider bc = this.gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        b = GetMaxBounds(this.gameObject);
        bc.isTrigger = true;
        bc.size = b.size;
        bc.center = b.center - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Bounds GetMaxBounds(GameObject g)
    {
        var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("New Part: I have a collision deliting myself");
        Destroy(this.gameObject);
    }
}
