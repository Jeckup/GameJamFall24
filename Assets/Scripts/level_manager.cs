using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class level_manager : MonoBehaviour
{
    public void PlayGame(string levelName)
    {
        SceneManager.LoadSceneAsync("Test Scene");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
