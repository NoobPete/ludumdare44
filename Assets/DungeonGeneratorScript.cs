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

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(startPart, new Vector3(0, 0, 0), Quaternion.identity);

        foreach (Transform t in GetDoors(o.transform))
        {
            unfinishedDoors.Add(t);
        }
    }

    public void BuildOnePart()
    {
        if (unfinishedDoors.Count != 0)
        {
            Transform originalDoor = unfinishedDoors[0];
            unfinishedDoors.Remove(originalDoor);

            GameObject newPart = Instantiate(partList[0], new Vector3(0, 0, 0), Quaternion.identity);

            List<Transform> newDoors = GetDoors(newPart.transform);

            Transform newDoorToMatch = newDoors[0];
            newDoors.Remove(newDoorToMatch);

            newPart.transform.rotation = originalDoor.transform.rotation * Quaternion.Inverse(newDoorToMatch.rotation) * Quaternion.Euler(0, 180, 0);
            newPart.transform.position = originalDoor.transform.position - (newDoorToMatch.position - newPart.transform.position);

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