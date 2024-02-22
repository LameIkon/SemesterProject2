using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider healthbar;
    [SerializeField] private int maxHealth = 100; // decides the max health a player can get
    [SerializeField] private int currentHealth; // the current amount the player have at the given moment

    private void Start()
    {
        healthbar = GameObject.Find("Health bar").GetComponent<Slider>();
        currentHealth = maxHealth; // set players health to max
        healthbar.maxValue = maxHealth; // ensure Slider hold 100 as max value 
        healthbar.value = maxHealth; // set health bar UI to max
    }

    private void Awake()
    {
        
    }

    // just to show that the player can take and gain health
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            TakeDamage(2); // testing if the player loses 2 hp
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GainHealth(2); // testing if the player gains 2 hp
        }

        if (!gameIsOver)
        {
            GameOver();
        }
        
    }

    // this method is to be called when player take damage
    void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            UpdateHealthBar();
        }       
    }

    // this method is to be called when player gain hp
    void GainHealth(int health)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += health;
            UpdateHealthBar();
        }
        
    }

    // this method is to be called to update health bar UI
    void UpdateHealthBar()
    {
        healthbar.value = currentHealth;
    }

    // this should maybe be in its own script
    // the method checks if the player has 0 or less health 
    private bool gameIsOver;
    void GameOver()
    {        
        if (currentHealth <= 0)
        {
            Debug.Log("you have reached 0 health thus you die!");
            gameIsOver = true; 
        }
    }
}
