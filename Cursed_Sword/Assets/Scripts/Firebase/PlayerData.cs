using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible for cataloging the information for the database
/// </summary>
/// <param name="PlayerData"> </param>
public class PlayerData 
{ 
    [Header("Data Info")]
    public string _playerName;
    public int _playerScore;


    public PlayerData(string _playerName, int _playerScore)
    {
        this._playerName = _playerName;
        this._playerScore = _playerScore;
    }

}
