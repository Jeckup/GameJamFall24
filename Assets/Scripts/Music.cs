using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public static Music mp {get; private set;}

    private AudioSource _audioSource;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (mp != null && mp != this) 
        { 
            Destroy(gameObject);
        } 
        else 
        { 
            mp = this; 
        } 

        // if(GameObject.FindGameObjectsWithTag("MainMusic").Length > 1){
        //     Destroy(gameObject);
        //     return;
        // }
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    void Update(){
        if(SceneManager.GetActiveScene().name.CompareTo("Main Menu") == 0){
            StopMusic();
            Destroy(gameObject);
        }
    }
}
