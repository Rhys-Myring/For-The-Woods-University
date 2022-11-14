/*
Purpose: Lock player onto platform whilst fighting enemies
Author:  Rhys Myring
Date:    13/07/2021
Notes:   This script locks the player onto a platform until they have defeated all of the enemies on the platform
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLock : MonoBehaviour
{
    //X Values that the player is clamped between
    [SerializeField]
    private float platformXMin = 0.0f;
    [SerializeField]
    private float platformXMax = 0.0f;

    //Enemy spawners for the area the player is locked into
    [SerializeField]
    private GameObject spawnerLeft;
    [SerializeField]
    private GameObject spawnerRight;


    //Keeps track of whether the player has stepped onto the platform or not
    private bool playerHasEnteredArea = false;


    // Update is called once per frame
    void Update()
    {
        //When the player has entered the trigger they are locked between the 2 x values
        if (playerHasEnteredArea)
        {
            LockPlayerOnPlatform();

            //When both of the spawners are defeated the gameobject destroys itself so the player is freed
            if (spawnerLeft.activeInHierarchy == false && spawnerRight.activeInHierarchy == false)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //Sets player has entered platform to true
    //Sets both spawners to active so they spawn enemies
    public void PlayerHasEnteredPlatform()
    {
        if (playerHasEnteredArea == false)
        {
            playerHasEnteredArea = true;

            spawnerLeft.SetActive(true);
            spawnerRight.SetActive(true);
        }              
    }

    //Restricts player's x position between 2 points to lock them onto a platform during a fight sequence
    private void LockPlayerOnPlatform()
    {
        GameObject playerGameObject = GameObject.Find("Player");
        
        /* Gets player's x position, if it is smaller than the minimum then it is set to the minimum
           and if it is larger than the maximum it is set to the maximum */
        if (playerGameObject.transform.position.x > platformXMax)
        {
            playerGameObject.transform.position = new Vector2(platformXMax, playerGameObject.transform.position.y);
        }
        else if (playerGameObject.transform.position.x < platformXMin)
        {
            playerGameObject.transform.position = new Vector2(platformXMin, playerGameObject.transform.position.y);
        }
    }
}
