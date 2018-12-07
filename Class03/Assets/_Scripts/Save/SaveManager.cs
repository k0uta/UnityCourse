using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public SaveInfo save;

    static SaveManager _instance;

    public static SaveManager Instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<SaveManager>()); }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    // Use this for initialization
    void Awake () {
        LoadGame();
	}

    public void SaveGame()
    {
        var savedGame = JsonUtility.ToJson(save);
        PlayerPrefs.SetString("Save", savedGame);
    }

    public void LoadGame()
    {
        var loadedGame = PlayerPrefs.GetString("Save", string.Empty);
        save = JsonUtility.FromJson<SaveInfo>(loadedGame) ?? new SaveInfo();
    }

}
