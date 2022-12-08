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

    public PlayerData(string playerName, string playerTime)
    {
        this.playerName = playerName;
        this.playerTime = playerTime;
    }
}
