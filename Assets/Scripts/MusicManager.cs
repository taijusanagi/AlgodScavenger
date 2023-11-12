using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource effectSource;
    public static MusicManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayEffect()
    {
        effectSource.Play();
    }

    private void OnLevelWasLoaded(int index)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
        {
            musicSource.Play();
        }
    }
}
