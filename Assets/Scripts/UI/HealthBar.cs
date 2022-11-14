using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 10f;
    PlayerScript playerScript;
    public Sprite[] lifeImages = new Sprite[6];
    public Sprite healthBarCurrentImage;
    public GameObject deathMenu;

    private void Start()
    {
        //Find
        healthBar = GetComponent<Image>();
        playerScript = FindObjectOfType<PlayerScript>();
        healthBarCurrentImage = GetComponent<Image>().sprite;
    }

    private void Update()
    {
        //healthBarCurrentImage = lifeImages[3];
        GetComponent<Image>().sprite = lifeImages[3];

        Debug.Log("Image changed");

        //Gets Player's current health
        currentHealth = playerScript.GetPlayerObject().GetHealth() / 2;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth > 4)
        {
            GetComponent<Image>().sprite = lifeImages[5];
        }
        else if (currentHealth > 3)
        {
            GetComponent<Image>().sprite = lifeImages[4];
        }
        else if (currentHealth > 2)
        {
            GetComponent<Image>().sprite = lifeImages[3];
        }
        else if (currentHealth > 1)
        {
            GetComponent<Image>().sprite = lifeImages[2];
        }
        else if (currentHealth > 0)
        {
            GetComponent<Image>().sprite = lifeImages[1];
        }
        else
        {
            GetComponent<Image>().sprite = lifeImages[0];
        }

        if (playerScript.GetPlayerObject().GetIsAlive() == false)
        {
            deathMenu.SetActive(true);
        }
    }
}
