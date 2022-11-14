/* 
Purpose: Script to control the item slot
Author:  Rhys Myring
Date:    18/08/2021
Notes:   This script will check which item the player currently has every frame and displays it on screen.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    //Variable to store the current item that the player has
    private int currentItem;
    
    //All the different Item images, serialised so they can be edited from the inspector
    [SerializeField]
    private GameObject healthSlot;
    [SerializeField]
    private GameObject doubleDamageSlot;
    [SerializeField]
    private GameObject invincibleSlot;

    void Update()
    {
        //Checks the player's item every frame and then sets the correct item active,
        //If there is not current item they are all set to false

        currentItem = GameObject.Find("Player").GetComponent<PlayerScript>().GetPlayerObject().GetCurrentItem();

        switch (currentItem)
        {
            case 0:
                healthSlot.SetActive(false);
                doubleDamageSlot.SetActive(false);
                invincibleSlot.SetActive(false);
                break;

            case 1:
                healthSlot.SetActive(true);
                doubleDamageSlot.SetActive(false);
                invincibleSlot.SetActive(false);
                break;

            case 2:
                healthSlot.SetActive(false);
                doubleDamageSlot.SetActive(true);
                invincibleSlot.SetActive(false);
                break;

            case 3:
                healthSlot.SetActive(false);
                doubleDamageSlot.SetActive(false);
                invincibleSlot.SetActive(true);
                break;
        }
    }
}
