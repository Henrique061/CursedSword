using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRanking : MonoBehaviour
{
    [HideInInspector] public string score;
    float timer = 0;

    [HideInInspector] public string playerName;

    [HideInInspector] public bool isPlaying = true;
    private void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
        }
        else 
        {
            if (score == null)
            {
                SetScore();
            }
        }
    }

    public void SetName(string name) 
    {
        this.playerName = name;
    }

    void SetScore()
    {
        int scoreS = (int)timer % 60;

        int scoreM = scoreS % 60;
        scoreS = scoreS / 60;

        int scoreH = scoreM % 60;
        scoreM = scoreM / 60;

        score = $"{scoreH.ToString("00")}h:{scoreM.ToString("00")}m:{scoreS.ToString("00")}s";
    }

    public void NotPlaying() 
    {
        isPlaying = false;
    }
}
