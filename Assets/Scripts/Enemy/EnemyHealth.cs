using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float fullHealth;
    public float health;

    public Image healthBar;
    private float healthBarChangeRate = 3f;

    private float EnemyAHealth = 200f;
    private float EnemyBHealth = 400f;
    private float EnemyCHealth = 600f;

    private float BossHealth = 25000;
    private void Awake()
    {
        switch (gameObject.tag)
        {
            case "EnemyA":
                fullHealth = EnemyAHealth;
                break;
            case "EnemyB":
                fullHealth = EnemyBHealth;
                break;
            case "EnemyC":
                fullHealth = EnemyCHealth;
                break;
            case "Boss":
                fullHealth = BossHealth;
                break;
            default:
                Destroy(gameObject);
                break;
        }
        health = fullHealth;
    }

    private void Update()
    {
        if (healthBar.fillAmount != health / fullHealth)
        { 
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / fullHealth, healthBarChangeRate * Time.deltaTime);
        }
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Enemy died");
    }
}
