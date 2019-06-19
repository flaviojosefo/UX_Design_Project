using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public List<GameObject> nPCs;

    private int maxNPCs;
    [HideInInspector]
    public int currentNPCs;
    private float timer;
    private int npcToSpawn;
    private int pointToSpawn;

    private void Start() {
        maxNPCs = 6;
        currentNPCs = 0;
        timer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (currentNPCs != maxNPCs && timer > 15) {
            npcToSpawn = Random.Range(0, 4);
            pointToSpawn = Random.Range(0, 4);
            Vector3 localSpawn = spawnPoints[pointToSpawn].transform.position;
            Instantiate(nPCs[npcToSpawn], localSpawn, spawnPoints[pointToSpawn].transform.rotation);
            currentNPCs++;
            timer = 0;
        }
    }
}
