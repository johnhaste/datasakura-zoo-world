using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public Canvas Canvas; // Ensure this is set to the canvas in the editor
    public TMP_Text DeadPreysText;
    public TMP_Text DeadPredatorsText;
    public GameObject TastyTextPrefab; // Ensure this prefab is designed to be a UI element

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

    public void ShowTastyText(Vector3 worldPosition)
    {
        // Convert the world position to a screen position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Instantiate the tasty text prefab
        GameObject tastyText = Instantiate(TastyTextPrefab);

        // Set the parent of the tastyText to the canvas, maintaining world position settings
        tastyText.transform.SetParent(Canvas.transform, false);

        // Set the position of the tastyText within the canvas
        tastyText.transform.position = screenPosition;

        // Automatically destroy the tasty text after a delay
        Destroy(tastyText, 1f);
    }
}
