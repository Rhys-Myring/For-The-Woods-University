/*
Purpose: Controls Shockwave
Author:  Rhys Myring
Date:    12/08/2021
Notes:   This script controls the shock wave that is given off when a Boss ground pounds. It will move to the left until
         it goes off screen or collides with the player
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{
    //Variable to store the movement speed of the shock wave
    private float moveSpeed = -10;

    //Fixed update used to move the shockwave
    void FixedUpdate()
    {
        //Increases the x coordinate by speed times delta time so it is independent from frame rate
        float newXPos = this.transform.position.x + moveSpeed * Time.fixedDeltaTime;
        this.transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
    }

    //Destroys shockwave when it goes off screen
    void OnBecameInVisible()
    {
        Destroy(gameObject);
    }

    //Damages player when hitting them and destroys the shockwave
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().GetPlayerObject().TakeDamage();
            Destroy(gameObject);
        }
    }
}
