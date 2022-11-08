using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    private VideoPlayer cutscene;

    private double cutsceneTime = 0;

    private void Awake()
    {
        cutscene = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (cutsceneTime >= cutscene.length + 2)
        {
            SceneManager.LoadScene("Skill_Choose");
        }

        else
            cutsceneTime += Time.deltaTime;
    }
}
