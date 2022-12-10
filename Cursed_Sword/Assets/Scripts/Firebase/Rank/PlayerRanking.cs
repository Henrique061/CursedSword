using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerRanking : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [HideInInspector] public static float timer = 0; //tempo que ele jogou
    [HideInInspector] public static string playerName; //nome do jogador

    [HideInInspector] public string score; //tempo que jogador jogou em 00h : 00m : 00s

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

            SetScore(timer);

            if (timerTxt != null)
                timerTxt.text = score;
        }
        else 
        {
            if (score == null)
            {
                SetScore(timer);
            }
        }
    }

    public void SetName(string name) 
    {
        playerName = name;
    }

    public void SetScore(float timer)
    {
        gm.UpdateScore();
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
