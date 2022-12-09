// Criado por: Henrique Batista de Assis
// Data: 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string playerTime;

    public PlayerData(string playerName, float playerTime)
    {
        this.playerName = playerName;
        SetScore(playerTime);
    }

    void SetScore(float timer)
    {
        int scoreH = (int)timer % 60;

        int scoreM = scoreH % 60;
        scoreH = scoreH / 60;

        int scoreS = scoreM % 60;
        scoreM = scoreM / 60;

        this.playerTime = $"{scoreH.ToString("00")}h :{scoreM.ToString("00")}m :{scoreS.ToString("00")}s";
    }


}
