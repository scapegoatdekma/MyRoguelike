using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    private float chunkSize = 20;
    PlayerMovement pm;

    public Transform parent;

    [Header("Optimisation")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;


    public List<GameObject> vectors;
 

    void Start()
    {
        pm = FindAnyObjectByType<PlayerMovement>();
    }

  
    void Update()
    {
        ChunkChecker();
        ChunkOptimiser();
    }

    void ChunkChecker()
    {

        if (!currentChunk)
        {
            return;
        }

        if (pm.moveDir.y != 0 || pm.moveDir.x != 0)
        {


            vectors[0].transform.position = currentChunk.transform.position + new Vector3(0, chunkSize, 0); //up
            vectors[1].transform.position = currentChunk.transform.position + new Vector3(0, -chunkSize, 0); //down

            vectors[2].transform.position = currentChunk.transform.position + new Vector3(chunkSize, 0, 0); // r
            vectors[3].transform.position = currentChunk.transform.position + new Vector3(-chunkSize, 0, 0); // l

            vectors[4].transform.position = currentChunk.transform.position + new Vector3(chunkSize, chunkSize, 0); // ur
            vectors[5].transform.position = currentChunk.transform.position + new Vector3(-chunkSize, chunkSize, 0); // ul

            vectors[6].transform.position = currentChunk.transform.position + new Vector3(chunkSize, -chunkSize, 0); // dr
            vectors[7].transform.position = currentChunk.transform.position + new Vector3(-chunkSize, -chunkSize, 0); // dl

            for (int i = 0; i < vectors.Count; i++)
            {
                if (!Physics2D.OverlapCircle(vectors[i].transform.position, checkerRadius, terrainMask)) // u
                {
                    SpawnChunk(vectors[i].transform.position);
                }
            }
        }
    }

    void SpawnChunk(Vector3 positionToSpawn)
    {

        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], positionToSpawn, Quaternion.identity);
        latestChunk.transform.SetParent(parent);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimiser()
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else return;

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist) { chunk.SetActive(false); } 
            else { chunk.SetActive(true); }
        }
    }
}
