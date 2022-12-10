using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndBattleController : MonoBehaviour
{
    [Header("Scripts Instantiations")]
    [SerializeField] private Health enemyHealth;
    [SerializeField] private CharacterController cc;
    [SerializeField] private CharacterMovement cm;
    [SerializeField] private CharacterDamage cd;
    [SerializeField] private Health he;
    [SerializeField] private Skill sk;

    [Header("GameObjects")]
    [SerializeField] private GameObject mainObj;
    [SerializeField] private GameObject scoreManager;
    [SerializeField] private GameObject gameManager;

    public GameManager gm;

    private float timer = 0.1f;

    private bool isDead = true;
    private bool ending = false;


    private void Update()
    {
        if (enemyHealth.currentHealth <= 0)
        {
            PauseController.canPause = false;
            PauseController.gamePaused = false;
            if (isDead)
            {
                if (timer <= 0)
                    isDead = false;

                else
                    timer -= Time.unscaledDeltaTime;
            }

            else
            {
                if (!ending)
                {
                    scoreManager.GetComponent<PlayerRanking>().NotPlaying();

                    cc.canAttack = false;
                    cc.canJump = false;
                    cm.canWalk = false;
                    sk.canUseSkill = false;
                    he.damageable = false;
                    cc.attackDelay = false;
                    cd.cannotAttack = true;
                    FindObjectOfType<AudioManager>().StopAll();
                    mainObj.SetActive(true);
                    timer = 12f;
                    ending = true;
                    Time.timeScale = 0;
                }
            }
        }

        if (ending)
        {
            if (timer <= 0)
            {
                AudioListener.pause = false;
                PauseController.canPause = true;
                PauseController.gamePaused = false;
                Time.timeScale = 1;
                DontDestroyOnLoad(scoreManager);
                DontDestroyOnLoad(gameManager);
                gm.GameOver();
                //SceneManager.LoadScene("Ranking");
            }

            else
                timer -= Time.unscaledDeltaTime;
        }
    }

    /*public void ToRank() 
    {
        SceneManager.LoadScene("Ranking");
        Time.timeScale = 1;
    }*/
}
