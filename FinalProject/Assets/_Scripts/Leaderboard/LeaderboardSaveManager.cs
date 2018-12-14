using UnityEngine;

public class LeaderboardSaveManager : MonoBehaviour
{

    public LeaderboardInfo save;

    static LeaderboardSaveManager _instance;

    public static LeaderboardSaveManager Instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<LeaderboardSaveManager>()); }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    // Use this for initialization
    void Awake()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        var savedGame = JsonUtility.ToJson(save);
        PlayerPrefs.SetString("LeaderboardSave", savedGame);
    }

    public void LoadGame()
    {
        var loadedGame = PlayerPrefs.GetString("LeaderboardSave", string.Empty);
        save = JsonUtility.FromJson<LeaderboardInfo>(loadedGame) ?? new LeaderboardInfo();
    }

}