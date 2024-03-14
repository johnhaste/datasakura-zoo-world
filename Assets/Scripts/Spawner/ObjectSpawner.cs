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
    private int spawnOffSet = 3;

    [Header("Prefabs")]
    public List<GameObject> Prefabs = new List<GameObject>();
    public GameObject SpawnAreaChecker;

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
            int StageBoundsLeftX = (int)StageBoundsLeft.transform.position.x + spawnOffSet;
            int StageBoundsRightX = (int)StageBoundsRight.transform.position.x - spawnOffSet;
            int StageBoundsTopZ = (int)StageBoundsTop.transform.position.z - spawnOffSet;
            int StageBoundsBottomZ = (int)StageBoundsBottom.transform.position.z + spawnOffSet;

            SpawnAreaChecker.gameObject.SetActive(true);

            //Calculates random spawn position
            Vector3 spawnPosition = new Vector3();

            do
            {
                //Generate random spawn position
                SpawnAreaChecker.GetComponent<SpawnAreaChecker>().IsTouchingAnimal = false;
                spawnPosition = new Vector3(Random.Range(StageBoundsLeftX, StageBoundsRightX), 0, Random.Range(StageBoundsBottomZ, StageBoundsTopZ));
                SpawnAreaChecker.transform.position = spawnPosition;
                yield return new WaitForSeconds(0.1f);
            } 
            while (SpawnAreaChecker.GetComponent<SpawnAreaChecker>().IsTouchingAnimal);

            SpawnAreaChecker.gameObject.SetActive(false);


            //TODO: check if must instantiate or activate

            //Spawning the Object
            Instantiate(Prefabs[randomIndex], spawnPosition, Quaternion.identity);

            if(Prefabs[randomIndex].GetComponent<Animal>() != null)
            {
                if(Prefabs[randomIndex].GetComponent<Animal>().animalType == AnimalType.PREY)
                {
                    GameManager.Instance.spawnedPreys++;
                }
                else
                {
                    GameManager.Instance.spawnedPredators++;
                }
            }

            //Counting the spawned objects
            spawnedObjects++;
        }
    }
}
