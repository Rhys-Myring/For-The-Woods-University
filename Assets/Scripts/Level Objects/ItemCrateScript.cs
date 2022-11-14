/* 
Purpose: Control what item is held by a specific item crate.
Author:  Rhys Myring
Date:    21/07/2021
Notes:   This script contains a variable to store which item is contained inside of an item crate. There is also
         a function to return this value so that it can be accessed by the crate class.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrateScript : MonoBehaviour
{
    //Item inside of the crate
    //Serialised so it can be changed via the inspector
    [SerializeField]
    private GameObject item;

    //Returns item held by this crate so it can be accesses form the crate class
    public GameObject GetItem()
    {
        return item;
    }
}
