using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : SingletonMonoBehaviour<VoiceManager> {

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip GameStartClip;
    [SerializeField]
    AudioClip GameEndClip;


    // ゲームスタート
    public void GameStart()
    {
        audioSource.PlayOneShot(GameStartClip);
    }

    // ゲームエンド
    public void GameEnd()
    {
        audioSource.PlayOneShot(GameEndClip);
    }

}

