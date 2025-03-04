using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomiser : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

   
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach(GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity);
            prop.transform.position = new Vector3 (prop.transform.position.x, prop.transform.position.y, -1);
            prop.transform.parent = sp.transform;
        }
    }
}
