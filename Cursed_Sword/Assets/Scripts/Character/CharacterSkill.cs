using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSkill : MonoBehaviour
{
    private InputMaster input;

    private Skill skill;

    private void Awake()
    {
        skill = GetComponent<Skill>();
        input = new InputMaster();

        input.PlayerControl.Skill_1.performed += ctx => skill.SkillUse(1);
        input.PlayerControl.Skill_2.performed += ctx => skill.SkillUse(2);
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (skill.usingSkill)
            {
                input.PlayerControl.Skill_1.Disable();
                input.PlayerControl.Skill_2.Disable();
            }

            else
            {
                input.PlayerControl.Enable();
                input.PlayerControl.Skill_1.Enable();
                input.PlayerControl.Skill_2.Enable();
            }
        }

        else
            input.PlayerControl.Disable();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
