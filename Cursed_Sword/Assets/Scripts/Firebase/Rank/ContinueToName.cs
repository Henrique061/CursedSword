using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinueToName : MonoBehaviour
{
    public void Name(TMP_Text name) 
    {
        PlayerRanking pr = GameObject.Find("scoreManager").GetComponent<PlayerRanking>();
        pr.SetName(name.text);
    }
}
