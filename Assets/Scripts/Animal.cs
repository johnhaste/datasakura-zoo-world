using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalType animalType;
    public string animalName;
    public int spawnIndex;

    public void Start()
    {
        //Index for future comparisons
        spawnIndex = ObjectSpawner.Instance.spawnedObjects;
        
        //Grouping by animal type
        if(animalType == AnimalType.PREY)
        {
            transform.SetParent(GameManager.Instance.PreysGroup.transform);
        }
        else
        {
            transform.SetParent(GameManager.Instance.PredatorsGroup.transform);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter: " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<Animal>() != null)
        {
            Animal collidedAnimal = collision.gameObject.GetComponent<Animal>();
            //Debug.Log("OnTriggerEnter: Animal:" + collidedAnimal.animalName + "->" + collidedAnimal.animalType);

            if(animalType == AnimalType.PREDATOR && collidedAnimal.animalType == AnimalType.PREY)
            {
                //Debug.Log("Predator " + spawnIndex +" caught prey " + collidedAnimal.spawnIndex);
                GameManager.Instance.AddDeadAnimalCounter(collidedAnimal);
                UIManager.Instance.ShowTastyText(collidedAnimal.transform.position);

                collision.gameObject.SetActive(false);
                //Destroy(collision.gameObject);
            }
            else if(animalType == AnimalType.PREDATOR && collidedAnimal.animalType == AnimalType.PREDATOR)
            {
                if(spawnIndex > collidedAnimal.spawnIndex)
                {
                    //Debug.Log("Predator " + spawnIndex + " caught by predator " + collidedAnimal.spawnIndex);
                    GameManager.Instance.AddDeadAnimalCounter(collidedAnimal);
                    UIManager.Instance.ShowTastyText(collidedAnimal.transform.position);

                    collision.gameObject.SetActive(false);
                    //Destroy(collision.gameObject);                    
                }
                else
                {
                    //Debug.Log("Predator " + collidedAnimal.spawnIndex + " caught by predator " + spawnIndex);
                    GameManager.Instance.AddDeadAnimalCounter(this);
                    UIManager.Instance.ShowTastyText(transform.position);

                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                }
            }

            GameManager.Instance.UpdateActiveObjectsCounter();
        }
    }
}

public enum AnimalType
{
    PREY,
    PREDATOR
}

