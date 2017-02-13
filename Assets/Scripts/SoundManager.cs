using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip GoalClip;


    // ゴール
    public void Goal()
    {
        audioSource.PlayOneShot(GoalClip);
    }

}
