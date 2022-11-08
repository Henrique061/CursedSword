using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBeginManager : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] CharacterMovement cm;
    [SerializeField] CharacterDamage cd;
    [SerializeField] Skill sk;
    [SerializeField] Animator[] vineAnims;

    [HideInInspector] public bool battleBegin = false;

    void Start()
    {
        cc.canAttack = false;
        cc.canJump = false;
        cm.canWalk = false;
        sk.canUseSkill = false;
        cd.cannotAttack = true;
        PauseController.canPause = false;
        PauseController.gamePaused = false;
        FindObjectOfType<AudioManager>().PlaySound("BattleMusic");

        StartCoroutine("BeginBattle");
    }

    IEnumerator BeginBattle()
    {
        yield return new WaitForSeconds(2);

        vineAnims[0].SetTrigger("Rise");
        vineAnims[1].SetTrigger("Rise");
        FindObjectOfType<AudioManager>().PlaySound("VineRise");

        battleBegin = true;
        cc.canAttack = true;
        cc.canJump = true;
        cm.canWalk = true;
        sk.canUseSkill = true;
        cd.cannotAttack = false;
        PauseController.canPause = true;
    }
}
