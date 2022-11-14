/*
Purpose: Create and control crate objects
Author:  Rhys Myring
Date:    20/07/2021
Notes:   This script creates an object of the crate class and accesses it's functions.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    //Crate type
    [SerializeField]
    private string crateType = "normal";

    //Variable to store the crate object
    private Crate crateObject;

    // Start is called before the first frame update
    void Start()
    {
        crateObject = new Crate(crateType, this.gameObject);
    }


    //Returns crate object so that it can be accessed from other scripts
    public Crate GetCrateObject()
    {
        return crateObject;
    }
}
