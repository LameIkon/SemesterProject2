using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider healthbar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        healthbar = GameObject.Find("Health bar").GetComponent<Slider>();
    }

    private void Start()
    {
        currentHealth = maxHealth; // set players health to max
        healthbar.maxValue = maxHealth; // ensure Slider hold 100 as max value 
        healthbar.value = maxHealth; // set health bar UI to max
    }

    // just to show that the player can take and gain health
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GainHealth(2);
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

    // this method is to be called to update UI
    void UpdateHealthBar()
    {
        healthbar.value = currentHealth;
    }

    // this should maybe be in its own script
    private bool gameIsOver;
    void GameOver()
    {        
        if (currentHealth <= 0)
        {
            Debug.Log("you have less than 0 health thus you die!");
            gameIsOver = true;
        }
    }
}
