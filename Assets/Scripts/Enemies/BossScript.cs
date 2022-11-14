/*
Purpose: Create and control a boss object for a prefab
Author:  Rhys Myring
Date:    28/07/2021
Notes:   This script creates an object of the boss class for a prefab and accesses it's update and fixed update
         functions to control the boss' behaviour.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    //Boss attributes that are serialized fields so they can be accessed from the inspector
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
    [SerializeField]
    GameObject shockWave;

    //Variable for the instance of the boss object
    private Boss bossObject;

    // Start is called before the first frame update
    void Start()
    {
        //Creates instance of Boss Object
        bossObject = new Boss(moveSpeed, jumpSpeed, attackDistance, health, this.gameObject, attackCoolDown, shockWave);
    }

    // Update is called once per frame
    void Update()
    {
        bossObject.BossUpdate();
    }

    /* Fixed Update is used for any functions that contain physics as those calculations need to be 
       carried out much more frequently */
    void FixedUpdate()
    {
        bossObject.BossFixedUpdate();
    }

    //Returns boss object so that it can be accessed from other scripts
    public Boss GetBossObject()
    {
        return bossObject;
    }
}
