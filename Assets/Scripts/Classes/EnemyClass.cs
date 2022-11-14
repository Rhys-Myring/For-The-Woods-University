/* 
Purpose: Class for enemies and their behaviour
Author:  Rhys Myring
Date:    29/06/2021
Notes:   This script will store all the member variables and functions for any enemy objects. All these functions
         are different actions the enemy can carry out.
Changes: 01/08/2021 Added animation calls to certain functions so animations can be played - Rhys Myring   
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //Member variables
    protected float m_attackDistance;
    private GameObject m_enemyPrefab;
    private AnimationScript m_enemyAnimationScript;

    //Different Enemy states
    protected enum EnemyStates
    {
        Normal,
        Obstructed,
        Attack
    }
    protected EnemyStates m_currentEnemyState;

    //Constructor
    public Enemy(float movementSpeed, float jumpForce, float attackDistance, float health, GameObject enemyPrefab, 
        float attackCoolDown)
        : base(movementSpeed, jumpForce, attackDistance, health, enemyPrefab, attackCoolDown)
    {
        m_movementSpeed = movementSpeed + Random.Range(0.0f, 0.50f);
        m_jumpForce = jumpForce;
        m_health = health;
        m_originalHealth = health;
        m_attackDistance = attackDistance + Random.Range(0.0f, 0.50f);
        m_enemyPrefab = enemyPrefab;
        m_enemyAnimationScript = enemyPrefab.GetComponent<AnimationScript>();
        m_canAttack = true;
        m_attackCoolDown = attackCoolDown;
        m_isAlive = true;
    }

    //Gets distance from enemy to player (although technically it's displacement as it has a direction)
    protected float CheckDistanceToPlayer()
    {
        //Gets both player and enemy current position
        GameObject playerObject = GameObject.Find("Player");
        Vector3 playerPosition = playerObject.transform.position;

        Vector3 distance = m_enemyPrefab.transform.position - playerPosition;

        return distance.x;
    }

    /*Faces enemy towards player
      Seperate function to moving as the enemies can get stuck in a position that they could get out
      of by just turning */
    protected void FacePlayer()
    {
        float distanceToPlayer = CheckDistanceToPlayer();

        //If player is to the left, enemy faces to the left
        if (distanceToPlayer > 0)
        {
            m_enemyPrefab.GetComponent<SpriteRenderer>().flipX = true;
        }
        //If player is to the right, enemy faces to the right
        else
        {
            m_enemyPrefab.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    //Moves towards the player
    protected void MoveTowardsPlayer()
    {
        //Sets current enemy animation state to walking
        m_enemyAnimationScript.SetAnimationState("isWalking");

        //If enemy is facing left, enemy moves to the left
        if (m_enemyPrefab.GetComponent<SpriteRenderer>().flipX)
        {
            this.MoveLeft(m_enemyPrefab, m_movementSpeed);
        }
        //If enemy is facing right, enemy moves to the right
        else
        {
            this.MoveRight(m_enemyPrefab, m_movementSpeed);
        }
    }

    /* When something is blocking the enemy they stop walking so they don't keep walking into something
       When the obstruction is cleared they walk again
       Uses raycasting to check the distance to object in front and stops if it is within range
       I have done it this way as is the most accurate way to judge distances */
    protected void CheckIfBlocked()
    {
        Vector3 rayOrigin = m_enemyPrefab.transform.position;
        Vector3 rayDirection = Vector3.right;

        if (m_enemyPrefab.gameObject.GetComponent<SpriteRenderer>().flipX == true)
        {
            rayDirection = Vector3.left;
        }


        //Casts ray with the length of the attack distance to check what objects are in that range
        RaycastHit[] colliderRay =
            Physics.RaycastAll(rayOrigin, m_enemyPrefab.transform.TransformDirection(rayDirection), m_attackDistance);

        //If there is something within the attack range of the enemy they stop walking
        if (colliderRay.Length > 1)
        {
            if (colliderRay[1].distance <= m_attackDistance)
            {
                //Checks whether the object is the player or not and if it is the player they will attack
                if (colliderRay[1].collider.gameObject == GameObject.Find("Player"))
                {
                    m_currentEnemyState = (EnemyStates)2;
                }
                else
                {
                    m_currentEnemyState = (EnemyStates)1;
                }
            }
        }
        else if (m_currentEnemyState == (EnemyStates)1)
        {
            m_currentEnemyState = (EnemyStates)0;
        }
    }


    // Used for any non-physics related functions that the enemy needs to carry out every frame
    public void EnemyUpdate()
    {
        FacePlayer();

        //When chasing the player if the enemy gets within range they switch to an attack state
        if (Mathf.Abs(CheckDistanceToPlayer()) <= m_attackDistance)
        {
            //Sets current enemy animation state to the default to stop them from walking
            m_enemyAnimationScript.SetAnimationStateToDefault();

            m_currentEnemyState = (EnemyStates)2;
        }

        /* If enemy is in attack state it will attack but if the player gets too far away they will go back
           to chasing the player */
        if (m_currentEnemyState == (EnemyStates)2)
        {
            //Starts Attack coroutine
            StartCoroutineEnemy(this.Attack());

            //Puts enemy back into it's default state if the enemy is out of attack range
            if (Mathf.Abs(CheckDistanceToPlayer()) > m_attackDistance)
            {
                m_currentEnemyState = (EnemyStates)0;
            }
        }
    }

    /* Used for any physics or movement functions as this update is carried out more
       times per second */
    public void EnemyFixedUpdate()
    {
        //In fixed update as it uses raycasting
        CheckIfBlocked();

        //The enemy will walk towards the player as long as nothing is in their path
        //This is done so the enemies don't keep walking into eachother
        if (m_currentEnemyState == (EnemyStates)0)
        {
            MoveTowardsPlayer();
        }
    }

    /* Attack function for the enemies
       It detects whether the enemy is in range of the player and deals damage if they are 
       IEnumerator used so that I can add a wait to deal damage until after the enemy has hit */
    protected IEnumerator Attack()
    {
        if (m_canAttack)
        {
            //Sets can attack to false so enemy can only attack once at a time
            m_canAttack = false;

            //Plays enemy attack animation
            m_enemyAnimationScript.PlayAnimation("punch");

            //Gets length of the current animation
            float animationLength = m_enemyAnimationScript.GetCurrentAnimationLength();

            //Waits for the attack animation to finish before dealing damage to player
            yield return new WaitForSeconds(animationLength);

            //Deals damage to the player
            GameObject.Find("Player").GetComponent<PlayerScript>().GetPlayerObject().TakeDamage();

            //Starts enemy attack cool down
            StartCoroutineEnemy(this.AttackCoolDown());
        }
    }

    //Enemy Override for taking damage
    //Done so all enemy coroutines can be stopped when taking damage
    public new void TakeDamage()
    {
        base.TakeDamage();

        //Stops all coroutines after being hit to stop any current attacks being completed
        if (m_health > 0)
        {
            StopAllCoroutinesEnemy();
        }
    }

    /* This function gets the script component of the enemy as it is monobehaviour and then calls a given coroutine
       this is done as a coroutine can only be started from a monobehaviour script 
       so this is a little work around for that issue */
    protected void StartCoroutineEnemy(IEnumerator function)
    {
        //Detects whether current object is an enemy or boss as they use different scripts
        if (m_enemyPrefab.tag == "Enemy")
        {
            //Gets script component
            EnemyScript monoBehaviour = m_enemyPrefab.GetComponent<EnemyScript>();

            //Starts coroutine
            monoBehaviour.StartCoroutine(function);
        }
        else if (m_enemyPrefab.tag == "Boss")
        {
            //Gets script component
            BossScript monoBehaviour = m_enemyPrefab.GetComponent<BossScript>();

            //Starts coroutine
            monoBehaviour.StartCoroutine(function);
        }
    }

    //The same premise as the above function, but just for stopping any coroutines instead
    protected void StopAllCoroutinesEnemy()
    {
        //Detects whether current object is an enemy or boss as they use different scripts
        if (m_enemyPrefab.tag == "Enemy")
        {
            //Gets script component
            EnemyScript monoBehaviour = m_enemyPrefab.GetComponent<EnemyScript>();

            //Starts coroutine
            monoBehaviour.StopAllCoroutines();
        }
        else if (m_enemyPrefab.tag == "Boss")
        {
            //Gets script component
            BossScript monoBehaviour = m_enemyPrefab.GetComponent<BossScript>();

            //Starts coroutine
            monoBehaviour.StopAllCoroutines();
        }
    }
}