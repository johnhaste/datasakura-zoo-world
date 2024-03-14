using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> Prefabs = new List<GameObject>();
    public int minWait = 1;
    public int maxWait = 3;

    [Header("Boundaries")]
    public GameObject StageBoundsLeft;
    public GameObject StageBoundsRight;
    public GameObject StageBoundsTop;
    public GameObject StageBoundsBottom;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPrefab());
    }

    IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            int randomIndex = Random.Range(0, Prefabs.Count);

            //Boundaries
            int StageBoundsLeftX = (int)StageBoundsLeft.transform.position.x + 1;
            int StageBoundsRightX = (int)StageBoundsRight.transform.position.x - 1;
            int StageBoundsTopZ = (int)StageBoundsTop.transform.position.z - 1;
            int StageBoundsBottomZ = (int)StageBoundsBottom.transform.position.z + 1;

            //Generate random spawn position
            Vector3 spawnPosition = new Vector3(Random.Range(StageBoundsLeftX, StageBoundsRightX), 0, Random.Range(StageBoundsBottomZ, StageBoundsTopZ));

            Instantiate(Prefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
}
