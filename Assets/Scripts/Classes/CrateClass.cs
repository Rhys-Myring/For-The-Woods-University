/* 
Purpose: Main class for crates
Author:  Rhys Myring
Date:    20/07/2021
Notes:   This script contains all the member variables and functions for a crate.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    //Member variables
    private string m_crateType;
    private GameObject m_crateGameObject;

    //Constructor
    public Crate(string type, GameObject crateGameObject)
    {
        m_crateType = type;
        m_crateGameObject = crateGameObject;
    }

    //Destroys crate and carries out specific action based on the crate's type
    public void DestroyCrate()
    {
        if (m_crateType == "explosive")
        {
            ExplodeCrate();
        }

        if (m_crateType == "item")
        {
            DropItem();
        }

        Destroy(m_crateGameObject);
    }

    //Causes an explosion that deals damage to enemies
    private void ExplodeCrate()
    {
        //Finds all objects within the explosion range 
        Collider[] objectsInRange = Physics.OverlapSphere(m_crateGameObject.transform.position, 1.125f);

        //Checks each object in the range to check if there are any enemies
        if (objectsInRange.Length > 0)
        {
            for (int index = 0; index < objectsInRange.Length; index++)
            {
                if (objectsInRange[index].gameObject.tag == "Enemy")
                {
                    //Damages enemy
                    objectsInRange[index].gameObject.GetComponent<EnemyScript>().GetEnemyObject().TakeDamage();
                }
            }
        }
    }

    //Drops an item for the player to pick up
    private void DropItem()
    {
        GameObject itemToInstanciate;

        //Gets item stored in the item crate
        itemToInstanciate = m_crateGameObject.GetComponent<ItemCrateScript>().GetItem();

        //Spawns item at the location of the crate
        Instantiate(itemToInstanciate, m_crateGameObject.transform.position, Quaternion.identity);
    }
}
