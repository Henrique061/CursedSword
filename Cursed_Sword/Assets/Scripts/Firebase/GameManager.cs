/// GameManager
/// @Author: Eduardo Gonelli
/// Created at 22/10/2022

using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreTotal;
    [SerializeField] TextMeshProUGUI scoreFinal;
    [SerializeField] TextMeshProUGUI rankFinal;
    //[SerializeField] TMP_InputField inputName;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Button sendDataButton;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] DataManager dm;
    [SerializeField] int scoreForKill = 50;
    private int score;
    private int rank = 0;
    private bool gameOver = false;
    string[] playersName;
    int[] playersScore;
    Dictionary<string, int> topTenPlayersScoreUnsorted;

    void Start()
    {
        // we are using InvokeReapeting because the data
        // loading is asynchronous and may have a delay.
        InvokeRepeating("GetDatamanagerScore", 0.2f, 0.2f);
        scoreTotal.text = "Loading from Database.\nPlease, wait...";
        // limits target framerate to save mobile device battery
        //Application.targetFrameRate = 30;        
    }

    public void GetDatamanagerScore()
    {
        // if the data is not ready, skip the rest of the method
        if (!dm.isDatabaseOk)
        {
            return;
        }
        // if the data has been loaded we request it from the DataManager
        dm.GetLoadedData();
        // cancels InvokeReapeting as the data is ready
        CancelInvoke("GetDatamanagerScore");
        // after the data has been handled by GetLoadedData, we get the created arrays
        playersName = dm.GetName();
        playersScore = dm.GetScore();
        score = 0;
        scoreText.text = "Score: 000000";
        scoreTotal.text = "Top 10 Scores\n\n";
        // create a dictionary from it
        topTenPlayersScoreUnsorted = new Dictionary<string, int>();
        for (int i = 0; i < playersName.Length; i++)
        {
            topTenPlayersScoreUnsorted.Add(playersName[i], playersScore[i]);
        }
        // sort dictionary to display on screen
        int counterTop10 = 10;
        foreach (KeyValuePair<string, int> plr in topTenPlayersScoreUnsorted.OrderByDescending(key => key.Value))
        {
            if (counterTop10 > 0)
            {
                scoreTotal.text += plr.Key + ": " + plr.Value + "\n";
                counterTop10--;
            }
            else { break; }
        }
        // if the number of scores found in the bank is less than 10 items.
        if (counterTop10 > 0)
        {
            for (int i = 0; i < counterTop10; i++)
            {
                scoreTotal.text += "Not Set: 000000\n";
            }
        }
    }

    // updates the score on destroying an enemy
    public void UpdateScore()
    {
        score += scoreForKill;
        scoreText.text = "Score: " + score.ToString("000000");
    }

    public void GameOver()
    {
        gameOver = true;
        scoreFinal.text = "Score: " + score.ToString("000000");

        bool isInRank = false; // to control if the player is ranked in the top 10
        // lists the dictionary in a KeyValuePair (uses Linq)
        // and orders the score from highest to lowest
        foreach (KeyValuePair<string, int> plr in topTenPlayersScoreUnsorted.OrderByDescending(key => key.Value))
        {
            rank++;
            if (score > plr.Value)
            {
                isInRank = true;
                break;
            }
        }
        // if the player is in the top 10, the message that
        // he is within the rank is displayed. Otherwise, it
        // is displayed that it is not in the top 10.
        rankFinal.text = "Rank: " + (!isInRank ? "Out of top 10!" : (rank).ToString("000000"));
        // show game over screen
        gameOverPanel.SetActive(true);

        // se o jogador nao estiver logado, nao habilita o
        // botao de salvar e a pontuacao nao sera enviada.        
        if (AuthManager.instance.CheckIfThePlayerIsLoggedIn() == false)
        {
            sendDataButton.interactable = false;
            resultText.text = "Player not logged in. The score will not be recorded.";
            Debug.Log("Não está logado!");
        }
        else
        {
            Debug.Log(AuthManager.instance.GetPlayerName());
        }
    }
    // reload the scene and use GetSceneBuildIndex(0) to get which scene is open and reload
    public void NewGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void SetRankingScore()
    {
        // disable the button
        sendDataButton.interactable = false;
        // receive player's name from the input field
        string playerName = AuthManager.instance.GetPlayerName();
        // if player's name is empty, set a default value
        if (playerName == "") { playerName = AuthManager.instance.GetPlayerEmail(); }
        // check if actual score is bigger than previos score
        dm.GetOneChildScoreToCheckIfAtualScoreIsBigger(playerName, score);
        SetResultMessage();
    }

    // based on DataManager public variable result, check if
    // result is lesser (-1), then decide is data must be sent.
    public void SetResultMessage()
    {
        if (dm.getResultSuccessfully)
        {
            if (dm.result == -1)
            { // if actual score le lesser than previous score
                resultText.text = "This score is less than previos Score!\nData not sent!";
                return;
            }
            // if actual score is equal, bigger or even the player is not
            // in the ranking, send new score to database
            else
            {
                // receive player's name from the input field
                string playerName = AuthManager.instance.GetPlayerName();
                if (playerName == "") playerName = AuthManager.instance.GetPlayerEmail();
                // if player's name is empty, set a default value
                if (playerName == "") { playerName = "Anonymous"; }
                bool sendData = dm.SendDataToDatabase(playerName, score);
                // if data was sent successfully
                if (sendData) { resultText.text = "Data sent successfully!"; }
                // if any error occurs
                else { resultText.text = "Error sending data to database!"; }
            }
        }
    }
}
