/*
Purpose: Create and control item object
Author:  Rhys Myring
Date:    15/08/2021
Notes:   This script creates an object of the item class and accesses it's functions.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    //Variable to store Item Object
    private Item itemObject;

    //Variable to control the type of Item
    [SerializeField]
    private string itemType;

    //Controls wait time before the object is collected
    private float collectionTime = 0.5f;

    //Controls whether the item is collected or not
    private bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        //Creates new instance of Item Class
        itemObject = new Item(itemType);
    }

    /* When the trigger is entered, the collider is checked to see if it's a player
       if the collider is a player, the collect item function is executed */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isCollected == false)
        {
            StartCoroutine(CollectItem());
        }
    }

    //The item is collected; the game object is destroyed and the item's action is carried out
    private IEnumerator CollectItem()
    {
        //Sets item to collected
        isCollected = true;

        //Waits half a second before collecting item so it can be seen by the player
        yield return new WaitForSeconds(collectionTime);

        //Sets item to collected
        itemObject.Collected();

        //Destroys Item so it cannot be collected again
        Destroy(gameObject);
    }
}
