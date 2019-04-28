using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject blocker;
    private bool locked = false;
    private bool lockedPerm = false;
    private List<GameObject> listToWatch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockedPerm && listToWatch != null)
        {
            foreach (GameObject o in listToWatch)
            {
                if (o != null)
                {
                    return;
                }
            }

            locked = false;
            blocker.SetActive(false);
        }
    }

    public void ClosePermanent()
    {
        locked = true;
        lockedPerm = true;
        blocker.SetActive(true);
    }

    internal void CloseUntilNull(List<GameObject> monsters)
    {
        locked = true;
        blocker.SetActive(true);
        listToWatch = monsters;
    }
}
