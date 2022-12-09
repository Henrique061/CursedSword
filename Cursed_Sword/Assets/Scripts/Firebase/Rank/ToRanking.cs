using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRanking : MonoBehaviour
{
    [SerializeField] GameObject rank;
    [SerializeField] Transform pai;
    void Start()
    {
        for (int i = 0; i < 10; i++) 
        {
            GameObject rnk = Instantiate(rank, pai);

            rnk.GetComponent<TextsOnRank>().texts[0].text = $"{i + 1} - ";
            rnk.GetComponent<TextsOnRank>().texts[1].text = $"{GameManager.scoreTotalName[i]}";
            rnk.GetComponent<TextsOnRank>().texts[2].text = $"{GameManager.scoreTotalTime[i]}";

        }
    }
}
