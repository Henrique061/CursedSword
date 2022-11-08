using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector] public bool damageable = true; // to check if can damage the player
    [HideInInspector] public float currentHealth; // the current health of player/enemy
    [HideInInspector] public bool spikeBreak = false;
    [HideInInspector] public float dmgTongue;

    [SerializeField] private HealthDamageBar hdb;
    [SerializeField] private TongueBattleManager tbm;
    [SerializeField] private CollisionDetect[] cd;

    [Header("Spike")]
    [SerializeField] private SpriteRenderer srSpike;

    private float healthBarLoss; // to know the value to pass to the health bar (cause some dmgs are random values)
    private float healthBarLossNormalized; // to pass a damage value normilized between 0-1
    private float beforeHealth; // to know how much damage to pass to the health by calculation

    private Color spikeColor;

    public float maxHealth = 1000; // the max health of player/enemy

    private void Awake()
    {
        if (srSpike != null)
            spikeColor = srSpike.GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        beforeHealth = currentHealth;
    }

    public void HealthLossVariation(float dmg, bool isSpike)
    {
        foreach (CollisionDetect tongueDmg in cd)
            if (tongueDmg.tongueDamaged)
            {
                tongueDmg.tongueDamaged = false;
                DamageTongue(dmg);
            }

        currentHealth -= Random.Range(dmg - 3, dmg + 3);

        if (!isSpike)
        {
            if (!this.CompareTag("Training"))
            {
                healthBarLoss = beforeHealth - currentHealth;
                beforeHealth = currentHealth;
                healthBarLossNormalized = healthBarLoss / maxHealth;

                hdb.SetHealthBar(healthBarLossNormalized);
            }
        }

        else // if is spike
        {
            spikeColor.b = currentHealth / maxHealth;
            spikeColor.g = currentHealth / maxHealth;

            srSpike.color = spikeColor;

            if (currentHealth <= 0)
            {
                spikeBreak = true;
            }
        }

    }

    public void HealthLossStatic(float dmg, bool isSpike)
    {
        foreach (CollisionDetect tongueDmg in cd)
            if (tongueDmg.tongueDamaged)
            {
                tongueDmg.tongueDamaged = false;
                DamageTongue(dmg);
            }

        currentHealth -= dmg;

        if (!isSpike)
        {
            healthBarLoss = beforeHealth - currentHealth;
            beforeHealth = currentHealth;
            healthBarLossNormalized = healthBarLoss / maxHealth;

            hdb.SetHealthBar(healthBarLossNormalized);
        }

        else // if is spike
        {
            spikeColor.b = currentHealth / maxHealth;
            spikeColor.g = currentHealth / maxHealth;

            srSpike.color = spikeColor;

            if (currentHealth <= 0)
            {
                spikeBreak = true;
            }
        }
    }

    public void DamageTongue(float dmg)
    {
        tbm.stageHealth -= dmg;
    }

}
