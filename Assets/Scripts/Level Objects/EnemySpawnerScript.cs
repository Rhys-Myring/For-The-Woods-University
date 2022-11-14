/*
Purpose: Spawn enemy prefabs
Author:  Rhys Myring
Date:    12/07/2021
Notes:   This script enables a specific enemy prefab at a set interval
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    //Spawner attributes that are serialized fields so they can be accessed from the inspector
    [SerializeField]
    private float minSpawningInterval = 3.0f;
    [SerializeField]
    private float maxSpawningInterval = 3.0f;
    [SerializeField]
    private int numberOfType1Enemies = 5;
    [SerializeField]
    private int numberOfType2Enemies = 5;
    [SerializeField]
    private int numberOfType3Enemies = 5;
    [SerializeField]
    private int numberOfType4Enemies = 5;
    [SerializeField]
    private int numberOfType5Enemies = 5;
    [SerializeField]
    private int maxEnemiesOnScreen = 3;

    //List to store all of the available enemy types
    private List<string> enemyTypes = new List<string>();

    //Keeps track of how many enemies are on screen
    private int numberOfEnemiesOnScreen;

    //Keeps track of how many enemies have been spawned by this spawner
    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemyType1Spawned = 0;
    private int numberOfEnemyType2Spawned = 0;
    private int numberOfEnemyType3Spawned = 0;
    private int numberOfEnemyType4Spawned = 0;
    private int numberOfEnemyType5Spawned = 0;

    //Stores the total number of enemies in this spawner
    private int numberOfEnemiesInSpawner;

    // Start is called before the first frame update
    void Start()
    {
        /* Runs the SpawnEnemy function repeatedly at an interval set in the inspector
           Repeatedly spawns an enemy at a set interval */
        InvokeRepeating("SpawnEnemy", 0.0f, Random.Range(minSpawningInterval, maxSpawningInterval));

        //Adds up the total number of enemies in the spawner
        numberOfEnemiesInSpawner = numberOfType1Enemies + numberOfType2Enemies + numberOfType3Enemies
            + numberOfType4Enemies + numberOfType5Enemies;

        //Adds all of the different types used in the spawner to the list
        if (numberOfType1Enemies > 0)
        {
            enemyTypes.Add("EnemyType1");
        }
        if (numberOfType2Enemies > 0)
        {
            enemyTypes.Add("EnemyType2");
        }
        if (numberOfType3Enemies > 0)
        {
            enemyTypes.Add("EnemyType3");
        }
        if (numberOfType4Enemies > 0)
        {
            enemyTypes.Add("EnemyType4");
        }
        if (numberOfType5Enemies > 0)
        {
            enemyTypes.Add("EnemyType5");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Checks the current number of enemies on the screen
        numberOfEnemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy").Length;

        //When all the enemies from this spawner have been defeated the spawner sets itself to inactive
        if (numberOfEnemiesSpawned == numberOfEnemiesInSpawner && numberOfEnemiesOnScreen == 0)
        {
            this.gameObject.SetActive(false);
        }
    }


    /* Checks whether the maximum number of enemies is on screen or if the all the enemies from this spawner
       have been spawned, if both are false then an enemy is spawned.
       This is done so there aren't too many enemies on screen at once and so that the spawner 
       doesn't create infinite enemies */
    private void SpawnEnemy()
    {
        if (numberOfEnemiesOnScreen < maxEnemiesOnScreen && numberOfEnemiesSpawned < numberOfEnemiesInSpawner)
        {
            //Spawns enemy at the location of the spawner
            GameObject enemy = RandomiseEnemy(); 
            if (enemy != null) 
            {
                /* Sets enemy game object to the position and rotation of the spawner and sets them to active
                   this is done to be more efficient than instanciating and destroying enemies */
                enemy.transform.position = this.gameObject.transform.position;
                enemy.transform.rotation = this.gameObject.transform.rotation;
                enemy.SetActive(true); 
            }

            //Increases the number of enemies that have been spawned
            numberOfEnemiesSpawned++;
        }
    }


    //Randomly generates an enemy type to spawn from the spawner
    private GameObject RandomiseEnemy()
    {
        //Randomised integer to decide which enemy type is spawned
        int randomType = Mathf.FloorToInt(Random.Range(0, enemyTypes.Count));

        string typeToSpawn = enemyTypes[randomType];

        //Game object to return so it can be activated
        GameObject enemyToActivate = null;

        //Checks each enemy type to see if they are the type that was randomly selected to be spawned      
        CheckEnemyType(1, typeToSpawn, randomType, ref numberOfEnemyType1Spawned, numberOfType1Enemies, 
            ref enemyToActivate);
        CheckEnemyType(2, typeToSpawn, randomType, ref numberOfEnemyType2Spawned, numberOfType2Enemies,
            ref enemyToActivate); 
        CheckEnemyType(3, typeToSpawn, randomType, ref numberOfEnemyType3Spawned, numberOfType3Enemies,
             ref enemyToActivate);
        CheckEnemyType(4, typeToSpawn, randomType, ref numberOfEnemyType4Spawned, numberOfType4Enemies,
            ref enemyToActivate);
        CheckEnemyType(5, typeToSpawn, randomType, ref numberOfEnemyType5Spawned, numberOfType5Enemies,
            ref enemyToActivate);
        

        //Returns enemy prefab to be spawned
        return enemyToActivate;
    }


    /* Checks a given enemy type to see if they are the type that was randomly selected to be spawned
       if they are, the enemy to activate is set to an instance of that type.
       Then if all enemies of that chosen type have been spawned, the type is removed from the list so 
       that it can't be picked again */
    private void CheckEnemyType(int typeChecked, string typespawned, int index, ref int numOfTypeSpawned, 
        int numInSpawner, ref GameObject enemyToActivate)
    {
        if (typespawned == "EnemyType" + typeChecked)
        {
            numOfTypeSpawned++;

            enemyToActivate = GameObject.Find("Enemy" + typeChecked + "ObjectPool")
                .GetComponent<ObjectPool>().GetPooledObject();

            if (numOfTypeSpawned >= numInSpawner)
            {
                enemyTypes.RemoveAt(index);
            }
        }
    }
}
