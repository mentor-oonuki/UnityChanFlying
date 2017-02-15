using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager> {

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip BgmClip;


    // メインBGM
    public void MainBgmPlay()
    {
        audioSource.loop = true;
        audioSource.clip = BgmClip;
        audioSource.volume = 0.1f;
        audioSource.PlayDelayed(3.0f);
    }

    // メインBGM停止
    public void MainBgmStop()
    {
        audioSource.loop = false;
        audioSource.clip = BgmClip;
        audioSource.volume = 0.0f;
        audioSource.Stop();
    }
}
