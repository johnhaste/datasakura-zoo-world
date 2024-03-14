using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalType animalType;
}

public enum AnimalType
{
    PREY,
    PREDATOR
}

