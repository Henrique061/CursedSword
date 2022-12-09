using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerRanking : MonoBehaviour
{
    [HideInInspector] public string score;
    float timer = 0;

    [HideInInspector] public string playerName;

    [HideInInspector] public bool isPlaying = true;
    [SerializeField] TMP_Text timerTxt;

    /*private void Start()
    {
        if(SceneManager.GetActiveScene().Equals("Gaia_Room"))
            DontDestroyOnLoad(this.gameObject);
    }*/

    private void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;

            SetScore();

            if (timerTxt != null)
                timerTxt.text = score;
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
        int scoreH = (int)timer % 60;

        int scoreM = scoreH % 60;
        scoreH = scoreH / 60;

        int scoreS = scoreM % 60;
        scoreM = scoreM / 60;

        score = $"{scoreH.ToString("00")}h :{scoreM.ToString("00")}m :{scoreS.ToString("00")}s";
    }

    public void NotPlaying() 
    {
        isPlaying = false;
    }
}
