/* 
Purpose: Class for the Items
Author:  Rhys Myring
Date:    15/08/2021
Notes:   This script will store all the member variables and functions for the Items.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //Member Variable
    private string m_type;

    //Variable to store player object so the player's attributes can be modified by the power ups
    private Player playerObject;

    //Constructor
    public Item(string itemType)
    {
        m_type = itemType;

        //Gets player object
        playerObject = GameObject.Find("Player").GetComponent<PlayerScript>().GetPlayerObject();
    }

    //When Item is collected the correct effect is applied to the player
    public void Collected()
    {
        //Removes any item effects
        RemoveStatusEffects();

        switch(m_type)
        {
            case "Health":
                //Variables to avoid magic numbers
                float healthIncrease = 5;
                float maxHealth = 10;

                //Gets current player health
                float playerHealth = playerObject.GetHealth();

                //Adds health increase to current player health
                playerHealth += healthIncrease;

                //If new health is larger than the max, the health is set to the max
                if (playerHealth > maxHealth)
                {
                    playerHealth = maxHealth;
                }

                //Sets player's health to the new health
                playerObject.SetHealth(playerHealth);

                //Sets current power up to health
                playerObject.SetCurrentItem(1);
                break;

            case "Double Damage":
                //Doubles attack multiplier
                playerObject.SetPlayerAttackMultiplier(2);

                //Sets current power up to Double Damage
                playerObject.SetCurrentItem(2);
                break;

            case "Invincibility":
                //Sets player to invincible
                playerObject.Invincible();

                //Sets current power up to invincible
                playerObject.SetCurrentItem(3);
                break;
        }
    }


    //Removes any status effects from player
    private void RemoveStatusEffects()
    {
        playerObject.SetPlayerAttackMultiplier(1);
        playerObject.SetInvincibility(false);
    }
}
