using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCollisionDetect : MonoBehaviour
{
    [SerializeField] private TutorialSceneBeginManager tsbm;

    [HideInInspector] public bool canTransition2 = false;

    private void OnTriggerEnter2D(Collider2D collision) // detect collision with triggers
    {
        CollisionProcess(collision.gameObject);
    }

    private void CollisionProcess(GameObject collider)
    {
        if (collider.CompareTag("Transition1") && !canTransition2)
        {
            tsbm.transition1 = true;
        }

        else if (collider.CompareTag("Transition2") && canTransition2)
        {
            tsbm.transition2 = true;
        }
    }
}
