using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarBlink : MonoBehaviour
{
    [SerializeField] private Health he;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("Health", he.currentHealth);
    }
}
