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
    public List<Bounds> dungeonBoundsUnvisited = new List<Bounds>();
    public int numberOfRoomToGenrate = 0;
    public int maxTriesPerPart = 10;
    public GameObject[] monsterList;
    private List<GameObject> parts = new List<GameObject>();
    public GameObject player;
    public int monstersPerSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(startPart, new Vector3(0, 0, 0), Quaternion.identity);

        // Make it detect detection
        Bounds bounds = GetMaxBounds(o);
        float boundsMargin = 1f;
        bounds.size = bounds.size - new Vector3(boundsMargin, 0, boundsMargin);

        dungeonBounds.Add(bounds);
        parts.Add(o);

        foreach (Transform t in GetDoors(o.transform))
        {
            unfinishedDoors.Add(t);
        }
        
        while(numberOfRoomToGenrate > 0)
        {
            numberOfRoomToGenrate--;
            BuildOnePart();
        }

        CloseAllUnfinished();

        foreach (Bounds b in dungeonBounds)
        {
            Bounds newB = b;
            float margin = 1f;
            bounds.size = bounds.size - new Vector3(margin, 0, margin);

            dungeonBoundsUnvisited.Add(newB);
        }
    }

    private List<GameObject> SpawnMonsterInRoom(GameObject part)
    {
        MonsterSpwanerScript[] msss = part.gameObject.GetComponentsInChildren<MonsterSpwanerScript>();

        List<GameObject> monsters = new List<GameObject>();

        foreach (MonsterSpwanerScript mss in msss)
        {
            for (int i = 0; i < monstersPerSpawn; i++)
            {
                monsters.Add(Instantiate(monsterList[UnityEngine.Random.Range(0, monsterList.Length)], mss.transform.position, Quaternion.identity));
            }
        }

        return monsters;
    }

    public void BuildOnePart()
    {
        if (unfinishedDoors.Count != 0)
        {
            // Find door to expand
            Transform originalDoor = unfinishedDoors[UnityEngine.Random.Range(0, unfinishedDoors.Count)];
            unfinishedDoors.Remove(originalDoor);

            int triesLeft = maxTriesPerPart;
            outherWhile:
            while (triesLeft > 0)
            {
                triesLeft--;

                // Spawn the object
                int partNumber = UnityEngine.Random.Range(0, partList.Count);
                GameObject partPrefab = partList[partNumber];
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
                        Debug.Log("Bounds has collision adding failled. Part id: " + partNumber);
                        goto outherWhile;
                    }
                }

                dungeonBounds.Add(bounds);
                parts.Add(newPart);

                // Add new doors as unfinished
                foreach (Transform t in newDoors)
                {
                    unfinishedDoors.Add(t);
                }

                break;
            }

            if (triesLeft == 0)
            {
                Debug.Log("Closing door");
                DoorScript ds = originalDoor.GetComponent<DoorScript>();
                ds.ClosePermanent();
            }
        } else
        {
            Debug.LogWarning("Now places left to place a part");
        }
    }

    public void CloseAllUnfinished()
    {
        foreach (Transform door in unfinishedDoors)
        {
            DoorScript ds = door.GetComponent<DoorScript>();
            ds.ClosePermanent();
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
        for (int i = 0; i < dungeonBoundsUnvisited.Count; i++)
        {
            if (dungeonBoundsUnvisited[i].ClosestPoint(player.transform.position) == player.transform.position)
            {
                List<GameObject> monsters = SpawnMonsterInRoom(parts[i]);
                GameObject part = parts[i];

                dungeonBoundsUnvisited.RemoveAt(i);
                parts.RemoveAt(i);

                if (monsters.Count != 0)
                {
                    DoorScript[] dss = part.GetComponentsInChildren<DoorScript>();
                    foreach (DoorScript ds in dss)
                    {
                        ds.CloseUntilNull(monsters);
                    }
                }
            }
        }
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