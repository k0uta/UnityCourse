using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public GameObject playerResultPrefab;

    public Transform resultList;

    public Transform leaderboardList;

    public CanvasGroup resultGroup;

    public CanvasGroup leaderboardGroup;

    public Text switchButtonText;

    public bool showingLeaderboard = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateMode();
    }

    void UpdateMode()
    {
        switchButtonText.text = showingLeaderboard ? "Results" : "Leaderboard";
        resultGroup.alpha = showingLeaderboard ? 0 : 1;
        leaderboardGroup.alpha = showingLeaderboard ? 1 : 0;
    }

    public void SwitchMode()
    {
        showingLeaderboard = !showingLeaderboard;
        UpdateMode();
    }

    public void Open()
    {
        for (int i = 0; i < resultList.childCount; i++)
        {
            Destroy(resultList.GetChild(i).gameObject);
        }

        for (int i = 0; i < leaderboardList.childCount; i++)
        {
            Destroy(leaderboardList.GetChild(i).gameObject);
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        var playersArray = FindObjectsOfType<PlayerController>();

        List<PlayerController> players = new List<PlayerController>(playersArray);

        players.Sort((player, otherPlayer) => otherPlayer.score.CompareTo(player.score));

        int rank = 0;

        var entries = LeaderboardSaveManager.Instance.save.entries;

        foreach (var player in players)
        {
            var playerResultObject = (GameObject)Instantiate(playerResultPrefab);

            var playerResult = new PlayerResult
            {
                score = player.score,
                playerName = player.playerName,
                rank = ++rank
            };

            playerResultObject.GetComponent<PlayerResultView>().playerResult = playerResult;

            playerResultObject.transform.SetParent(resultList);

            var leaderboardEntry = new LeaderboardEntry
            {
                playerName = player.playerName,
                score = player.score
            };

            entries.Add(leaderboardEntry);
        }

        entries.Sort((entry, otherEntry) => otherEntry.score.CompareTo(entry.score));

        LeaderboardSaveManager.Instance.SaveGame();

        int leaderboardRank = 0;

        foreach (var leaderboardEntry in entries)
        {
            var leaderboardResultObject = (GameObject)Instantiate(playerResultPrefab);

            var leaderboardResult = new PlayerResult
            {
                score = leaderboardEntry.score,
                playerName = leaderboardEntry.playerName,
                rank = ++leaderboardRank
            };

            leaderboardResultObject.GetComponent<PlayerResultView>().playerResult = leaderboardResult;

            leaderboardResultObject.transform.SetParent(leaderboardList);
        }
    }

    public void Rematch()
    {
        StageManager.Instance.Rematch();
    }
}
