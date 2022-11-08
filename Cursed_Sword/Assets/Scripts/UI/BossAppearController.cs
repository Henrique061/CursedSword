using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppearController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private SkillChooseController skillChoose;
    [SerializeField] private GameObject skillCanvas;

    private bool startAnim = false;

    private float animTime = 1;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animTime <= 0 && !startAnim)
        {
            animTime = 9.25f;
            FindObjectOfType<AudioManager>().PlaySound("BossAppear");
            anim.SetTrigger("Appear");
            startAnim = true;
        }

        else if (animTime <= 0 && startAnim)
        {
            Destroy(this.gameObject);
            skillCanvas.SetActive(true);
        }

        else
            animTime -= Time.deltaTime;
    }
}
