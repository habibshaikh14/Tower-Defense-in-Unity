using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    public float startHealth = 200f;
    private float health;

    public int prize = 25;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

	public void Start ()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float rate)
    {
        speed = startSpeed * (1f - rate); 
    }

    public void Die ()
    {
        PlayerStats.Money += prize;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }

}
