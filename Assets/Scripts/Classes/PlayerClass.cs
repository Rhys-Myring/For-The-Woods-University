/* 
Purpose: Class for the player
Author:  Rhys Myring
Date:    07/07/2021
Notes:   This script will store all the member variables and functions for the player. All these functions
         are different actions the player can carry out.
Changes: 21/07/2021 Made attack distance based instead of collision based. - Rhys Myring
         04/08/2021 Added animation calls to certain functions so animations can be played - Rhys Myring   
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //Member variables
    private GameObject m_playerGameObject;
    private AnimationScript m_playerAnimationScript;
    private float m_attackMultiplier;
    private int m_currentItem;

    //Used to store what collider is currently below the player to check whether they are on the ground or not
    private Collider m_colliderBelowPlayer;

    //Used to call coroutines as they must be called from a monobehaviour script
    private PlayerScript m_monoBehaviour;

    //Variables to control the player's invinciblity
    private bool m_isInvincible;
    private float m_invincibleTime;

    //Constructor
    public Player(float jumpForce, float attackRange, float health, GameObject playerGameObject, float attackCoolDown,
        float invincibleTime)
        : base(0f, jumpForce, attackRange, health, playerGameObject, attackCoolDown)
    {
        m_attackRange = attackRange;
        m_health = health;
        m_playerGameObject = playerGameObject;
        m_playerAnimationScript = playerGameObject.GetComponent<AnimationScript>();
        m_monoBehaviour = m_playerGameObject.GetComponent<PlayerScript>();
        m_canAttack = true;
        m_canJump = true;
        m_attackCoolDown = attackCoolDown;
        m_isAlive = true;
        m_isInvincible = false;
        m_invincibleTime = invincibleTime;
        m_attackMultiplier = 1;
        m_currentItem = 0;
    }

    //Player update function, called once every frame
    public void PlayerUpdate()
    {
        //As long as player is alive
        if (m_isAlive == true)
        {
            //Checks what keys the user is pressing and responds accordingly
            GetInputs();
        }
        //When player is dead stops them from moving
        else
        {
            //Removes the movement component so the player can't move after dying
            m_playerGameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    /* Seperate as it contains raycasting and this update is carried out more times per second */
    public void PlayerFixedUpdate()
    {
        //Gets player's current position
        Vector3 rayOrigin = m_playerGameObject.transform.position;

        //Casts ray to check whether the player is above solid ground
        RaycastHit[] jumpRay =
            Physics.RaycastAll(rayOrigin, m_playerGameObject.transform.TransformDirection(Vector3.down),
            Mathf.Infinity);

        //Checks if there is anything solid below player
        if (jumpRay.Length > 0)
        {
            //Updates the collider below the player
            m_colliderBelowPlayer = jumpRay[0].collider;
        }
        else
        {
            //Sets collider below player to null if there isn't a collider below them
            m_colliderBelowPlayer = null;
        }
    }

    //Checks what keys have been pressed and carries out a function depending on which is pressed
    private void GetInputs()
    {
        //If jump is pressed, jumps and then plays the jump animation 
        if (Input.GetKeyDown("space"))
        {
            if (m_canJump)
            {
                //Makes player jump
                Jump(m_playerGameObject, m_jumpForce);

                m_playerAnimationScript.SetAnimationStateToDefault();
                m_playerAnimationScript.PlayAnimation("jump");

                m_canJump = false;
            }
        }

        //Detects whether an attack key has been pressed and attacks if it has
        else if (Input.GetKeyUp("k") && m_canAttack)
        {
            Attack(1);
        }
        else if (Input.GetKeyUp("j") && m_canAttack)
        {
            Attack(2);
        }

        /* Flips the sprite depending on what input is pressed to represent which direction the player is facing
           Plays walking animation if the player is on the ground */
        else if (Input.GetKey("a"))
        {
            GameObject.Find("PlayerVisual").GetComponent<SpriteRenderer>().flipX = true;

            if (m_canJump != false)
            {
                m_playerAnimationScript.SetAnimationState("isWalking");
            }
        }
        else if (Input.GetKey("d"))
        {
            GameObject.Find("PlayerVisual").GetComponent<SpriteRenderer>().flipX = false;

            if (m_canJump != false)
            {
                m_playerAnimationScript.SetAnimationState("isWalking");
            }
        }

        //If nothing is pressed the player returns to the idle animation
        else
        {
            m_playerAnimationScript.SetAnimationStateToDefault();
        }
    }

    /* Player attack function
       It detects whether the player is within range an enemy or crate and if they are it damages that object. */
    private void Attack(int attackType)
    {
        float animationLength = 0f;

        //Plays the correct animation depending on which attack was initiated
        switch (attackType)
        {
            case 1:
                //Plays punch animation
                m_playerAnimationScript.PlayAnimation("punch");

                //Gets length of the current animation
                animationLength = m_playerAnimationScript.GetCurrentAnimationLength();
                break;
            case 2:
                //Plays sword attack animation
                m_playerAnimationScript.PlayAnimation("sword");

                //Gets length of the current animation
                animationLength = m_playerAnimationScript.GetCurrentAnimationLength();
                break;
        }

        //Gets player's x coordinate to find distance
        float playerX = m_playerGameObject.transform.position.x;

        //Gets the direction the player is facing
        bool isPlayerFacingLeft = GameObject.Find("PlayerVisual").GetComponent<SpriteRenderer>().flipX;

        //Finds all enemies
        GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        //Finds all crates
        GameObject[] cratesArray = GameObject.FindGameObjectsWithTag("Crate");

        //Finds all bosses
        GameObject[] bossesArray = GameObject.FindGameObjectsWithTag("Boss");

        //Checks whether any enemies, bosses or crates are within the players attack range and attacks them if they are
        //m_monoBehaviour used to start coroutine
        m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetPlayerObject().
            AttackObjects(enemiesArray, playerX, isPlayerFacingLeft, animationLength));
        m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetPlayerObject().
            AttackObjects(cratesArray, playerX, isPlayerFacingLeft, animationLength));
        m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetPlayerObject().
            AttackObjects(bossesArray, playerX, isPlayerFacingLeft, animationLength));

        //Sets can attack to false
        m_canAttack = false;

        //Sets attack cool down to length of attack animation
        m_attackCoolDown = animationLength;

        //Starts attack cool down to prevent over spamming attacks
        m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetPlayerObject().AttackCoolDown());
    }

    //Checks whether the player is facing and within attack range of any objects in a given array
    //If they are within attack range and are facing them, the object is hit
    private IEnumerator AttackObjects(GameObject[] objectsArray, float playerX, bool isPlayerFacingLeft,
        float animLength)
    {
        //Waits for the attack animation to finish before dealing damage to an object
        yield return new WaitForSeconds(animLength);

        //Float to store object's x coordinates
        float objectX;

        if (objectsArray.Length != 0)
        {
            //Checks to see if the player is within attack range of any of the crates and destroys them if they are
            for (int index = 0; index < objectsArray.Length; index++)
            {
                objectX = objectsArray[index].transform.position.x;

                /* Checks whether the player is facing the object or not, if they are the object is attacked, if not
                   the object is not attacked */
                switch (isPlayerFacingLeft)
                {
                    case true:
                        //Checks whether the player is to the right of an object
                        if (playerX - objectX > 0 && Mathf.Abs(playerX - objectX) < m_attackRange)
                        {
                            //Hits object
                            HitObject(objectsArray, index);
                        }
                        break;
                    case false:
                        //Checks whether the player is to the left of an object
                        if (playerX - objectX < 0 && Mathf.Abs(playerX - objectX) < m_attackRange)
                        {
                            //Hits object
                            HitObject(objectsArray, index);
                        }
                        break;
                }
            }
        }
    }

    /* Checks whether the object is a crate or an enemy or a boss and applies the appropriate damage function.
       It is it's own function to reduce duplicate code */
    private void HitObject(GameObject[] objectsArray, int index)
    {
        //Checks whether the object is a crate or an enemy or a boss
        if (objectsArray[index].tag == "Crate")
        {
            objectsArray[index].GetComponent<CrateScript>().GetCrateObject().DestroyCrate();
        }

        else if (objectsArray[index].tag == "Enemy")
        {
            objectsArray[index].GetComponent<EnemyScript>().GetEnemyObject().TakeDamage();
        }

        else if (objectsArray[index].tag == "Boss")
        {
            objectsArray[index].GetComponent<BossScript>().GetBossObject().TakeDamage();
        }
    }

    /* Checks if the object that the player has collided with is the ground below them or not
       If it is then the player can jump again */
    public void CheckIfGround(Collider objectHit)
    {
        if (objectHit == m_colliderBelowPlayer)
        {
            m_canJump = true;
        }
    }

    /* Player override for taking damage, it stops them from attacking until the hit animation is done */
    public new void TakeDamage()
    {
        //If player is invincible they don't take damage
        if (m_isInvincible == false)
        {
            //Does the base Take Damage function
            base.TakeDamage();

            //Gets current animation length
            float animationLength = m_playerAnimationScript.GetCurrentAnimationLength();

            m_canAttack = false;

            //Sets attack cool down to animation length
            m_attackCoolDown = animationLength;

            //Makes player wait for animation to finish before being able to attack again
            m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetPlayerObject().AttackCoolDown());
        }
    }

    //Sets player to invincible
    public void Invincible()
    {
        SetInvincibility(true);

        //Starts timer for invincibilty
        m_monoBehaviour.StartCoroutine(this.StopBeingInvincible());

    }

    //Makes player vincible after a set amount of time
    private IEnumerator StopBeingInvincible()
    {
        yield return new WaitForSeconds(m_invincibleTime);

        SetInvincibility(false);
        m_currentItem = 0;
    }

    //Getters
    public float GetPlayerAttackMultiplier()
    {
        return m_attackMultiplier;
    }

    public int GetCurrentItem()
    {
        return m_currentItem;
    }

    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    //Setters
    public void SetPlayerAttackMultiplier(float multiplier)
    {
        m_attackMultiplier = multiplier;
    }

    public void SetInvincibility(bool invincible)
    {
        m_isInvincible = invincible;
    }

    public void SetCurrentItem(int item)
    {
        m_currentItem = item;
    }
}