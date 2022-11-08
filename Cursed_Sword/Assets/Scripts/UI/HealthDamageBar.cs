using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDamageBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image damageBar;
    [SerializeField] private Health he;

    [SerializeField] private float fixedDamageTimer = 1;
    [SerializeField] private float damageBarLossRate = 1.5f;

    private bool damageReceived = false;
    private bool damageBarDecrease = false;
    private bool canLossDamageBar = false;
    private float damageBarDeacreseTimer; // to make damage bar start deacrease only when the timer ends;
    private float totalHealthLoss = 0; // to know how much to deacrese of damage bar
    private float damageBarLossAmount = 0; // to lock the amount of damage bar loss
    private float beforeDamageLossValue;
    private float damageLossValue;

    private void Start()
    {
        damageBarLossRate /= he.maxHealth;
    }

    private void Update()
    {
        if (damageBar.fillAmount > healthBar.fillAmount)
        {
            if (damageReceived)
            {
                if (!damageBarDecrease)
                {
                    damageBarDeacreseTimer -= Time.deltaTime;
                    canLossDamageBar = true;
                }
            }

            if (damageBarDeacreseTimer <= 0 && canLossDamageBar)
            {
                damageBarLossAmount = totalHealthLoss;
                damageBarDecrease = true;
                canLossDamageBar = false;
                damageReceived = false;
            }

            if (damageBarDecrease && damageBarLossAmount > 0)
            {
                SetDamageBar(damageBarLossRate);
                damageBarLossAmount -= damageLossValue;
            }

            if (damageBarDecrease && damageBarLossAmount <= 0)
            {
                damageBarDecrease = false;
                damageBarLossAmount = 0;
            }
        }

        else
        {
            damageReceived = false;
            damageBarDecrease = false;
            damageBarDeacreseTimer = 0;
            canLossDamageBar = false;
            damageBarLossAmount = 0;
        }
    }

    public void SetHealthBar(float healthDecrase) // healthDeacrease must be 0-1 value
    {
        healthBar.fillAmount -= healthDecrase;
        damageReceived = true;
        totalHealthLoss += healthDecrase;

        damageBarDeacreseTimer = fixedDamageTimer;
    }

    public void SetDamageBar(float damageDecrease) // damageDeacrease must be 0-1 value
    {
        beforeDamageLossValue = damageBar.fillAmount;
        damageBar.fillAmount -= damageDecrease;
        damageLossValue = beforeDamageLossValue - damageBar.fillAmount; 
    }
}
