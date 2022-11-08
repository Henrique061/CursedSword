using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] Skill sk;
    [SerializeField] Material hurtMaterial;
    [SerializeField] private SpriteRenderer spikeRenderer;
    [SerializeField] private SpriteRenderer tongueRenderer;
    [SerializeField] private SpriteRenderer trainingRenderer;

    [HideInInspector] public float fireupVariation = 1;

    private Health he;
    private Material defaultMaterial;
    private SpriteRenderer[] srChildren;

    private void Awake()
    {
        he = GetComponent<Health>();
        srChildren = GetComponentsInChildren<SpriteRenderer>();

        if (!this.CompareTag("Spike1") && !this.CompareTag("Spike2") && !this.CompareTag("Spike3") && !this.CompareTag("Spike4") && !this.CompareTag("Spike5"))
            defaultMaterial = srChildren[0].material;

        else
            defaultMaterial = spikeRenderer.material;
    }

    public void DamageEnemy(string dmgSource, bool isSpike, int spikeIndex)
    {
        switch (dmgSource)
        {
            case "basicAttack":
                if (isSpike)
                {
                    cc.spikeAttackDmg = false; // to cause only one hit damage per attack
                    he.HealthLossVariation(cc.attackDamageValue * fireupVariation, true);
                }

                else
                {
                    cc.causeDamage = false; // to cause only one hit damage per attack
                    he.HealthLossVariation(cc.attackDamageValue * fireupVariation, false);
                }

                break;

            case "lowflight":
                if (isSpike)
                {
                    sk.spikeLowDmg[spikeIndex] = false;
                    he.HealthLossStatic(sk.lowflightDamage, true);
                }

                else
                {
                    sk.lowflightCauseDmg = false;
                    he.HealthLossStatic(sk.lowflightDamage, false);
                }

                break;

            case "earthquake":
                if (isSpike)
                {
                    sk.spikeEarthDmg[spikeIndex] = false;
                    he.HealthLossStatic(sk.earthquakeDamage, true);
                }

                else
                {
                    sk.earthCauseDmg = false;
                    he.HealthLossStatic(sk.earthquakeDamage, false);
                }

                break;

            case "laser":
                if (isSpike)
                {
                    sk.spikeLaserDmg[spikeIndex] = false;
                    he.HealthLossStatic(sk.laserDamage, true);
                }

                else
                {
                    sk.laserCauseDmg = false;
                    he.HealthLossStatic(sk.laserDamage, false);
                }

                break;
        }

        StartCoroutine("SpriteBlink");

        FindObjectOfType<AudioManager>().PlaySound("AttackHit");
    }

    IEnumerator SpriteBlink()
    {
        if (!this.CompareTag("Spike1") && !this.CompareTag("Spike2") && !this.CompareTag("Spike3") && !this.CompareTag("Spike4") && !this.CompareTag("Spike5"))
        {
            if (this.CompareTag("Training"))
                trainingRenderer.material = hurtMaterial;

            else
            {
                foreach (SpriteRenderer sprite in srChildren)
                    sprite.material = hurtMaterial;

                tongueRenderer.material = hurtMaterial;
            }

        }

        else // if is spike
        {
            spikeRenderer.material = hurtMaterial;
        }

        yield return new WaitForSeconds(.1f);

        if (!this.CompareTag("Spike1") && !this.CompareTag("Spike2") && !this.CompareTag("Spike3") && !this.CompareTag("Spike4") && !this.CompareTag("Spike5"))
        {
            if (this.CompareTag("Training"))
                trainingRenderer.material = defaultMaterial;

            else
            {
                foreach (SpriteRenderer sprite in srChildren)
                    sprite.material = defaultMaterial;

                tongueRenderer.material = defaultMaterial;
            }

        }

        else // if is spike
        {
            spikeRenderer.material = defaultMaterial;
        }
    }
}
