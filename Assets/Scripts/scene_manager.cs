using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scene_manager : MonoBehaviour
{
    // [SerializeField] private AudioSource audioSource;
    public void PlayGame(string levelName)
    {
        // audioSource.Play();
        SceneManager.LoadSceneAsync(levelName);
    }

    public void Restart(){
        // audioSource.Play();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // audioSource.Play();
        Application.Quit();
    }
}
