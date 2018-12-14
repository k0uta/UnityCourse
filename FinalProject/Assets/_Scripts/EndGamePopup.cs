using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePopup : MonoBehaviour {

    CanvasGroup canvasGroup;

    public GameObject playerResultPrefab;

    public Transform resultList;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;

        var playersArray = FindObjectsOfType<PlayerController>();

        List<PlayerController> players = new List<PlayerController>(playersArray);

        players.Sort((player, otherPlayer) => otherPlayer.score.CompareTo(player.score));

        int rank = 0;

        foreach (var player in players)
        {
            var playerResultObject = (GameObject)Instantiate(playerResultPrefab);

            var playerResult = new PlayerResult();
            playerResult.score = player.score;
            playerResult.playerName = player.playerName;
            playerResult.rank = ++rank;

            playerResultObject.GetComponent<PlayerResultView>().playerResult = playerResult;

            playerResultObject.transform.SetParent(resultList);
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
    }
}
