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

}
