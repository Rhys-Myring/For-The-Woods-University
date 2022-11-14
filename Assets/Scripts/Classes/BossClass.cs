/* 
Purpose: Class for bosses and their behaviour
Author:  Rhys Myring
Date:    28/07/2021
Notes:   This script will store all the member variables and functions for any boss objects. All these functions
         are different actions the boss can carry out.
Changes: 11/08/2021 Added multiple attacks that can be randomly swapped between - Rhys Myring  
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //Member variables
    private GameObject m_bossPrefab;
    private AnimationScript m_bossAnimationScript;

    //Variables to control which attack the boss is currently doing
    private int m_currentAttack;
    private bool m_isAttackComplete;

    //Used to call coroutines as they must be called from a monobehaviour script
    private BossScript m_monoBehaviour;

    //Variables to store player's health to decide when to change the current attack
    private float playerHealth = 0;
    private float playerHealthPrevious = 0;

    //Variable to control whether the boss can ground pound or not
    private bool m_canGroundPound;

    //Variable to store the shockwave object that will be released when the boss has ground pounded
    private GameObject m_shockwaveObject;

    //Constructor
    public Boss(float movementSpeed, float jumpForce, float attackDistance, float health, GameObject bossPrefab,
        float attackCoolDown, GameObject shockWavePrefab)
        : base(movementSpeed, jumpForce, attackDistance, health, bossPrefab, attackCoolDown)
    {
        m_movementSpeed = movementSpeed;
        m_jumpForce = jumpForce;
        m_health = health;
        m_originalHealth = health;
        m_attackDistance = attackDistance;
        m_bossPrefab = bossPrefab;
        m_bossAnimationScript = m_bossPrefab.GetComponent<AnimationScript>();
        m_monoBehaviour = m_bossPrefab.GetComponent<BossScript>();
        m_canAttack = true;
        m_canGroundPound = true;
        m_currentAttack = 0;
        m_shockwaveObject = shockWavePrefab;
        m_attackCoolDown = attackCoolDown;
        m_isAlive = true;
        m_isAttackComplete = false;
    }

    // Used for any non-physics related functions that the boss needs to carry out every frame
    public void BossUpdate()
    {
        //If the current attack state is the default attack the boss approaches the player and punches them
        if (m_currentAttack == 0)
        {
            EnemyUpdate();

            //Checks if boss has punched player
            CheckIfPunchAttackFinished();
        }

        //If the current attack state is the ground pound state then the boss pounds the ground
        //Checks whether the boss can ground pound to avoid them doing it every frame
        else if (m_currentAttack == 1 && m_canGroundPound)
        {
            m_canGroundPound = false;
            m_monoBehaviour.StartCoroutine(m_monoBehaviour.GetBossObject().GroundPound());
        }

        //If the boss completes the attack a new attack is chosen
        if (m_isAttackComplete)
        {
            m_isAttackComplete = false;
            ChooseAttack();
        }
    }

    /* Used for any physics or movement functions as this update is carried out more
       times per second */
    public void BossFixedUpdate()
    {
        //If the current attack state is the default the boss walks towards the player
        if (m_currentAttack == 0)
        {
            //If the boss is in a normal state they will walk towards the player
            if (m_currentEnemyState == (EnemyStates)0)
            {
                MoveTowardsPlayer();
            }
        }
    }

    //Boss punches ground and creates shockwave that can damage the player
    private IEnumerator GroundPound()
    {
        m_bossAnimationScript.SetAnimationStateToDefault();

        //Waits before starting ground pound to give player time to react
        yield return new WaitForSeconds(m_attackCoolDown);

        //Plays ground pound animation
        m_bossAnimationScript.PlayAnimation("groundPound");

        //Gets length of the current animation
        float animationLength = m_bossAnimationScript.GetCurrentAnimationLength();

        //Waits for animation to complete before spawning the shock wave
        yield return new WaitForSeconds(1);

        float spawnX = m_bossPrefab.transform.position.x - 3;
        float spawnY = m_bossPrefab.transform.position.y - 1.1f;

        Vector3 spawnPoint = new Vector3(spawnX, spawnY, m_bossPrefab.transform.position.z);

        Instantiate(m_shockwaveObject, spawnPoint, Quaternion.identity);

        //Sets ability to ground pound to true
        m_canGroundPound = true;

        //Sets attack to complete
        m_isAttackComplete = true;
    }

    //Randomly decides which attack the boss will carry out next
    private void ChooseAttack()
    {
        m_currentAttack = Mathf.RoundToInt(Random.Range(0.0f, 1.0f));
    }

    // Checks to see if the punch attack is completed
    /* Checks player's current health and compares it to their health from the previous frame to determine if 
       the Boss' attack has been completed */
    private void CheckIfPunchAttackFinished()
    {
        //Gets player's current health
        playerHealth = GameObject.Find("Player").GetComponent<PlayerScript>().GetPlayerObject().GetHealth();

        //Compares player's current health to their previous health to see if the boss has dealt them damage
        if (playerHealth < playerHealthPrevious)
        {
            //Sets attack to complete
            m_isAttackComplete = true;
        }

        playerHealthPrevious = playerHealth;
    }

    //Boss Override for taking damage
    //Done so attack can be set as complete when taking damage
    public new void TakeDamage()
    {
        base.TakeDamage();

        //Sets ability to ground pound to true
        m_canGroundPound = true;

        //Sets attack to complete
        m_isAttackComplete = true;
    }
}
