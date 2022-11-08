using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [Header("Skill Icons HUD")]
    [SerializeField] private GameObject lowflightIcon;
    [SerializeField] private GameObject earthquakeIcon;
    [SerializeField] private GameObject fireupIcon;
    [SerializeField] private GameObject laserIcon;

    [Header("Skill Activated HUD")]
    public GameObject skill1Activated;
    public GameObject skill2Activated;

    [Header("Cooldown Black")]
    public GameObject cooldownBlack1;
    public GameObject cooldownBlack2;

    [Header("Cooldown Text HUD")]
    [SerializeField] private Text cooldownText1;
    [SerializeField] private Text cooldownText2;

    [HideInInspector] public bool skill1Used = false;
    [HideInInspector] public bool skill2Used = false;

    private float skill1Cooldown;
    private float skill2Cooldown;
    private float fixedSkill1Cooldown;
    private float fixedSkill2Cooldown;
    private float showCooldown1;
    private float showCooldown2;
    private Image skill2Image;
    private Skill skill;

    private void Awake()
    {
        skill = GetComponent<Skill>();
    }

    private void Start()
    {
        skill.skill1 = SkillChooseController.skill1;
        skill.skill2 = SkillChooseController.skill2;

        switch (skill.skill1)
        {
            case "lowflight":
                fixedSkill1Cooldown = skill.flyCooldown;
                break;

            case "earthquake":
                fixedSkill1Cooldown = skill.earthCooldown;
                break;

            case "fireup":
                fixedSkill1Cooldown = skill.fireupCooldown;
                break;

            case "laser":
                fixedSkill1Cooldown = skill.laserCooldown;
                break;
        }

        switch (skill.skill2)
        {
            case "lowflight":
                fixedSkill2Cooldown = skill.flyCooldown;
                break;

            case "earthquake":
                fixedSkill2Cooldown = skill.earthCooldown;
                break;

            case "fireup":
                fixedSkill2Cooldown = skill.fireupCooldown;
                break;

            case "laser":
                fixedSkill2Cooldown = skill.laserCooldown;
                break;
        }

        skill1Cooldown = fixedSkill1Cooldown;
        showCooldown1 = fixedSkill1Cooldown;
        skill2Cooldown = fixedSkill2Cooldown;
        showCooldown2 = fixedSkill2Cooldown;

        SkillIconShow(skill.skill1, skill.skill2);
    }

    private void Update() // to make the cooldown countdown
    {
        if (!PauseController.gamePaused)
        {
            if (skill1Used)
            {
                if (!cooldownBlack1.activeSelf)
                {
                    cooldownBlack1.SetActive(true);
                    cooldownText1.gameObject.SetActive(true);
                }

                cooldownText1.text = showCooldown1.ToString("0");
                skill1Cooldown -= Time.deltaTime;
                showCooldown1 = Mathf.CeilToInt(skill1Cooldown);

                if (skill1Cooldown <= 0)
                {
                    cooldownBlack1.SetActive(false);
                    cooldownText1.gameObject.SetActive(false);
                    skill1Cooldown = fixedSkill1Cooldown;
                    showCooldown1 = fixedSkill1Cooldown;
                    skill1Used = false;
                }

            }

            if (skill2Used)
            {
                if (!cooldownBlack2.activeSelf)
                {
                    cooldownBlack2.SetActive(true);
                    cooldownText2.gameObject.SetActive(true);
                }

                cooldownText2.text = showCooldown2.ToString("0");
                skill2Cooldown -= Time.deltaTime;
                showCooldown2 = Mathf.CeilToInt(skill2Cooldown);

                if (skill2Cooldown <= 0)
                {
                    cooldownBlack2.SetActive(false);
                    cooldownText2.gameObject.SetActive(false);
                    skill2Cooldown = fixedSkill2Cooldown;
                    showCooldown2 = fixedSkill2Cooldown;
                    skill2Used = false;
                }

            }
        }

    }

    public void SkillIconShow(string skill1, string skill2)
    {
        switch (skill1)
        {
            case "lowflight":
                lowflightIcon.SetActive(true);
                break;

            case "earthquake":
                earthquakeIcon.SetActive(true);
                break;

            case "fireup":
                fireupIcon.SetActive(true);
                break;

            case "laser":
                laserIcon.SetActive(true);
                break;
        }

        switch (skill2)
        {
            case "lowflight":
                skill2Image = lowflightIcon.GetComponent<Image>();
                lowflightIcon.SetActive(true);
                break;

            case "earthquake":
                skill2Image = earthquakeIcon.GetComponent<Image>();
                earthquakeIcon.SetActive(true);
                break;

            case "fireup":
                skill2Image = fireupIcon.GetComponent<Image>();
                fireupIcon.SetActive(true);
                break;

            case "laser":
                skill2Image = laserIcon.GetComponent<Image>();
                laserIcon.SetActive(true);
                break;
        }

        skill2Image.rectTransform.anchoredPosition = new Vector2(123, skill2Image.rectTransform.anchoredPosition.y);
    }

    #region Skill Cooldown

    public void SkillCooldown(float cooldown1, float cooldown2)
    {
        if (cooldown1 > 0)
            StartCoroutine(CooldownTimer(cooldown1, true)); // true if the skill called is the skill number 1

        else
            StartCoroutine(CooldownTimer(cooldown2, false)); // false if the skill called is the skill number 2
    }

    IEnumerator CooldownTimer(float cooldown, bool isSkill1)
    {
        if (isSkill1)
            skill.canSkill1 = false; // cannot use skill number 1, if skill 1 is true
        else
            skill.canSkill2 = false; // cannot use skill number 2, if skill 1 is false

        yield return new WaitForSeconds(cooldown); // wait to use the correlated skill again, by the cooldown passed by parameter

        if (isSkill1)
            skill.canSkill1 = true; // can use skill number 1 again, if skill 1 was true
        else
            skill.canSkill2 = true; // can use skill number 2 again, if skill 2 was true

        skill.laserCauseDmg = true; // to not apply a second damage in the wrong timing

        for (int i = 0; i < skill.spikeLaserDmg.Length; i++)
            skill.spikeLaserDmg[i] = true;

        FindObjectOfType<AudioManager>().PlaySound("SkillActive");
    }

    #endregion
}
