/*
Purpose: Create and control player object
Author:  Rhys Myring
Date:    07/07/2021
Notes:   This script creates an object of the player class and accesses it's functions.
Changes: 09/07/2021 Removed movement functions as Ilyes has added his now.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Player attributes
    [SerializeField]
    private float jumpForce = 60f;
    [SerializeField]
    private float attackRange = 1.0f;
    [SerializeField]
    private float attackCoolDown = 0.25f;
    [SerializeField]
    private float health = 100.0f;
    [SerializeField]
    private float invincibleTime = 2.0f;

    //Variable for the instance of the player object
    private Player playerObject;

    // Start is called before the first frame update
    void Start()
    {
        //Creates instance of player Object
        playerObject = new Player(jumpForce, attackRange, health, this.gameObject, attackCoolDown, invincibleTime);
    }

    // Update is called once per frame
    void Update()
    {
        playerObject.PlayerUpdate();
    }

    /* Fixed Update is used for any functions that contain physics as those calculations need to be 
       carried out much more frequently */
    void FixedUpdate()
    {
        playerObject.PlayerFixedUpdate();
    }

    //Checks if the object the player has collided with is the ground or not
    private void OnCollisionEnter(Collision collision)
    {
        playerObject.CheckIfGround(collision.collider);
    }

    /* If the player enters a trigger the correct behaviour fot that trigger is carried out
       if the trigger is a position lock trigger the player is locked onto a platform
       but if it is a level end trigger the current level is completed. */
    private void OnTriggerEnter(Collider trigger)
    {
        //Locks player onto platform if it is a position lock trigger
        if (trigger.tag == "PostitionLockTrigger")
        {
            trigger.GetComponent<PositionLock>().PlayerHasEnteredPlatform();
        }

        //Exits level if it is a level end trigger
        else if (trigger.tag == "LevelEnd")
        {
            trigger.GetComponent<LoadNextLevel>().ExitLevel();
        }
    }

    //Returns player object so that it can be accessed from other scripts
    public Player GetPlayerObject()
    {
        return playerObject;
    }
}
