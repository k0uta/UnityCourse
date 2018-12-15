using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StageManager : NetworkBehaviour {
    [SyncVar(hook = "OnChangeTime")]
    public float secondsLeft = 60;

    public float roundTime = 60;

    public Text timerText;

    public Text scoreText;

    public CanvasGroup gameUI;

    public CanvasGroup endGamePopup;

    public InputField playerNameInput;

    public AudioSource bgmAudioSource;

    List<PatrolBehaviour> patrolBehaviours;

    static StageManager _instance;

    public static StageManager Instance
    {
        get {
            return _instance ?? FindObjectOfType<StageManager>();
        }
    }

    private void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }

    public override void OnStartServer()
    {
        Reset();
    }

    private void Awake()
    {
        patrolBehaviours = new List<PatrolBehaviour>(FindObjectsOfType<PatrolBehaviour>());
    }

    private void Start()
    {
        Time.timeScale = 1;
        gameUI.alpha = 1;
    }

    IEnumerator GameCountdown()
    {
        while(secondsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            secondsLeft--;
        }

        RpcEndGame();
    }

    [ClientRpc]
    void RpcEndGame()
    {
        Time.timeScale = 0;
        gameUI.alpha = 0;

        bgmAudioSource.Stop();

        foreach (var patrolBehaviour in patrolBehaviours)
        {
            patrolBehaviour.Stop();
        }

        endGamePopup.GetComponent<EndGamePopup>().Open();
    }

    void OnChangeTime(float _secondsLeft)
    {
        float percentageCompleted = (roundTime - _secondsLeft) / roundTime;
        if(percentageCompleted > 0.5)
        {
            bgmAudioSource.pitch = 1.25f;
        } else if(percentageCompleted > 0.8)
        {
            bgmAudioSource.pitch = 1.5f;
        } else
        {
            bgmAudioSource.pitch = 1.0f;
        }

        timerText.text = string.Format("Time Left: {0}:{1:00}", (int)_secondsLeft / 60, (int) _secondsLeft % 60);
    }

    void Reset()
    {
        bgmAudioSource.Play();
        Time.timeScale = 1;
        gameUI.alpha = 1;

        if (!isServer)
        {
            return;
        }

        var players = FindObjectsOfType<PlayerController>();

        foreach (var player in players)
        {
            player.Reset();
        }

        secondsLeft = roundTime;
        StartCoroutine("GameCountdown");
        InteractableSpawner.Instance.Reset();
        
        foreach (var patrolBehaviour in patrolBehaviours)
        {
            patrolBehaviour.Reset();
        }
    }

    [ClientRpc]
    public void RpcRematch()
    {
        endGamePopup.GetComponent<EndGamePopup>().canvasGroup.alpha = 0;
        Reset();
    }

    public void Rematch()
    {
        if(!isServer)
        {
            return;
        }
        RpcRematch();
    }
}
