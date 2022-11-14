/* 
Purpose: Parent class for any characters
Author:  Rhys Myring
Date:    29/06/2021
Notes:   This script is the parent class for any character in the game, it contains member variables and functions
         that are required for any player/enemy. This class should not be instanciated.
Changes: 01/08/2021 Added animation calls to certain functions so animations can be played - Rhys Myring  
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Member variables
    protected float m_movementSpeed;
    protected float m_jumpForce;
    protected float m_attackRange;
    protected float m_health;
    protected float m_originalHealth;
    private GameObject m_charactergameObject;
    protected bool m_canAttack;
    protected bool m_canJump;
    protected float m_attackCoolDown;
    protected bool m_isAlive;

    //Constructor
    public Character(float movementSpeed, float jumpForce, float attackRange, float health, GameObject characterObject,
        float attackCoolDown)
    {
        m_movementSpeed = movementSpeed;
        m_jumpForce = jumpForce;
        m_attackRange = attackRange;
        m_health = health;
        m_originalHealth = health;
        m_charactergameObject = characterObject;
        m_canAttack = true;
        m_attackCoolDown = attackCoolDown;
        m_isAlive = true;
    }

    /* Decreases health of character
       If health is zero then it kills the character */
    public void TakeDamage()
    {
        //Plays hit animation
        m_charactergameObject.GetComponent<AnimationScript>().PlayAnimation("hit");

        /* Checks if object is player, if it is, then just 1 is taken from their health
           if it is an enemy, then the damage is multiplied by the player's attack multiplier */
        if (m_charactergameObject.tag == "Player")
        {
            m_health -= 1;
        }
        else
        {
            float DamageMultiplier = GameObject.Find("Player").GetComponent<PlayerScript>().GetPlayerObject()
                .GetPlayerAttackMultiplier();

            m_health -= 1 * DamageMultiplier;
        }

        if (m_health <= 0 && m_isAlive == true)
        {
            //Checks the type of character and gets monobehaviour based on the type and then runs a coroutine
            switch (m_charactergameObject.tag)
            {
                case "Player":
                    m_charactergameObject.GetComponent<PlayerScript>().StopAllCoroutines();
                    m_charactergameObject.GetComponent<PlayerScript>().StartCoroutine(Kill(m_charactergameObject));
                    break;
                case "Enemy":
                    m_charactergameObject.GetComponent<EnemyScript>().StopAllCoroutines();
                    m_charactergameObject.GetComponent<EnemyScript>().StartCoroutine(Kill(m_charactergameObject));
                    break;
                case "Boss":
                    m_charactergameObject.GetComponent<BossScript>().StopAllCoroutines();
                    m_charactergameObject.GetComponent<BossScript>().StartCoroutine(Kill(m_charactergameObject));
                    break;
            }
        }
    }

    //Moves given game object to the left in the world
    protected void MoveLeft(GameObject gameObject, float movementSpeed)
    {
        gameObject.transform.Translate(-movementSpeed * Time.fixedDeltaTime, 0, 0);
    }

    //Moves given game object to the right in the world
    protected void MoveRight(GameObject gameObject, float movementSpeed)
    {
        gameObject.transform.Translate(movementSpeed * Time.fixedDeltaTime, 0, 0);
    }

    //Applies force to given gameobjct to make them jump
    protected void Jump(GameObject gameObject, float jumpForce)
    {
        //Applies force to make character jump
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * jumpForce, ForceMode.Impulse);
    }

    //Deactivates a given game object (unless it is the player)
    //This is done as it is more efficient than destroying an object and the object can be reused
    protected IEnumerator Kill(GameObject gameObject)
    {
        //Sets character to dead
        m_isAlive = false;

        m_charactergameObject.GetComponent<AnimationScript>().PlayAnimation("dead");

        //Gets length of the current animation
        float animationLength = m_charactergameObject.GetComponent<AnimationScript>().GetCurrentAnimationLength();

        /* Checks to see if the object being deactivated is the player or not, if it is, the object isn't deactivated.
           This is done to avoid issues that arrise with deactivating the player game object */
        if (m_charactergameObject.name != "Player")
        {
            //Waits for the death animation to finish before deactivating the game object
            //Plus another 0.5 so it doesn't disappear immediately
            yield return new WaitForSeconds(animationLength + 0.5f);

            //Deactivates object
            gameObject.SetActive(false);
        }
    }

    //Waits a set time before setting the can attack to true to prevent spam attacking
    protected IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(m_attackCoolDown);
        m_canAttack = true;
    }

    //Setter
    public void SetHealth(float health)
    {
        m_health = health;
    }

    //Getter
    public float GetHealth()
    {
        return m_health;
    }
}
