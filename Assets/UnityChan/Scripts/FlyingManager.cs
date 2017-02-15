using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingManager : SingletonMonoBehaviour<FlyingManager> {

    public enum State
    {
        GameStart,
        GameEnd
    }
    public State GameState;


	void Start () {
        GameState = State.GameStart;
        GameLoop();
	}

    // ゲームループ
    public void GameLoop()
    {
        switch (GameState)
        {
            case State.GameStart:
                GameStart();
                break;

            case State.GameEnd:
                GameEnd();
                break;
        }
    }

    // ゲームループ呼び出し
    public void GameLoop(State state)
    {
        GameState = state;
        GameLoop();
    }

    // ゲーム開始
    private void GameStart()
    {
        VoiceManager.Instance.GameStart();
        MusicManager.Instance.MainBgmPlay();
        Timer.Instance.TimerStart();
    }

    //　ゲーム終了
    private void GameEnd()
    {
        Timer.Instance.TimerStop();
        MusicManager.Instance.MainBgmStop();
        VoiceManager.Instance.GameEnd();
        SoundManager.instance.Goal();
    }

}
