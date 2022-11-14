/*
Purpose: Create and control enemy object for the prefabs
Author:  Rhys Myring
Date:    29/06/2021
Notes:   This script creates an object of the enemy class for a prefab and accesses it's update and fixed update
         functions to control the enemy's behaviour. It is done this way so that each enemy has their own 
         individual instance of an enemy object
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Enemy attributes that are serialized fields so they can be accessed from the inspector
    [SerializeField]
    private float attackDistance = 1.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float jumpSpeed = 5.0f;
    [SerializeField]
    private float attackCoolDown = 1.0f;
    [SerializeField]
    private float health = 3.0f;

    //Variable for the instance of the enemy object
    private Enemy enemyObject;

    // Start is called before the first frame update
    void Start()
    {
        //Creates instance of Enemy Object
        enemyObject = new Enemy(moveSpeed, jumpSpeed, attackDistance, health, this.gameObject, attackCoolDown);
    }

    // Update is called once per frame
    void Update()
    {
        enemyObject.EnemyUpdate();
    }

    /* Fixed Update is used for any functions that contain physics as those calculations need to be 
       carried out much more frequently */
    void FixedUpdate()
    {
        enemyObject.EnemyFixedUpdate();
    }

    //Returns enemy object so that it can be accessed from other scripts
    public Enemy GetEnemyObject()
    {
        return enemyObject;
    }

    /* When enemy game object is enabled, start is called so a new enemy object is created
       I have done this to prevent a bug that occurs where an enemy will stand still and do nothing 
       after the game object is reused (through the object pooling) */
    private void OnEnable()
    {
        Start();
    }
}
