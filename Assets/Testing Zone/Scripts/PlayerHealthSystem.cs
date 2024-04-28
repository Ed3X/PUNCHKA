using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Color originalColor;
    public Color damageColor;
    public float damageFlashTime = 0.2f;

    public HealthBar healthBar;

    private Material myMaterial;
    private float flashTimer;

    public bool isInvincible = false;

    public LayerMask WhatIsHealing;
    public Transform GroundChecker;
    public float groundSphereRadius = 0.5f;
    public DrugBar drugBar;
    public Transform Respawn;

    private void Start()
    {
        currentHealth = maxHealth;
        myMaterial = GetComponent<Renderer>().material;
        originalColor = myMaterial.color;

        healthBar.SetMaxHealth(maxHealth); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerTakesDamage(100);
        }
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            myMaterial.color = damageColor;
        }
        else
        {
            myMaterial.color = originalColor;
        }

        if (IsHealing())
        {
            drugBar.drogaActual = 100f;
        }
    }

    private bool IsHealing()
    {
        return Physics.CheckSphere(
            GroundChecker.position, groundSphereRadius, WhatIsHealing );
    }

    public void PlayerTakesDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        flashTimer = damageFlashTime;
        if (!isInvincible) // Check invulnerability after damage
        {
            StartCoroutine(InvulnerabilityTimer());
        }
        healthBar.SetHealth(currentHealth); 
    }

    IEnumerator InvulnerabilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3.0f);
        isInvincible = false;
        Debug.Log("isInvincible");
    }

    private void Die()
    {
        SceneManager.LoadScene("Testing Zone");
    }
}
