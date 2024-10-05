using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scene_manager : MonoBehaviour
{

    public void PlayGame(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
