using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public bool dmgTakenBoosted;
    public float dmgTakenBoost;

    public bool healBoosted;
    public float healBoost;

    public bool healNegated;


    private float fullHealth = 200f;
    private float fullArmor = 50f;

    private float armorDmgReducer = 0.7f;

    public float currHealth;
    public float currArmor;

    public float combinedStartHealth;

    public Image healthBar;
    public Image armorBar;

    private float healthAndArmorBarChangeRate = 3f;

    private void Awake()
    {
        currHealth = fullHealth;
        currArmor = fullArmor;
        combinedStartHealth = currHealth + currArmor;
    }

    void Update()
    {
        if (healthBar.fillAmount != currHealth / fullHealth)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currHealth / fullHealth, healthAndArmorBarChangeRate * Time.deltaTime);
        }
        if (armorBar.fillAmount != currArmor / fullArmor)
        {
            armorBar.fillAmount = Mathf.Lerp(armorBar.fillAmount, currArmor / fullArmor, healthAndArmorBarChangeRate * Time.deltaTime);
        }
    }
    public void TakeDamage(float dmg)
    {
        if (dmgTakenBoosted && dmgTakenBoost != 0)
        {
            dmg *= dmgTakenBoost;
        }

        if (currArmor > 0)
        {
            currArmor -= dmg * armorDmgReducer;
            if (currArmor < 0)
            {
                currArmor = 0;
            }
        }
        else
        {
            currHealth -= dmg;
        }
    }

    public void Heal(float healAmount)
    {
        if (healNegated)
        {
            return;
        }

        if (healBoosted && healBoost != 0)
        {
            healAmount *= healBoost;
        }

        if (currArmor <= 0)
        {
            if (currHealth + healAmount <= fullHealth)
            {
                currHealth += healAmount;
            }
            else
            {
                currArmor += healAmount - (currHealth - fullHealth);
                currHealth = fullHealth;
                Mathf.Clamp(currArmor, 0, fullArmor);

            }
        }
        else if (currArmor < fullArmor)
        {
            currArmor += healAmount;
            Mathf.Clamp(currArmor, 0, fullArmor);
        }

    }
}
