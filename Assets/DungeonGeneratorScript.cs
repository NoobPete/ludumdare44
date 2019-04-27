using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonGeneratorScript : MonoBehaviour
{
    public GameObject startPart;
    public List<GameObject> partList = new List<GameObject>();
    public List<Transform> unfinishedDoors = new List<Transform>();
    public List<Bounds> dungeonBounds = new List<Bounds>();
    public int numberOfRoomToGenrate = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(startPart, new Vector3(0, 0, 0), Quaternion.identity);

        // Make it detect detection
        Bounds bounds = GetMaxBounds(o);
        float boundsMargin = 1f;
        bounds.size = bounds.size - new Vector3(boundsMargin, 0, boundsMargin);

        dungeonBounds.Add(bounds);

        foreach (Transform t in GetDoors(o.transform))
        {
            unfinishedDoors.Add(t);
        }
        
        while(numberOfRoomToGenrate > 0)
        {
            numberOfRoomToGenrate--;
            BuildOnePart();
        }
    }

    public void BuildOnePart()
    {
        if (unfinishedDoors.Count != 0)
        {
            // Find door to expand
            Transform originalDoor = unfinishedDoors[0];
            unfinishedDoors.Remove(originalDoor);

            // Spawn the object
            GameObject partPrefab = partList[UnityEngine.Random.Range(0, partList.Count)];
            GameObject newPart = Instantiate(partPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Match the doors
            List<Transform> newDoors = GetDoors(newPart.transform);

            Transform newDoorToMatch = newDoors[UnityEngine.Random.Range(0, newDoors.Count)];
            newDoors.Remove(newDoorToMatch);

            newPart.transform.rotation = originalDoor.transform.rotation * Quaternion.Inverse(newDoorToMatch.rotation) * Quaternion.Euler(0, 180, 0);
            newPart.transform.position = originalDoor.transform.position - (newDoorToMatch.position - newPart.transform.position);

            // Make it detect detection
            Bounds bounds = GetMaxBounds(newPart);
            float boundsMargin = 1f;
            bounds.size = bounds.size - new Vector3(boundsMargin, 0, boundsMargin);

            foreach (Bounds b in dungeonBounds)
            {
                if (bounds.Intersects(b))
                {
                    Destroy(newPart);
                    Debug.Log("Bounds has collision adding failled");
                    return;
                }
            }

            dungeonBounds.Add(bounds);

            // Add new doors as unfinished
            foreach (Transform t in newDoors)
            {
                unfinishedDoors.Add(t);
            }

        } else
        {
            Debug.LogWarning("Now places left to place a part");
        }
    }

    private List<Transform> GetDoors(Transform o)
    {
        List<Transform> result = new List<Transform>();

        for (int i = 0; i < o.childCount; i++)
        {
            if (o.GetChild(i).tag == "Door")
            {
                result.Add(o.GetChild(i));
            }
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        bool first = true;

        foreach (Transform t in unfinishedDoors)
        {
            if (first)
            {
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                first = false;
            }
            else
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
            }

            Gizmos.DrawCube(t.position, new Vector3(1, 1, 1));
        }

        Gizmos.color = new Color(0, 0, 1, 0.5f);
        foreach (Bounds b in dungeonBounds)
        {
            Gizmos.DrawWireCube(b.center, b.size);
        }
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
}

[CustomEditor(typeof(DungeonGeneratorScript))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonGeneratorScript myScript = (DungeonGeneratorScript)target;
        if (GUILayout.Button("Build Next Part"))
        {
            myScript.BuildOnePart();
        }
    }
}