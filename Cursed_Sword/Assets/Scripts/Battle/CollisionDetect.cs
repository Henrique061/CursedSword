using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    private CharacterDamage cd;
    private CharacterController cc;
    private Skill sk;

    [SerializeField] private EnemyDamage ed;
    [SerializeField] private EnemyDamage[] edSpike;
    [SerializeField] SpikeBattle sb;
    [SerializeField] TutorialManager tm;

    [HideInInspector] public bool tongueDamaged = false;
    private bool skillTutorial = false;

    private void Awake()
    {
        cd = GetComponentInParent<CharacterDamage>();
        cc = GetComponentInParent<CharacterController>();
        sk = GetComponentInParent<Skill>();
    }

    #region Collision Detect

    private void OnTriggerEnter2D(Collider2D collision) // detect collision with triggers
    {
        CollisionProcess(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CollisionProcess(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) // detect collision with colliders
    {
        CollisionProcess(collision.gameObject);
    }

    #endregion

    void CollisionProcess(GameObject collider) // call the collision functions
    {
        #region Damage Enemy

        #region Basic Attack

        if ((this.CompareTag("Blade") && (collider.CompareTag("Enemy") || collider.CompareTag("Training")) && cc.attackDamage && cc.causeDamage && !sk.usingLowflight)) // cause damage against the enemy if it's attacking
        {
            ed.DamageEnemy("basicAttack", false, -1);
        }

        if ((this.CompareTag("Blade") && collider.CompareTag("Tongue") && cc.attackDamage && cc.causeDamage && !sk.usingLowflight)) // cause damage against the gaia's tongue (tongue has it's own tag to not cause damage to the player if he pass your guard and face through the tongue
        {
            tongueDamaged = true;
            ed.DamageEnemy("basicAttack", false, -1);
        }

        if ((this.CompareTag("Blade") && collider.CompareTag("Spike1") && cc.attackDamage && cc.spikeAttackDmg && !sk.usingLowflight))
            edSpike[0].DamageEnemy("basicAttack", true, 0);

        if ((this.CompareTag("Blade") && collider.CompareTag("Spike2") && cc.attackDamage && cc.spikeAttackDmg && !sk.usingLowflight))
            edSpike[1].DamageEnemy("basicAttack", true, 1);

        if ((this.CompareTag("Blade") && collider.CompareTag("Spike3") && cc.attackDamage && cc.spikeAttackDmg && !sk.usingLowflight))
            edSpike[2].DamageEnemy("basicAttack", true, 2);

        if ((this.CompareTag("Blade") && collider.CompareTag("Spike4") && cc.attackDamage && cc.spikeAttackDmg && !sk.usingLowflight))
            edSpike[3].DamageEnemy("basicAttack", true, 3);

        if ((this.CompareTag("Blade") && collider.CompareTag("Spike5") && cc.attackDamage && cc.spikeAttackDmg && !sk.usingLowflight))
            edSpike[4].DamageEnemy("basicAttack", true, 4);

        #endregion

        ////////////////////////////////////////////////////////////////////////////////

        #region Earthquake

        if (this.CompareTag("Earthquake") && collider.CompareTag("Enemy") && sk.earthCauseDmg)
        {
            ed.DamageEnemy("earthquake", false, -1);
        }

        if (this.CompareTag("Earthquake") && collider.CompareTag("Tongue") && sk.earthCauseDmg)
        {
            tongueDamaged = true;
            ed.DamageEnemy("earthquake", false, -1);
        }

        if ((this.CompareTag("Earthquake") && collider.CompareTag("Spike1") && sk.spikeEarthDmg[0]))
            edSpike[0].DamageEnemy("earthquake", true, 0);

        if ((this.CompareTag("Earthquake") && collider.CompareTag("Spike2") && sk.spikeEarthDmg[1]))
            edSpike[1].DamageEnemy("earthquake", true, 1);

        if ((this.CompareTag("Earthquake") && collider.CompareTag("Spike3") && sk.spikeEarthDmg[2]))
            edSpike[2].DamageEnemy("earthquake", true, 2);

        if ((this.CompareTag("Earthquake") && collider.CompareTag("Spike4") && sk.spikeEarthDmg[3]))
            edSpike[3].DamageEnemy("earthquake", true, 3);

        if ((this.CompareTag("Earthquake") && collider.CompareTag("Spike5") && sk.spikeEarthDmg[4]))
            edSpike[4].DamageEnemy("earthquake", true, 4);

        #endregion

        ////////////////////////////////////////////////////////////////////////////////

        #region Lowflight

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && collider.CompareTag("Enemy")
            && sk.usingLowflight && sk.lowflightCauseDmg)
        {
            ed.DamageEnemy("lowflight", false, -1);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && collider.CompareTag("Tongue")
            && sk.usingLowflight && sk.lowflightCauseDmg)
        {
            tongueDamaged = true;
            ed.DamageEnemy("lowflight", false, -1);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && (collider.CompareTag("Spike1"))
            && sk.usingLowflight && sk.spikeLowDmg[0])
        {
            edSpike[0].DamageEnemy("lowflight", true, 0);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && (collider.CompareTag("Spike2"))
            && sk.usingLowflight && sk.spikeLowDmg[1])
        {
            edSpike[1].DamageEnemy("lowflight", true, 1);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && (collider.CompareTag("Spike3"))
            && sk.usingLowflight && sk.spikeLowDmg[2])
        {
            edSpike[2].DamageEnemy("lowflight", true, 2);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && (collider.CompareTag("Spike4"))
            && sk.usingLowflight && sk.spikeLowDmg[3])
        {
            edSpike[3].DamageEnemy("lowflight", true, 3);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && (collider.CompareTag("Spike5"))
            && sk.usingLowflight && sk.spikeLowDmg[4])
        {
            edSpike[4].DamageEnemy("lowflight", true, 4);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////

        #region LaserBeam

        if (this.CompareTag("Laser") && collider.CompareTag("Enemy") && sk.laserCauseDmg)
        {
            ed.DamageEnemy("laser", false, -1);
        }

        if (this.CompareTag("Laser") && collider.CompareTag("Tongue") && sk.laserCauseDmg)
        {
            tongueDamaged = true;
            ed.DamageEnemy("laser", false, -1);
        }

        if (this.CompareTag("Laser") && collider.CompareTag("Spike1") && sk.spikeLaserDmg[0])
            edSpike[0].DamageEnemy("laser", true, 0);

        if (this.CompareTag("Laser") && collider.CompareTag("Spike2") && sk.spikeLaserDmg[1])
            edSpike[1].DamageEnemy("laser", true, 1);

        if (this.CompareTag("Laser") && collider.CompareTag("Spike3") && sk.spikeLaserDmg[2])
            edSpike[2].DamageEnemy("laser", true, 2);

        if (this.CompareTag("Laser") && collider.CompareTag("Spike4") && sk.spikeLaserDmg[3])
            edSpike[3].DamageEnemy("laser", true, 3);

        if (this.CompareTag("Laser") && collider.CompareTag("Spike5") && sk.spikeLaserDmg[4])
        {
            edSpike[4].DamageEnemy("laser", true, 4);
        }

        #endregion

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Damage Player

        if ((this.CompareTag("Player")    && collider.CompareTag("Enemy") && !sk.usingLowflight) // damage player if guard and face touches the enemy
            || (this.CompareTag("Blade")  && collider.CompareTag("Enemy") && !cc.ignoreBladeColl && !sk.usingLowflight) // damage player if enemy touches the blade and sword is not attacking
            || (this.CompareTag("Player") && collider.CompareTag("Vine")  && !sk.usingLowflight) // damage the player if touches the vine
            || (this.CompareTag("Blade")  && collider.CompareTag("Vine")  && !sk.usingLowflight))
        {
            cd.DamagePlayer(150);
        }

        if ((this.CompareTag("Player") || this.CompareTag("Blade")) && collider.CompareTag("TrainingSkill") && !skillTutorial)
        {
            skillTutorial = true;
            tm.PressSkill();
        }


        // SPIKES ///////////////////////////////////

        if    ((this.CompareTag("Player") && collider.CompareTag("Spike1") && !sk.usingLowflight)
            || (this.CompareTag("Blade")  && collider.CompareTag("Spike1") && !cc.ignoreBladeColl && !sk.usingLowflight)) // damage player if spike touches the blade and sword is not attacking)
        {
            if (sb.spikeHighDmg[0])
                cd.DamagePlayer(200);

            else
                cd.DamagePlayer(50);
        }

        if    ((this.CompareTag("Player") && collider.CompareTag("Spike2") && !sk.usingLowflight)
            || (this.CompareTag("Blade") && collider.CompareTag("Spike2") && !cc.ignoreBladeColl && !sk.usingLowflight)) // damage player if spike touches the blade and sword is not attacking)
        {
            if (sb.spikeHighDmg[1])
                cd.DamagePlayer(200);

            else
                cd.DamagePlayer(50);
        }

        if ((this.CompareTag("Player") && collider.CompareTag("Spike3") && !sk.usingLowflight)
            || (this.CompareTag("Blade") && collider.CompareTag("Spike3") && !cc.ignoreBladeColl && !sk.usingLowflight)) // damage player if spike touches the blade and sword is not attacking)
        {
            if (sb.spikeHighDmg[2])
                cd.DamagePlayer(200);

            else
                cd.DamagePlayer(50);
        }

        if ((this.CompareTag("Player") && collider.CompareTag("Spike4") && !sk.usingLowflight)
            || (this.CompareTag("Blade") && collider.CompareTag("Spike4") && !cc.ignoreBladeColl && !sk.usingLowflight)) // damage player if spike touches the blade and sword is not attacking)
        {
            if (sb.spikeHighDmg[3])
                cd.DamagePlayer(200);

            else
                cd.DamagePlayer(50);
        }

        if ((this.CompareTag("Player") && collider.CompareTag("Spike5") && !sk.usingLowflight)
            || (this.CompareTag("Blade") && collider.CompareTag("Spike5") && !cc.ignoreBladeColl && !sk.usingLowflight)) // damage player if spike touches the blade and sword is not attacking)
            {
            if (sb.spikeHighDmg[4])
                cd.DamagePlayer(200);

            else
                cd.DamagePlayer(50);
        }

        #endregion
    }


}
