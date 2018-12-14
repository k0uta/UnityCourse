using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StageManager : NetworkBehaviour {
    [SyncVar(hook = "OnChangeTime")]
    public int secondsLeft = 60;

    public Text timerText;

    public Text scoreText;

    static StageManager _instance;

    public CanvasGroup gameUI;

    public CanvasGroup endGamePopup;

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
        StartCoroutine("GameCountdown");
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
        timerText.text = string.Format("Time Left: {0}:{1:00}", (int)_secondsLeft / 60, _secondsLeft);
    }
}
