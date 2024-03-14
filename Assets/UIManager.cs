using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public TMP_Text DeadPreysText;
    public TMP_Text DeadPredatorsText;

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
    
    public void UpdateDeadPreysText(int deadPreys)
    {
        DeadPreysText.text = "Dead Preys: " + deadPreys;
    }

    public void UpdateDeadPredatorsText(int deadPredators)
    {
        DeadPredatorsText.text = "Dead Predators: " + deadPredators;
    }
}

