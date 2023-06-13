using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    public void OnSkipCutscene()
    {
        SceneManager.LoadScene("Skill_Choose");
    }
}
