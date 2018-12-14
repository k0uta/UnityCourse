using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StageManager : NetworkBehaviour {
    [SyncVar(hook = "OnChangeTime")]
    public int secondsLeft = 60;

    public int roundTime = 60;

    public Text timerText;

    public Text scoreText;

    public CanvasGroup gameUI;

    public CanvasGroup endGamePopup;

    public InputField playerNameInput;

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
        endGamePopup.GetComponent<EndGamePopup>().Open();
    }

    void OnChangeTime(int _secondsLeft)
    {
        timerText.text = string.Format("Time Left: {0}:{1:00}", _secondsLeft / 60, _secondsLeft % 60);
    }

    void Reset()
    {
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
        var patrolBehaviours = FindObjectsOfType<PatrolBehaviour>();
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
