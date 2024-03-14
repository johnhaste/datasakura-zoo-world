using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //Singleton
    public static ObjectSpawner Instance;

    [Header("Settings")]
    public bool isActive = true;
    public float minWait = 1f;
    public float maxWait = 3f;
    public int offSet = 3;

    [Header("Prefabs")]
    public List<GameObject> Prefabs = new List<GameObject>();
    
    [Header("Counters")]
    public int spawnedObjects = 0;

    [Header("Boundaries")]
    public GameObject StageBoundsLeft;
    public GameObject StageBoundsRight;
    public GameObject StageBoundsTop;
    public GameObject StageBoundsBottom;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPrefab());
    }

    IEnumerator SpawnPrefab()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            //Random prefab
            int randomIndex = Random.Range(0, Prefabs.Count);

            //Boundaries
            int StageBoundsLeftX = (int)StageBoundsLeft.transform.position.x + offSet;
            int StageBoundsRightX = (int)StageBoundsRight.transform.position.x - offSet;
            int StageBoundsTopZ = (int)StageBoundsTop.transform.position.z - offSet;
            int StageBoundsBottomZ = (int)StageBoundsBottom.transform.position.z + offSet;

            //Generate random spawn position
            Vector3 spawnPosition = new Vector3(Random.Range(StageBoundsLeftX, StageBoundsRightX), 0, Random.Range(StageBoundsBottomZ, StageBoundsTopZ));

            //Spawning the Object
            Instantiate(Prefabs[randomIndex], spawnPosition, Quaternion.identity);

            //Counting the spawned objects
            spawnedObjects++;
        }
    }
}
