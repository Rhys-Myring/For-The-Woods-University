/* 
Purpose: Object Pooling
Author:  Rhys Myring
Date:    26/07/2021
Notes:   This script will store a list of gameObjects that are instanciated at the start of the program but set as 
         inactive until they are needed.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //Variables to store which object will be in the pool and how many of that object there will be
    [SerializeField]
    private GameObject objectToPool;
    [SerializeField]
    private int numberOfObjectsToPool = 0;

    private List<GameObject> pooledObjects;


    //Object pooling research is from here: https://learn.unity.com/tutorial/introduction-to-object-pooling#5ff8d015edbc2a002063971d

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        
        for (int i = 0; i < numberOfObjectsToPool; i++) 
        {
            pooledObjects.Add(Instantiate(objectToPool));
            pooledObjects[i].SetActive(false); 
        }
    }

    /*Returns an inactive object from the pool so it can be used
      public so that it can be accessed by other scripts */
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < numberOfObjectsToPool; i++) 
        { 
            if (!pooledObjects[i].activeInHierarchy) 
            { 
                return pooledObjects[i]; 
            } 
        }

        return null;
    }
}
