using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaChecker : MonoBehaviour
{
    public bool IsTouchingAnimal = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Animal>() != null)
        {
            IsTouchingAnimal = true;
            Debug.Log("OnTriggerEnter:SpawnAreaChecker is touching animal");
        }
    }
}
