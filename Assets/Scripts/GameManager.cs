using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Counters")]
    public int deadPreys;
    public int deadPredators;

    [Header("Groups")]
    public GameObject PreysGroup;
    public GameObject PredatorsGroup;

    [Header("Counters")]
    public int spawnedPreys;
    public int activePreys;
    public int spawnedPredators;
    public int activePredators;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDeadAnimalCounter(Animal animal)
    {
        switch(animal.animalType)
        {
            case AnimalType.PREY:
                deadPreys++;
                UIManager.Instance.UpdateDeadPreysText(deadPreys);
                break;
            case AnimalType.PREDATOR:
                deadPredators++;
                UIManager.Instance.UpdateDeadPredatorsText(deadPredators);
                break;
        }
    }

    public void UpdateActiveObjectsCounter()
    {
        spawnedPreys = PreysGroup.transform.childCount;
        spawnedPredators = PredatorsGroup.transform.childCount;

        activePredators = 0;
        foreach(Transform predator in PredatorsGroup.transform)
        {
            if(predator.gameObject.activeSelf)
            {
                activePredators++;
            }
        }

        activePreys = 0;
        foreach(Transform prey in PreysGroup.transform)
        {
            if(prey.gameObject.activeSelf)
            {
                activePreys++;
            }
        }
    }

}
