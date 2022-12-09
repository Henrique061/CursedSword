/// DataManager
/// @Author: Eduardo Gonelli
/// Created at 22/10/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DataManager : MonoBehaviour
{
    [SerializeField] GameManager gm;
    string[] playersName;
    int[] playersScore;

    DatabaseReference reference;
    DataSnapshot rawData;
    public bool isDatabaseOk = false;
    FirebaseApp app;
    private void Start()
    {
        // este bloco comentado esta dando carregamento infinito nessa
        // versao da unity. foi comentado para evitar erros
        /*
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                isDatabaseOk = true;
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        RetrieveDataFromDatabase();
    }

    // receives the name and the score, inserts it into an
    // object of type PlayerData and then transforms it into JSON
    public bool SendDataToDatabase(string name, float score)
    {
        // if (!isDatabaseOk) return false;        
        PlayerData pData = new PlayerData(name, score);
        string json = JsonUtility.ToJson(pData);
        // send the json file into the "players" structure in the database
        reference.Child("players").Child(AuthManager.instance.GetPlayerId()).SetRawJsonValueAsync(json);
        return true;
    }

    // retrieve data from database
    public void RetrieveDataFromDatabase()
    {
        isDatabaseOk = false;
        // retrieve all children of "players", sort by child
        // "playerScore", limit to the last 10 records (the highest).
        FirebaseDatabase.DefaultInstance.GetReference("players").OrderByChild("playerScore").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            //if any error occurred
            if (task.IsFaulted)
            {
                Debug.LogError("Error while loading database");
                isDatabaseOk = false;
            }
            // if it worked, get the result and associate it
            // to the variable of type Datasnapshot rawData
            else if (task.IsCompleted)
            {
                rawData = task.Result;
                isDatabaseOk = true;
            }
        });
    }

    public int[] GetScore()
    {
        return playersScore;
    }
    public string[] GetName()
    {
        return playersName;
    }

    // after the rawData has been loaded, it associates
    // the values found in playerScore and playerName in
    // the playersScore and playersName vectors
    public void GetLoadedData()
    {
        int counterPlayers = 0;
        playersName = new string[rawData.ChildrenCount];
        playersScore = new int[rawData.ChildrenCount];

        foreach (DataSnapshot child in rawData.Children)
        {
            // the first child refers to the immediate child
            // of "players". child.Child("playerName").Value
            // captures the value of the playerName key
            playersName[counterPlayers] = child.Child("playerName").Value.ToString();
            // same thing for playerScore child, but need to
            // pass to string and then pass to int.
            int.TryParse(child.Child("playerScore").Value.ToString(), out playersScore[counterPlayers]);
            counterPlayers++;
        }
    }

    public bool getResultSuccessfully = false;
    public int result;

    // this method is incomplete as it only checks the 10
    // results found in rawData. In a future update it will
    // check the entire bank if the player already exists
    // and if the score should be updated.
    public void GetOneChildScoreToCheckIfAtualScoreIsBigger(string pname, int score)
    {
        result = 0; // default when player isn't in database
        foreach (DataSnapshot child in rawData.Children)
        {
            if (pname == child.Child("playerName").Value.ToString()) //if player is in database
            {
                int oldScore; // get old score from database (line below)
                int.TryParse(child.Child("playerScore").Value.ToString(), out oldScore);
                if (score > oldScore) { result = 1; } // if actual score is bigger
                else { result = -1; } // if is lesser or equal
                break;
            }
        }
        getResultSuccessfully = true;
    }
}
